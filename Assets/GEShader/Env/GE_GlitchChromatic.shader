Shader "GE/GlitchChromatic"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _GlitchIntensity ("Glitch Intensity", Range(0,1)) = 0.3
        _ChromaticOffset ("Chromatic Offset", Range(0,5)) = 1.5
        _LineJitter ("Line Jitter", Range(0,5)) = 1.0
        _BlockSize ("Block Size (px)", Range(2,64)) = 12
        _ColorBleed ("Color Bleed", Range(0,1)) = 0.25
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
            float _GlitchIntensity, _ChromaticOffset, _LineJitter, _BlockSize, _ColorBleed;

            struct appdata { float4 vertex:POSITION; float2 uv:TEXCOORD0; };
            struct v2f { float2 uv:TEXCOORD0; float4 vertex:SV_POSITION; };
            v2f vert(appdata v){ v2f o; o.vertex = UnityObjectToClipPos(v.vertex); o.uv=v.uv; return o; }

            float hash21(float2 p){ p = frac(p*float2(43758.5453, 12345.6789)); p += dot(p,p+34.345); return frac(p.x*p.y); }

            float2 barrel(float2 uv, float k)
            {
                float2 cc = uv * 2 - 1; float r2 = dot(cc,cc); cc *= (1 + k * r2); return (cc * 0.5 + 0.5);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;

                // Random horizontal line shifts
                float lineNoise = (hash21(float2(0.0, floor(i.uv.y * _MainTex_TexelSize.w))) - 0.5);
                uv.x += lineNoise * _LineJitter * 0.005 * _GlitchIntensity;

                // Blocky displacement (pixel blocks)
                float2 block = floor(i.uv * float2(_MainTex_TexelSize.z, _MainTex_TexelSize.w) / _BlockSize) * _BlockSize;
                float2 ruv = block / float2(_MainTex_TexelSize.z, _MainTex_TexelSize.w);
                float2 rnd = float2(hash21(ruv + _Time.yy), hash21(ruv + _Time.yx));
                uv += (rnd - 0.5) * 0.01 * _GlitchIntensity;

                // Chromatic aberration with slight barrel
                float co = _ChromaticOffset * 0.002 * _GlitchIntensity;
                float2 uvr = barrel(uv + float2( co, 0),  0.05 * _GlitchIntensity);
                float2 uvg = barrel(uv,                        0.03 * _GlitchIntensity);
                float2 uvb = barrel(uv + float2(-co, 0),  0.05 * _GlitchIntensity);

                float r = tex2D(_MainTex, uvr).r;
                float g = tex2D(_MainTex, uvg).g;
                float b = tex2D(_MainTex, uvb).b;
                float3 col = float3(r,g,b);

                // Color bleed
                float3 bleed = tex2D(_MainTex, uv + float2(0.002, 0)).rgb;
                col = lerp(col, bleed, _ColorBleed * _GlitchIntensity);

                // Occasional full-line dropouts
                float dropout = step(0.995, hash21(float2(_Time.y*0.5, i.uv.y)));
                col *= (1.0 - dropout * _GlitchIntensity);

                return float4(col,1);
            }
            ENDCG
        }
    }
}
