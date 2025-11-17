Shader "GE/CelToon"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _EdgeThickness ("Edge Thickness", Range(0.5,3)) = 1
        _EdgeIntensity ("Edge Intensity", Range(0,3)) = 1.4
        _PosterizeSteps ("Color Steps", Float) = 5
        _Saturation ("Saturation", Range(0,2)) = 1.1
        _EdgeColor ("Edge Color", Color) = (0,0,0,1)
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
            float _EdgeThickness, _EdgeIntensity, _PosterizeSteps, _Saturation; float4 _EdgeColor;

            struct appdata { float4 vertex:POSITION; float2 uv:TEXCOORD0; };
            struct v2f { float2 uv:TEXCOORD0; float4 vertex:SV_POSITION; };

            v2f vert(appdata v){ v2f o; o.vertex = UnityObjectToClipPos(v.vertex); o.uv=v.uv; return o; }

            float3 sat(float3 c, float s)
            {
                float l = dot(c, float3(0.299,0.587,0.114));
                return lerp(float3(l,l,l), c, s);
            }

            float3 sample(float2 uv){ return tex2D(_MainTex, uv).rgb; }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 px = _MainTex_TexelSize.xy * _EdgeThickness;
                float3 c  = sample(i.uv);
                float3 cN = sample(i.uv + float2(0,-px.y));
                float3 cS = sample(i.uv + float2(0, px.y));
                float3 cE = sample(i.uv + float2( px.x,0));
                float3 cW = sample(i.uv + float2(-px.x,0));
                float3 cNE= sample(i.uv + float2( px.x,-px.y));
                float3 cNW= sample(i.uv + float2(-px.x,-px.y));
                float3 cSE= sample(i.uv + float2( px.x, px.y));
                float3 cSW= sample(i.uv + float2(-px.x, px.y));

                float3 lumW = float3(0.299,0.587,0.114);
                float l  = dot(c,lumW);
                float lN = dot(cN,lumW); float lS=dot(cS,lumW);
                float lE = dot(cE,lumW); float lW=dot(cW,lumW);
                float lNE=dot(cNE,lumW); float lNW=dot(cNW,lumW);
                float lSE=dot(cSE,lumW); float lSW=dot(cSW,lumW);

                float gx = (lNE + 2*lE + lSE) - (lNW + 2*lW + lSW);
                float gy = (lSW + 2*lS + lSE) - (lNW + 2*lN + lNE);
                float edge = saturate(sqrt(gx*gx + gy*gy) * _EdgeIntensity);

                // Posterize and slightly boost saturation to mimic cel shading
                float steps = max(_PosterizeSteps, 2.0);
                float3 col = floor(c * steps) / steps;
                col = sat(col, _Saturation);

                // Composite outline
                col = lerp(col, _EdgeColor.rgb, edge);

                return float4(col, 1);
            }
            ENDCG
        }
    }
}
