Shader "SelfQuest/Player/MarbleNoise"
{
    Properties
    {
        _Color1 ("Light Vein", Color) = (1,1,1,1)
        _Color2 ("Dark Base", Color) = (0.05,0.05,0.08,1)
        _Scale ("Scale", Float) = 3
        _VeinIntensity ("Vein Intensity", Range(0,2)) = 1
        _Warp ("Warp", Range(0,5)) = 1.5
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
        #include "UnityCG.cginc"
        fixed4 _Color1, _Color2; float _Scale, _Warp; half _VeinIntensity, _Smoothness, _Metallic;

        float hash31(float3 p){ return frac(sin(dot(p, float3(12.9898,78.233,37.719))) * 43758.5453); }
        float noise3(float3 p){
            float3 i = floor(p);
            float3 f = frac(p);
            float n = 0.0;
            for(int x=0;x<2;x++) for(int y=0;y<2;y++) for(int z=0;z<2;z++){
                float3 g = float3(x,y,z);
                float w = dot(f - g, f - g);
                n += (1.0 - w) * hash31(i + g);
            }
            return saturate(n / 2.0);
        }

        struct Input{ float3 worldPos; };
        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            float3 obj = mul(unity_WorldToObject, float4(IN.worldPos,1)).xyz * _Scale;
            float n = noise3(obj + noise3(obj*2.0) * _Warp);
            float veins = saturate(0.5 + sin(obj.x + obj.y*0.5 + obj.z*0.25 + n*3.14159) * 0.5);
            float t = pow(veins, 1.5) * _VeinIntensity;
            fixed3 col = lerp(_Color2.rgb, _Color1.rgb, t);
            o.Albedo = col;
            o.Smoothness = _Smoothness;
            o.Metallic = _Metallic;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
