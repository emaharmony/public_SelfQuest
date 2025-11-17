Shader "SelfQuest/Player/LavaFlow"
{
    Properties
    {
        _Cold ("Cold Color", Color) = (0.1,0.02,0.02,1)
        _Hot ("Hot Color", Color) = (1.0,0.5,0.1,1)
        _Crust ("Crust Color", Color) = (0.02,0.02,0.02,1)
        _FlowSpeed ("Flow Speed", Range(0,5)) = 0.6
        _Scale ("Noise Scale", Float) = 2.5
        _Threshold ("Glow Threshold", Range(0,1)) = 0.55
        _Smoothness ("Smoothness", Range(0,1)) = 0.05
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0
        #include "UnityCG.cginc"
        fixed4 _Cold, _Hot, _Crust; float _FlowSpeed, _Scale, _Threshold; half _Smoothness;

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
        float fbm(float3 p){
            float a=0.5, s=0.0; float3 pp=p;
            for(int i=0;i<4;i++){ s += a*noise3(pp); pp*=2.02; a*=0.5; }
            return s;
        }

        struct Input{ float3 worldPos; };
        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            float3 obj = mul(unity_WorldToObject, float4(IN.worldPos,1)).xyz;
            float t = _Time.y * _FlowSpeed;
            float3 q = float3(obj.x + t, obj.y, obj.z - t) * _Scale;
            float n1 = fbm(q);
            float n2 = fbm(q*1.7 + 2.3);
            float m = saturate(n1*0.6 + n2*0.4);
            float glow = smoothstep(_Threshold, 1.0, m);
            fixed3 hot = lerp(_Cold.rgb, _Hot.rgb, glow);
            fixed3 col = lerp(_Crust.rgb, hot, m);
            o.Albedo = col;
            o.Emission = hot * pow(glow, 2.0);
            o.Metallic = 0;
            o.Smoothness = _Smoothness;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
