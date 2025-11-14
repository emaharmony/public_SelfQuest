Shader "GE/NightVision"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Gain ("Gain", Range(0,4)) = 1.8
        _Noise ("Noise", Range(0,1)) = 0.25
        _Vignette ("Vignette", Range(0,1)) = 0.65
        _Scanline ("Scanline", Range(0,1)) = 0.25
        _BloomThreshold ("Bloom Threshold", Range(0,1)) = 0.7
        _BloomAmount ("Bloom Amount", Range(0,2)) = 0.6
        _Tint ("Green Tint", Color) = (0.2,1,0.3,1)
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

            sampler2D _MainTex; float4 _MainTex_TexelSize;
            float _Gain, _Noise, _Vignette, _Scanline, _BloomThreshold, _BloomAmount; float4 _Tint;

            struct appdata { float4 vertex:POSITION; float2 uv:TEXCOORD0; };
            struct v2f { float2 uv:TEXCOORD0; float4 vertex:SV_POSITION; };
            v2f vert(appdata v){ v2f o; o.vertex = UnityObjectToClipPos(v.vertex); o.uv=v.uv; return o; }

            float hash21(float2 p){ p = frac(p*float2(91.32,12.12)); p += dot(p,p+19.19); return frac(p.x*p.y); }

            fixed4 frag(v2f i) : SV_Target
            {
                float3 col = tex2D(_MainTex, i.uv).rgb;
                // Amplify luminance mainly in green
                float l = dot(col, float3(0.299,0.587,0.114));
                float3 gcol = float3(0, l, 0) * _Gain;
                col = saturate(gcol) * _Tint.rgb;

                // Simple threshold bloom (soft)
                float bright = saturate((l - _BloomThreshold) / max(1e-3, 1.0 - _BloomThreshold));
                float2 px = _MainTex_TexelSize.xy * 1.5;
                float3 b = tex2D(_MainTex, i.uv + float2(px.x,0)).rgb + tex2D(_MainTex, i.uv + float2(-px.x,0)).rgb +
                           tex2D(_MainTex, i.uv + float2(0,px.y)).rgb + tex2D(_MainTex, i.uv + float2(0,-px.y)).rgb;
                b = (b*0.25).xxx; // brightness only
                col += b * bright * _BloomAmount;

                // Scanlines
                float scan = (fmod(i.uv.y * _MainTex_TexelSize.w, 2.0) < 1.0) ? (1.0 - _Scanline) : 1.0;
                col *= scan;

                // Vignette
                float2 uv = i.uv * 2 - 1; float r2 = dot(uv,uv);
                float vig = smoothstep(1.0, 0.2, r2);
                col *= lerp(1.0, vig, _Vignette);

                // Noise
                float n = hash21(i.uv * _MainTex_TexelSize.zw + _Time.yx*41.7);
                col = saturate(col + (n - 0.5) * _Noise);
                return float4(col,1);
            }
            ENDCG
        }
    }
}
