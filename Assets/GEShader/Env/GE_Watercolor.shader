Shader "GE/Watercolor"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BlurRadius ("Blur Radius", Range(0,3)) = 1.0
        _Steps ("Posterize Steps", Float) = 8
        _EdgeSoftness ("Edge Softness", Range(0,3)) = 1.2
        _Grain ("Paper Grain", Range(0,1)) = 0.2
        _Tint ("Paper Tint", Color) = (1,1,1,1)
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
            float _BlurRadius, _Steps, _EdgeSoftness, _Grain; float4 _Tint;

            struct appdata { float4 vertex:POSITION; float2 uv:TEXCOORD0; };
            struct v2f { float2 uv:TEXCOORD0; float4 vertex:SV_POSITION; };
            v2f vert(appdata v){ v2f o; o.vertex = UnityObjectToClipPos(v.vertex); o.uv=v.uv; return o; }

            float hash21(float2 p){ p = frac(p*float2(23.34,51.31)); p += dot(p,p+24.13); return frac(p.x*p.y); }

            float3 blur9(float2 uv)
            {
                float2 px = _MainTex_TexelSize.xy * _BlurRadius;
                float3 acc = 0; float w = 0;
                // simple 3x3 gaussian-like weights
                float k[3]; k[0]=0.075; k[1]=0.124; k[2]=0.075;
                for(int y=-1;y<=1;y++) for(int x=-1;x<=1;x++)
                {
                    float ww = k[abs(x)]*k[abs(y)];
                    acc += tex2D(_MainTex, uv + float2(x*px.x, y*px.y)).rgb * ww; w += ww;
                }
                return acc / max(w,1e-4);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float3 b = blur9(i.uv);
                // Posterize after blur for watercolor flat washes
                float steps = max(_Steps, 2.0);
                float3 col = floor(b * steps) / steps;

                // Soft edges via subtracting a gentle Sobel
                float2 px = _MainTex_TexelSize.xy * _BlurRadius;
                float3 cN = tex2D(_MainTex, i.uv + float2(0,-px.y)).rgb;
                float3 cS = tex2D(_MainTex, i.uv + float2(0, px.y)).rgb;
                float3 cE = tex2D(_MainTex, i.uv + float2( px.x,0)).rgb;
                float3 cW = tex2D(_MainTex, i.uv + float2(-px.x,0)).rgb;
                float3 w = float3(0.299,0.587,0.114);
                float gx = dot(cE - cW, w);
                float gy = dot(cS - cN, w);
                float edge = saturate(sqrt(gx*gx + gy*gy) * _EdgeSoftness);
                col *= (1.0 - edge*0.6);

                // Paper grain and tint
                float n = hash21(i.uv * _MainTex_TexelSize.zw + _Time.yx*19.3);
                col = saturate(col * _Tint.rgb + (n-0.5)*_Grain);
                return float4(col,1);
            }
            ENDCG
        }
    }
}
