Shader "SelfQuest/Player/PulsingEmissive"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (0.1,0.1,0.1,1)
        _PulseColor ("Pulse Color", Color) = (1,0.5,0.2,1)
        _PulseSpeed ("Pulse Speed", Range(0,10)) = 2
        _PulseIntensity ("Pulse Intensity", Range(0,5)) = 1.5
        _Smoothness ("Smoothness", Range(0,1)) = 0.05
        _Metallic ("Metallic", Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 150
        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0
        fixed4 _BaseColor, _PulseColor; half _PulseSpeed, _PulseIntensity; half _Smoothness, _Metallic;
        struct Input { float3 worldPos; };
        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            float s = sin(_Time.y * _PulseSpeed) * 0.5 + 0.5;
            float glow = s * _PulseIntensity;
            o.Albedo = _BaseColor.rgb;
            o.Emission = _PulseColor.rgb * glow;
            o.Smoothness = _Smoothness;
            o.Metallic = _Metallic;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
