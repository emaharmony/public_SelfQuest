Shader "GE/DuotonePoster"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ShadowColor ("Shadow Color", Color) = (0.06,0.07,0.2,1)
        _MidColor ("Mid Color", Color) = (0.3,0.2,0.6,1)
        _HighlightColor ("Highlight Color", Color) = (1,0.9,0.7,1)
        _Threshold1 ("Threshold 1", Range(0,1)) = 0.35
        _Threshold2 ("Threshold 2", Range(0,1)) = 0.7
        _Contrast ("Contrast", Range(0,2)) = 1.0
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
            float4 _ShadowColor, _MidColor, _HighlightColor; float _Threshold1, _Threshold2, _Contrast;

            struct appdata { float4 vertex:POSITION; float2 uv:TEXCOORD0; };
            struct v2f { float2 uv:TEXCOORD0; float4 vertex:SV_POSITION; };
            v2f vert(appdata v){ v2f o; o.vertex = UnityObjectToClipPos(v.vertex); o.uv=v.uv; return o; }

            fixed4 frag(v2f i) : SV_Target
            {
                float3 c = tex2D(_MainTex, i.uv).rgb;
                float l = dot(c, float3(0.2126,0.7152,0.0722));
                l = saturate((l - 0.5) * _Contrast + 0.5);
                float3 outc = (l < _Threshold1) ? _ShadowColor.rgb : (l < _Threshold2 ? _MidColor.rgb : _HighlightColor.rgb);
                return float4(outc, 1);
            }
            ENDCG
        }
    }
}
