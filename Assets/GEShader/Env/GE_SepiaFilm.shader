Shader "GE/SepiaFilm"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _SepiaStrength ("Sepia Strength", Range(0,1)) = 1
        _Vignette ("Vignette", Range(0,1)) = 0.35
        _Grain ("Film Grain", Range(0,1)) = 0.15
        _Scratches ("Scratches", Range(0,1)) = 0.1
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
            float _SepiaStrength, _Vignette, _Grain, _Scratches;

            struct appdata { float4 vertex:POSITION; float2 uv:TEXCOORD0; };
            struct v2f { float2 uv:TEXCOORD0; float4 vertex:SV_POSITION; };
            v2f vert(appdata v){ v2f o; o.vertex = UnityObjectToClipPos(v.vertex); o.uv=v.uv; return o; }

            float hash21(float2 p){ p = frac(p*float2(127.1,311.7)); p += dot(p,p+74.7); return frac(p.x*p.y); }

            fixed4 frag(v2f i) : SV_Target
            {
                float3 col = tex2D(_MainTex, i.uv).rgb;
                float3 sep = float3(dot(col, float3(0.393,0.769,0.189)),
                                     dot(col, float3(0.349,0.686,0.168)),
                                     dot(col, float3(0.272,0.534,0.131)));
                col = lerp(col, sep, _SepiaStrength);

                // Vignette
                float2 uv = i.uv * 2 - 1; float r2 = dot(uv,uv);
                float vig = smoothstep(1.0, 0.2, r2);
                col *= lerp(1.0, vig, _Vignette);

                // Grain
                float n = hash21(i.uv * _MainTex_TexelSize.zw + _Time.yy*13.5);
                col = saturate(col + (n-0.5) * _Grain);

                // Vertical scratches
                float sx = frac(i.uv.x * _MainTex_TexelSize.z * 0.02 + _Time.y*0.05);
                float scratch = step(0.98, sx) * (0.5 + 0.5*hash21(float2(i.uv.y, _Time.y)));
                col = lerp(col, col*0.6 + 0.4, scratch * _Scratches);

                return float4(col,1);
            }
            ENDCG
        }
    }
}
