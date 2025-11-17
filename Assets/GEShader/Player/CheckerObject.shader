Shader "SelfQuest/Player/CheckerObject"
{
    Properties
    {
        _ColorA ("Color A", Color) = (0.1,0.1,0.1,1)
        _ColorB ("Color B", Color) = (0.9,0.9,0.9,1)
        _Scale ("Checker Scale", Float) = 4
        _Smoothness ("Smoothness", Range(0,1)) = 0.0
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 150

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0
        #include "UnityCG.cginc"
        fixed4 _ColorA, _ColorB;
        float _Scale;
        half _Smoothness, _Metallic;

        struct Input
        {
            float3 worldPos;
        };

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            float3 objPos = mul(unity_WorldToObject, float4(IN.worldPos,1)).xyz * _Scale;
            float3 p = floor(objPos);
            float parity = fmod(p.x + p.y + p.z, 2.0);
            fixed3 col = (parity < 1.0) ? _ColorA.rgb : _ColorB.rgb;
            o.Albedo = col;
            o.Metallic = _Metallic;
            o.Smoothness = _Smoothness;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
