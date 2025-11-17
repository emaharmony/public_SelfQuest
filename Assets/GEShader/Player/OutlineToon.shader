Shader "SelfQuest/Player/OutlineToon"
{
    Properties
    {
        _Color ("Base Color", Color) = (0.9,0.9,0.9,1)
        _ShadeColor ("Shade Color", Color) = (0.25,0.25,0.3,1)
        _Bands ("Bands", Range(1,8)) = 3
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth ("Outline Width", Range(0.0005,0.02)) = 0.004
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 300

        // Pass 1: Outline (draw backfaces extruded along normals)
        Pass
        {
            Name "OUTLINE"
            Cull Front
            ZWrite On
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            fixed4 _OutlineColor; float _OutlineWidth;
            struct appdata { float4 vertex:POSITION; float3 normal:NORMAL; };
            struct v2f { float4 pos:SV_POSITION; };
            v2f vert(appdata v)
            {
                v2f o;
                // Extrude in view space to get consistent width
                float3 n = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.normal));
                float4 pos = mul(UNITY_MATRIX_MV, v.vertex);
                pos.xyz += n * _OutlineWidth * pos.w; // scale with w for perspective correctness
                o.pos = mul(UNITY_MATRIX_P, pos);
                return o;
            }
            fixed4 frag(v2f i) : SV_Target { return _OutlineColor; }
            ENDCG
        }

        // Pass 2: Interior toon
        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0
        fixed4 _Color, _ShadeColor;
        half _Bands;
        struct Input { float3 viewDir; float3 worldNormal; };
        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            float3 n = normalize(IN.worldNormal);
            float3 v = normalize(IN.viewDir);
            float ndv = saturate(dot(n, v));
            float bands = floor(ndv * _Bands) / max(_Bands-1, 1);
            o.Albedo = lerp(_ShadeColor.rgb, _Color.rgb, bands);
            o.Metallic = 0; o.Smoothness = 0.05;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
