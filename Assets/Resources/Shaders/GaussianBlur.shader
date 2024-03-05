Shader "FluffyDuck/GaussianBlur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BlurSizeX ("Blur Size X", Float) = 1.0
        _BlurSizeY ("Blur Size Y", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _BlurSizeX;
            float _BlurSizeY;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = fixed4(0,0,0,0);
                float2 blurSize = float2(_BlurSizeX / _ScreenParams.x, _BlurSizeY / _ScreenParams.y);

                // Gaussian weights - Fixed size array
                float weights[5] = {0.2270270270, 0.1945945946, 0.1216216216, 0.0540540541, 0.0162162162};

                // Apply Gaussian blur
                for(int x = -2; x <= 2; x++)
                {
                    for(int y = -2; y <= 2; y++)
                    {
                        float2 sampleOffset = float2(x, y) * blurSize;
                        col += tex2D(_MainTex, i.uv + sampleOffset) * weights[abs(x)] * weights[abs(y)];
                    }
                }

                col = clamp(col, 0.0, 1.0); // 색상 값을 0과 1 사이로 제한

                return col;
            }
            ENDCG
        }
    }
}
