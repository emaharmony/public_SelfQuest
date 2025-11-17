Shader "SelfQuest/Player/ToonColorBands"
{
    Properties
    {
        _Color ("Base Color", Color) = (0.8,0.8,0.8,1)
        _ShadeColor ("Shade Color", Color) = (0.2,0.2,0.25,1)
        _Bands ("Bands", Range(1,8)) = 3
        _RimColor ("Rim Color", Color) = (0.4,0.8,1,1)
        _RimPower ("Rim Power", Range(0.5,8)) = 2.5
        _Glossiness ("Smoothness", Range(0,1)) = 0.2
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 250

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0
        fixed4 _Color, _ShadeColor, _RimColor;
        half _Bands, _RimPower;
        half _Glossiness, _Metallic;

        struct Input
        {
            float3 viewDir;
            float3 worldNormal;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Base toon banding using NdotL approximated via baked lighting contribution
            // We fake bands with view-based lighting so it works without custom light function
            float3 n = normalize(IN.worldNormal);
            float3 v = normalize(IN.viewDir);
            float ndv = saturate(dot(n, v));
            float bands = floor(ndv * _Bands) / max(_Bands-1, 1);
            fixed3 baseCol = lerp(_ShadeColor.rgb, _Color.rgb, bands);

            // Rim lighting
            float rim = pow(1 - saturate(dot(n, v)), _RimPower);
            baseCol += _RimColor.rgb * rim;

            o.Albedo = saturate(baseCol);
            o.Smoothness = _Glossiness;
            o.Metallic = _Metallic;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
