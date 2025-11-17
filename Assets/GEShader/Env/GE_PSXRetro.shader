Shader "GE/PSXRetro"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ColorSteps ("Color Steps", Float) = 16
        _JitterAmount ("UV Jitter", Range(0,2)) = 0.5
        _ScanlineIntensity ("Scanline Intensity", Range(0,1)) = 0.15
        _Vignette ("Vignette", Range(0,1)) = 0.25
        _Noise ("Film Noise", Range(0,1)) = 0.1
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

            sampler2D _MainTex;
            float4 _MainTex_TexelSize; // x=1/w, y=1/h, z=w, w=h

            float _ColorSteps;
            float _JitterAmount;
            float _ScanlineIntensity;
            float _Vignette;
            float _Noise;

            struct appdata { float4 vertex:POSITION; float2 uv:TEXCOORD0; };
            struct v2f { float2 uv:TEXCOORD0; float4 vertex:SV_POSITION; };

            v2f vert(appdata v)
            {
                v2f o; o.vertex = UnityObjectToClipPos(v.vertex); o.uv = v.uv; return o;
            }

            float hash21(float2 p){ p = frac(p*float2(123.34,456.21)); p += dot(p,p+45.32); return frac(p.x*p.y); }

            fixed4 frag(v2f i) : SV_Target
            {
                // Horizontal per-scanline wobble
                float y = i.uv.y;
                float wob = (sin(y * _MainTex_TexelSize.w * 0.015 + _Time.y*1.3) * 0.5 + 0.5);
                wob = (wob - 0.5) * _JitterAmount * 0.003; // ~0.3% screen width

                // Subpixel jitter
                float2 jitter = float2(wob, (hash21(float2(_Time.y, y)) - 0.5) * _JitterAmount * 0.002);

                // Slight affine-like skew based on y (fake perspective wobble)
                float skew = (y - 0.5) * _JitterAmount * 0.004;
                float2 uv = i.uv + jitter + float2(skew, 0);

                fixed4 col = tex2D(_MainTex, uv);

                // Color quantization mimicking 15-bit color (approx)
                float steps = max(_ColorSteps, 2.0);
                col.rgb = floor(col.rgb * steps) / steps;

                // Simple 1px alternating scanlines
                float scan = (fmod(i.uv.y * _MainTex_TexelSize.w, 2.0) < 1.0) ? (1.0 - _ScanlineIntensity) : 1.0;
                col.rgb *= scan;

                // Vignette
                float2 centered = (i.uv - 0.5) * 2.0;
                float v = saturate(1.0 - dot(centered, centered));
                col.rgb *= lerp(1.0 - _Vignette, 1.0, v);

                // Film noise
                float n = hash21(i.uv * _MainTex_TexelSize.zw + _Time.yy * 37.0);
                col.rgb = saturate(col.rgb + (n - 0.5) * _Noise);

                return col;
            }
            ENDCG
        }
    }
}
