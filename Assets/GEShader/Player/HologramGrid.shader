Shader "SelfQuest/Player/HologramGrid"
{
    Properties
    {
        _Tint ("Tint", Color) = (0.2,0.9,1,1)
        _Alpha ("Alpha", Range(0,1)) = 0.6
        _GridScale ("Grid Scale", Float) = 16
        _Scroll ("Scroll Speed", Float) = 0.5
        _FresnelPow ("Fresnel Power", Range(0.5,8)) = 2
        _LineWidth ("Line Width", Range(0.001,0.2)) = 0.03
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha One
        ZWrite Off
        Cull Back

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            fixed4 _Tint; float _Alpha; float _GridScale; float _Scroll; float _FresnelPow; float _LineWidth;

            struct appdata { float4 vertex:POSITION; float3 normal:NORMAL; };
            struct v2f { float4 pos:SV_POSITION; float3 wpos:TEXCOORD0; float3 wnorm:TEXCOORD1; float3 viewDir:TEXCOORD2; };
            v2f vert(appdata v){ v2f o; o.pos=UnityObjectToClipPos(v.vertex); float3 wpos=mul(unity_ObjectToWorld,v.vertex).xyz; o.wpos=wpos; float3 n=UnityObjectToWorldNormal(v.normal); o.wnorm=n; o.viewDir=_WorldSpaceCameraPos - wpos; return o; }
            float gridLine(float2 p, float width)
            {
                float2 g = abs(frac(p) - 0.5);
                float d = min(g.x, g.y);
                return smoothstep(width, 0.0, d);
            }
            fixed4 frag(v2f i):SV_Target
            {
                float3 obj = mul(unity_WorldToObject, float4(i.wpos,1)).xyz;
                float t = _Time.y * _Scroll;
                float2 gxy = (obj.xy + t) * _GridScale;
                float2 gyz = (obj.yz + t) * _GridScale;
                float2 gxz = (obj.xz + t) * _GridScale;
                float line = (gridLine(gxy, _LineWidth) + gridLine(gyz, _LineWidth) + gridLine(gxz, _LineWidth)) / 3.0;
                float3 n = normalize(i.wnorm);
                float3 v = normalize(i.viewDir);
                float fres = pow(1 - saturate(dot(n,v)), _FresnelPow);
                float a = saturate(_Alpha * (0.5*line + fres));
                return fixed4(_Tint.rgb * (line + fres), a);
            }
            ENDCG
        }
    }
}
