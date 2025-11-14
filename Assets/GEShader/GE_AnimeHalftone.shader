Shader "GE/AnimeHalftone"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _EdgeThickness ("Edge Thickness", Range(0.5,3)) = 1
        _EdgeIntensity ("Edge Intensity", Range(0,3)) = 1.8
        _Steps ("Color Steps", Float) = 4
        _DotScale ("Halftone Dot Scale", Range(2,32)) = 12
        _DotIntensity ("Dot Intensity", Range(0,1)) = 0.5
        _Saturation ("Saturation", Range(0,2)) = 1.25
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
            float _EdgeThickness, _EdgeIntensity, _Steps, _DotScale, _DotIntensity, _Saturation;

            struct appdata { float4 vertex:POSITION; float2 uv:TEXCOORD0; };
            struct v2f { float2 uv:TEXCOORD0; float4 vertex:SV_POSITION; };
            v2f vert(appdata v){ v2f o; o.vertex = UnityObjectToClipPos(v.vertex); o.uv=v.uv; return o; }

            float3 sat(float3 c, float s){ float l = dot(c, float3(0.299,0.587,0.114)); return lerp(float3(l,l,l), c, s); }
            float3 sampleC(float2 uv){ return tex2D(_MainTex, uv).rgb; }

            float halftone(float2 uv, float luma)
            {
                // Rotate UVs 45 degrees for diagonal screen tone
                float2 p = (uv - 0.5) * float2(_MainTex_TexelSize.z, _MainTex_TexelSize.w);
                float s = 0.7071; float2x2 R = float2x2(s, -s, s, s);
                p = mul(R, p);
                p /= _DotScale; // scale controls dot size
                float2 g = frac(p) - 0.5;
                float d = length(g);
                // Dots larger in darker regions
                float r = saturate(0.5 + (1.0 - luma) * 0.5);
                return smoothstep(r, r - 0.15, d);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 px = _MainTex_TexelSize.xy * _EdgeThickness;
                float3 c  = sampleC(i.uv);
                float3 cN = sampleC(i.uv + float2(0,-px.y));
                float3 cS = sampleC(i.uv + float2(0, px.y));
                float3 cE = sampleC(i.uv + float2( px.x,0));
                float3 cW = sampleC(i.uv + float2(-px.x,0));
                float3 cNE= sampleC(i.uv + float2( px.x,-px.y));
                float3 cNW= sampleC(i.uv + float2(-px.x,-px.y));
                float3 cSE= sampleC(i.uv + float2( px.x, px.y));
                float3 cSW= sampleC(i.uv + float2(-px.x, px.y));

                float3 w = float3(0.299,0.587,0.114);
                float l  = dot(c,w);
                float lN = dot(cN,w), lS=dot(cS,w), lE=dot(cE,w), lW=dot(cW,w);
                float lNE=dot(cNE,w), lNW=dot(cNW,w), lSE=dot(cSE,w), lSW=dot(cSW,w);
                float gx = (lNE + 2*lE + lSE) - (lNW + 2*lW + lSW);
                float gy = (lSW + 2*lS + lSE) - (lNW + 2*lN + lNE);
                float edge = saturate(sqrt(gx*gx + gy*gy) * _EdgeIntensity);

                // Flat steps and color punch
                float steps = max(_Steps, 2.0);
                float3 col = floor(c * steps) / steps;
                col = sat(col, _Saturation);

                // Halftone overlay based on luma
                float tone = halftone(i.uv, l);
                col = lerp(col, col * tone, _DotIntensity);

                // Ink outlines
                col *= (1.0 - edge);
                return float4(col, 1);
            }
            ENDCG
        }
    }
}
