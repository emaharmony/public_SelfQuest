Shader "GE/HandDrawn"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _EdgeThickness ("Edge Thickness", Range(0.5,3)) = 1
        _EdgeIntensity ("Edge Intensity", Range(0,2)) = 1
        _PosterizeSteps ("Posterize Steps", Float) = 6
        _GrainAmount ("Paper Grain", Range(0,1)) = 0.15
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
            float4 _MainTex_TexelSize;

            float _EdgeThickness;
            float _EdgeIntensity;
            float _PosterizeSteps;
            float _GrainAmount;

            struct appdata { float4 vertex:POSITION; float2 uv:TEXCOORD0; };
            struct v2f { float2 uv:TEXCOORD0; float4 vertex:SV_POSITION; };

            v2f vert(appdata v)
            {
                v2f o; o.vertex = UnityObjectToClipPos(v.vertex); o.uv = v.uv; return o;
            }

            float hash21(float2 p){ p = frac(p*float2(34.34,12.92)); p += dot(p,p+23.17); return frac(p.x*p.y); }

            float3 sampleCol(float2 uv){ return tex2D(_MainTex, uv).rgb; }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 px = _MainTex_TexelSize.xy * _EdgeThickness;

                // Sobel edge in luma
                float3 c  = sampleCol(i.uv);
                float3 cN = sampleCol(i.uv + float2(0, -px.y));
                float3 cS = sampleCol(i.uv + float2(0,  px.y));
                float3 cE = sampleCol(i.uv + float2( px.x, 0));
                float3 cW = sampleCol(i.uv + float2(-px.x, 0));
                float3 cNE= sampleCol(i.uv + float2( px.x, -px.y));
                float3 cNW= sampleCol(i.uv + float2(-px.x, -px.y));
                float3 cSE= sampleCol(i.uv + float2( px.x,  px.y));
                float3 cSW= sampleCol(i.uv + float2(-px.x,  px.y));

                float l  = dot(c,  float3(0.299,0.587,0.114));
                float lN = dot(cN, float3(0.299,0.587,0.114));
                float lS = dot(cS, float3(0.299,0.587,0.114));
                float lE = dot(cE, float3(0.299,0.587,0.114));
                float lW = dot(cW, float3(0.299,0.587,0.114));
                float lNE= dot(cNE,float3(0.299,0.587,0.114));
                float lNW= dot(cNW,float3(0.299,0.587,0.114));
                float lSE= dot(cSE,float3(0.299,0.587,0.114));
                float lSW= dot(cSW,float3(0.299,0.587,0.114));

                float gx = (lNE + 2.0*lE + lSE) - (lNW + 2.0*lW + lSW);
                float gy = (lSW + 2.0*lS + lSE) - (lNW + 2.0*lN + lNE);
                float edge = saturate(sqrt(gx*gx + gy*gy) * _EdgeIntensity);

                // Posterize underlying color
                float steps = max(_PosterizeSteps, 2.0);
                float3 col = floor(c * steps) / steps;

                // Apply ink lines by darkening with edge mask
                col *= (1.0 - edge);

                // Paper grain
                float n = hash21(i.uv * _MainTex_TexelSize.zw + _Time.yx * 57.0);
                col = saturate(col + (n - 0.5) * _GrainAmount);

                return float4(col, 1);
            }
            ENDCG
        }
    }
}
