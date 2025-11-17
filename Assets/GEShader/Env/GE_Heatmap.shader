Shader "GE/Heatmap"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Contrast ("Contrast", Range(0,3)) = 1.0
        _Invert ("Invert", Range(0,1)) = 0
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

            sampler2D _MainTex; float _Contrast; float _Invert;

            struct appdata { float4 vertex:POSITION; float2 uv:TEXCOORD0; };
            struct v2f { float2 uv:TEXCOORD0; float4 vertex:SV_POSITION; };
            v2f vert(appdata v){ v2f o; o.vertex = UnityObjectToClipPos(v.vertex); o.uv=v.uv; return o; }

            float3 heatColor(float t)
            {
                // A simple black->blue->purple->red->yellow->white gradient
                float3 c1 = float3(0.0, 0.0, 0.0);
                float3 c2 = float3(0.0, 0.0, 0.8);
                float3 c3 = float3(0.5, 0.0, 0.5);
                float3 c4 = float3(0.8, 0.0, 0.0);
                float3 c5 = float3(1.0, 0.6, 0.0);
                float3 c6 = float3(1.0, 1.0, 1.0);
                float seg = t * 5.0; // 5 segments between 6 colors
                int i = (int)floor(seg);
                float f = frac(seg);
                if(i==0) return lerp(c1,c2,f);
                if(i==1) return lerp(c2,c3,f);
                if(i==2) return lerp(c3,c4,f);
                if(i==3) return lerp(c4,c5,f);
                return lerp(c5,c6,f);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float3 col = tex2D(_MainTex, i.uv).rgb;
                float l = dot(col, float3(0.299,0.587,0.114));
                l = saturate((l - 0.5) * _Contrast + 0.5);
                l = lerp(l, 1.0 - l, _Invert);
                float3 h = heatColor(l);
                return float4(h, 1);
            }
            ENDCG
        }
    }
}
