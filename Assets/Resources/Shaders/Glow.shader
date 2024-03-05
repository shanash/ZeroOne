Shader "FluffyDuck/Glow"
{
    // 셰이더에서 사용할 프로퍼티를 정의합니다.
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {} // 기본 텍스처
        _GlowColor ("Glow Color", Color) = (1,1,1,1) // 빛나는 색상
        _GlowIntensity ("Glow Intensity", Float) = 1.5 // 빛나는 강도
        _Threshold ("Threshold", Float) = 0.5 // 빛나는 효과를 적용할 최소 밝기
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" } // 불투명한 렌더링 유형 설정
        LOD 100 // 세부 수준 결정(Level Of Detail)

        Pass
        {
            CGPROGRAM
            // Vertex shader와 Fragment shader를 정의합니다.
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc" // Unity의 기본 CG include 파일

            // 정의된 프로퍼티에 대한 변수 선언
            sampler2D _MainTex; // 텍스처 샘플러
            float4 _GlowColor; // 빛나는 색상
            float _GlowIntensity; // 빛나는 강도
            float _Threshold; // 빛나는 효과를 적용할 최소 밝기

            // Vertex shader 입력 구조체
            struct appdata
            {
                float4 vertex : POSITION; // 정점 위치
                float2 uv : TEXCOORD0; // 텍스처 좌표
            };

            // Vertex shader 출력 / Fragment shader 입력 구조체
            struct v2f
            {
                float2 uv : TEXCOORD0; // 텍스처 좌표
                float4 vertex : SV_POSITION; // 스크린 위치
            };

            // Vertex shader 함수
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex); // 정점 위치를 클립 공간으로 변환
                o.uv = v.uv; // 텍스처 좌표 전달
                return o;
            }

            // Fragment shader 함수
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv); // 텍스처에서 색상 샘플링
                float luminance = dot(col.rgb, float3(0.2126, 0.7152, 0.0722)); // 픽셀의 밝기 계산
                if (luminance > _Threshold) // 밝기가 설정된 임계값을 초과하는 경우
                {
                    col += _GlowColor * _GlowIntensity; // 빛나는 색상과 강도를 적용
                }
                return col; // 결과 색상 반환
            }
            ENDCG
        }
    }
}
