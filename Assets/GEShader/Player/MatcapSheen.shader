Shader "SelfQuest/Player/MatcapSheen"
{
    Properties
    {
        _Tint ("Tint", Color) = (1,1,1,1)
        _Matcap ("Matcap (RGB)", 2D) = "gray" {}
        _Sheen ("Sheen Strength", Range(0,2)) = 0.5
        _Smoothness ("Smoothness", Range(0,1)) = 0.3
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "Queue"="Geometry" "RenderType"="Opaque" }
        LOD 200
        Pass
        {
            Tags { "LightMode"="ForwardBase" }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #include "UnityCG.cginc"
            sampler2D _Matcap;
            fixed4 _Tint;
            half _Sheen;

            struct appdata {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct v2f {
                float4 pos : SV_POSITION;
                float3 viewN : TEXCOORD0;
            };
            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                // view-space normal
                float3 n = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.normal));
                o.viewN = n;
                return o;
            }
            fixed4 frag(v2f i) : SV_Target
            {
                float3 n = normalize(i.viewN);
                float2 uv = n.xy * 0.5 + 0.5;
                fixed3 m = tex2D(_Matcap, uv).rgb;
                fixed3 col = m * _Tint.rgb;
                // simple sheen boost near grazing angles using nz
                float sheen = pow(1 - saturate(abs(n.z)), 2) * _Sheen;
                col += sheen * _Tint.rgb;
                return fixed4(saturate(col), 1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
