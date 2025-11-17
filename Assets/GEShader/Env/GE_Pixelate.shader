Shader "GE/PixelateImproved"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _PixelSize ("Pixel Size (screen px)", Float) = 4
        _PaletteSteps ("Palette Steps", Float) = 32
        _DitherIntensity ("Dither Intensity", Range(0,1)) = 0
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

            sampler2D _MainTex;
            float4 _MainTex_TexelSize; // x=1/w, y=1/h, z=w, w=h

            float _PixelSize;
            float _PaletteSteps;
            float _DitherIntensity;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // Bayer 2x2 dither matrix
            float dither2x2(float2 uv)
            {
                float2 p = floor(fmod(uv * _MainTex_TexelSize.zw, 2));
                // matrix: [0,2;3,1] normalized to [0,1]
                float m = (p.x < 0.5 ? (p.y < 0.5 ? 0.0 : 3.0) : (p.y < 0.5 ? 2.0 : 1.0));
                return m / 3.0;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Quantize UVs to a pixel grid measured in screen pixels
                float2 stepUV = _PixelSize * _MainTex_TexelSize.xy; // size of a block in UV units
                stepUV = max(stepUV, float2(_MainTex_TexelSize.x, _MainTex_TexelSize.y));

                float2 uvq = floor(i.uv / stepUV) * stepUV;
                fixed4 col = tex2D(_MainTex, uvq);

                // Optional simple ordered dithering before posterization
                if (_DitherIntensity > 0.0)
                {
                    float n = dither2x2(i.uv * _MainTex_TexelSize.zw);
                    col.rgb += (_DitherIntensity * 1.0/255.0) * (n - 0.5);
                }

                // Color posterization to emulate limited palettes
                if (_PaletteSteps > 1.0)
                {
                    float steps = max(_PaletteSteps, 2.0);
                    col.rgb = floor(col.rgb * steps) / steps;
                }

                return col;
            }
            ENDCG
        }
    }
}
