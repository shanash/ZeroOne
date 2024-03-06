Shader "FluffyDuck/TransparentHole"
{
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Rect ("Transparent Rect (x, y, width, height)", Vector) = (0,0,1,1)
        _Color ("Color", Color) = (1,1,1,1) // 단색을 위한 색상 속성
    }
    SubShader {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" "RenderQueue"="Transparent"}
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha // 알파 블렌딩을 위한 설정
        ZWrite Off // 투명한 효과를 위해 깊이 쓰기 비활성화

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Rect;
            fixed4 _Color; // Image 컴포넌트의 color 값

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                // Convert the UVs to a 0-1 range based on the _Rect
                float2 uv = (i.uv - _Rect.xy) / _Rect.zw;
                // Check if the UVs fall outside the transparent rectangle
                if (uv.x > 0 && uv.x < 1 && uv.y > 0 && uv.y < 1) {
                    discard; // Make this pixel transparent
                }
                // 텍스처 색상 대신 _Color 사용
                return _Color;
            }
            ENDCG
        }
    } 
    FallBack "Diffuse"
}