Shader "GE/CRTVHS"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Curvature ("Screen Curvature", Range(0,0.5)) = 0.12
        _ScanlineIntensity ("Scanline Intensity", Range(0,1)) = 0.25
        _Vignette ("Vignette", Range(0,1)) = 0.35
        _Noise ("Noise", Range(0,1)) = 0.08
        _Bleed ("Color Bleed", Range(0,1)) = 0.25
        _Warp ("Horizontal Warp", Range(0,1)) = 0.2
        _Jitter ("Frame Jitter", Range(0,1)) = 0.15
        _Tint ("Tint", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Cull Off ZWrite Off ZTest Always
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex; float4 _MainTex_TexelSize; float4 _Tint;
            float _Curvature, _ScanlineIntensity, _Vignette, _Noise, _Bleed, _Warp, _Jitter;

            struct appdata { float4 vertex:POSITION; float2 uv:TEXCOORD0; };
            struct v2f { float2 uv:TEXCOORD0; float4 vertex:SV_POSITION; };
            v2f vert(appdata v){ v2f o; o.vertex = UnityObjectToClipPos(v.vertex); o.uv=v.uv; return o; }

            float hash21(float2 p){ p = frac(p*float2(121.34, 423.23)); p += dot(p,p+45.32); return frac(p.x*p.y); }

            float2 barrel(float2 uv, float k)
            {
                float2 cc = uv*2-1; float r2 = dot(cc,cc); cc *= (1 + k*r2); return cc*0.5 + 0.5;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Vertical frame jitter and horizontal warp
                float y = i.uv.y + (hash21(float2(_Time.y*0.5,0.0)) - 0.5) * 0.004 * _Jitter;
                float wav = sin(y * _MainTex_TexelSize.w * 0.012 + _Time.y*2.0) * _Warp * 0.01;
                float2 uv = float2(i.uv.x + wav, y);

                // CRT curvature and chroma bleed
                uv = barrel(uv, _Curvature);
                float2 off = float2(1.5/_MainTex_TexelSize.z, 0);
                float r = tex2D(_MainTex, uv + off).r;
                float g = tex2D(_MainTex, uv).g;
                float b = tex2D(_MainTex, uv - off).b;
                float3 col = float3(r,g,b);
                col = lerp(tex2D(_MainTex, uv).rgb, col, _Bleed);
                col *= _Tint.rgb;

                // Scanlines (alternating dark rows)
                float scan = (fmod(uv.y * _MainTex_TexelSize.w, 2.0) < 1.0) ? (1.0 - _ScanlineIntensity) : 1.0;
                col *= scan;

                // Vignette
                float2 cuv = uv*2-1; float r2 = dot(cuv,cuv);
                float vig = smoothstep(1.0, 0.2, r2);
                col *= lerp(1.0, vig, _Vignette);

                // Noise/grain
                float n = hash21(uv * _MainTex_TexelSize.zw + _Time.yx*33.7);
                col = saturate(col + (n - 0.5) * _Noise);

                return float4(col,1);
            }
            ENDCG
        }
    }
}
