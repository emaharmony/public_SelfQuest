Shader "SelfQuest/Player/GradientFresnel"
{
    Properties
    {
        _TopColor ("Top Color", Color) = (1,0.8,0.6,1)
        _BottomColor ("Bottom Color", Color) = (0.2,0.3,0.6,1)
        _HeightScale ("Height Scale", Float) = 1.0
        _FresnelColor ("Fresnel Color", Color) = (0.5,0.9,1,1)
        _FresnelPower ("Fresnel Power", Range(0.5,8)) = 2
        _Smoothness ("Smoothness", Range(0,1)) = 0.1
        _Metallic ("Metallic", Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0
        fixed4 _TopColor, _BottomColor, _FresnelColor;
        half _FresnelPower;
        float _HeightScale;
        half _Smoothness, _Metallic;

        struct Input {
            float3 worldPos;
            float3 viewDir;
        };

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            float h = saturate((IN.worldPos.y * _HeightScale + 1) * 0.5);
            fixed3 c = lerp(_BottomColor.rgb, _TopColor.rgb, h);
            float rim = pow(1 - saturate(dot(normalize(o.Normal), normalize(IN.viewDir))), _FresnelPower);
            fixed3 emis = _FresnelColor.rgb * rim;
            o.Albedo = c;
            o.Emission = emis;
            o.Metallic = _Metallic;
            o.Smoothness = _Smoothness;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
