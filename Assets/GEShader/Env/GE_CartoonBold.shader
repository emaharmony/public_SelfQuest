Shader "GE/CartoonBold"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _EdgeThickness ("Edge Thickness", Range(0.5,3)) = 1.5
        _EdgeIntensity ("Edge Intensity", Range(0,4)) = 2.2
        _Steps ("Color Steps", Float) = 3
        _EdgeColor ("Edge Color", Color) = (0,0,0,1)
        _PaperWhite ("Paper White", Range(0,1)) = 0.0
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

            sampler2D _MainTex; float4 _MainTex_TexelSize; float4 _EdgeColor;
            float _EdgeThickness, _EdgeIntensity, _Steps, _PaperWhite;

            struct appdata { float4 vertex:POSITION; float2 uv:TEXCOORD0; };
            struct v2f { float2 uv:TEXCOORD0; float4 vertex:SV_POSITION; };
            v2f vert(appdata v){ v2f o; o.vertex = UnityObjectToClipPos(v.vertex); o.uv=v.uv; return o; }

            float3 S(float2 uv){ return tex2D(_MainTex, uv).rgb; }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 px = _MainTex_TexelSize.xy * _EdgeThickness;
                float3 c  = S(i.uv);
                float3 n  = S(i.uv + float2(0,-px.y));
                float3 s  = S(i.uv + float2(0, px.y));
                float3 e  = S(i.uv + float2( px.x,0));
                float3 w  = S(i.uv + float2(-px.x,0));
                float3 ne = S(i.uv + float2( px.x,-px.y));
                float3 nw = S(i.uv + float2(-px.x,-px.y));
                float3 se = S(i.uv + float2( px.x, px.y));
                float3 sw = S(i.uv + float2(-px.x, px.y));

                float3 lumW = float3(0.299,0.587,0.114);
                float ln  = dot(n,lumW), ls=dot(s,lumW), le=dot(e,lumW), lw=dot(w,lumW);
                float lne=dot(ne,lumW), lnw=dot(nw,lumW), lse=dot(se,lumW), lsw=dot(sw,lumW);
                float gx = (lne + 2*le + lse) - (lnw + 2*lw + lsw);
                float gy = (lsw + 2*ls + lse) - (lnw + 2*ln + lne);
                float edge = saturate(sqrt(gx*gx + gy*gy) * _EdgeIntensity);

                // Heavy posterization for bold cartoon look
                float steps = max(_Steps, 2.0);
                float3 col = floor(c * steps) / steps;
                // Slight lift for paper white look
                col = lerp(col, 1.0.xxx, _PaperWhite*0.2);

                // Overlay thick outlines
                col = lerp(col, _EdgeColor.rgb, edge);
                return float4(col, 1);
            }
            ENDCG
        }
    }
}
