Shader "SelfQuest/Player/RetroPixelDither"
{
    Properties
    {
        _ColorA ("Dark Color", Color) = (0.05,0.1,0.2,1)
        _ColorB ("Mid Color", Color) = (0.2,0.6,0.9,1)
        _ColorC ("Light Color", Color) = (0.85,0.95,1,1)
        _Threshold1 ("Threshold 1", Range(0,1)) = 0.4
        _Threshold2 ("Threshold 2", Range(0,1)) = 0.75
        _PixelSize ("Pixel Size", Range(1,16)) = 3
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 150
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            fixed4 _ColorA, _ColorB, _ColorC; float _Threshold1, _Threshold2; float _PixelSize;

            static const float4x4 BAYER4 = {
                0/16.0,  8/16.0,  2/16.0, 10/16.0,
                12/16.0, 4/16.0, 14/16.0, 6/16.0,
                3/16.0, 11/16.0, 1/16.0, 9/16.0,
                15/16.0, 7/16.0, 13/16.0, 5/16.0
            };

            struct appdata { float4 vertex:POSITION; float3 normal:NORMAL; };
            struct v2f { float4 pos:SV_POSITION; float3 n:TEXCOORD0; float4 sp:TEXCOORD1; };
            v2f vert(appdata v){ v2f o; o.pos=UnityObjectToClipPos(v.vertex); o.n = UnityObjectToWorldNormal(v.normal); o.sp = ComputeScreenPos(o.pos); return o; }

            fixed4 frag(v2f i):SV_Target
            {
                // simple shade factor from view-facing
                float3 n = normalize(i.n);
                float shade = saturate(n.z * 0.5 + 0.5);

                // pixelate coordinates
                float2 sp = i.sp.xy / i.sp.w;
                float2 res = _ScreenParams.xy;
                float2 pix = floor(sp * res / _PixelSize) * _PixelSize;
                float2 uvp = pix / res;

                // 4x4 Bayer index
                float2 ip = floor(frac(uvp * res) / _PixelSize);
                int ix = (int)ip.x & 3; int iy = (int)ip.y & 3;
                float threshold = BAYER4[iy][ix];

                // quantize into 3 colors using dithering
                float t = shade;
                fixed3 col;
                if (t < _Threshold1 - threshold*0.25) col = _ColorA.rgb;
                else if (t < _Threshold2 - threshold*0.25) col = _ColorB.rgb;
                else col = _ColorC.rgb;

                return fixed4(col, 1);
            }
            ENDCG
        }
    }
}
