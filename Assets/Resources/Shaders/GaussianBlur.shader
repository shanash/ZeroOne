Shader "FluffyDuck/GaussianBlur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _BlurSize ("Blur Size", Float) = 20.0 // 블러 범위
        _Quality ("Quality", Float) = 1.0 // 0~1까지의 값
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

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

            // 샘플러 선언 및 샘플러 상태 설정
            Texture2D _MainTex;
            SamplerState sampler_MainTex : register(s0)
            {
                Filter = MIN_MAG_MIP_LINEAR; // 선형 필터링
                AddressU = CLAMP; // U 방향으로 CLAMP 모드 적용
                AddressV = CLAMP; // V 방향으로 CLAMP 모드 적용
            };

            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;
            float4 _Color; // Changed from fixed4 to float4
            float _BlurSize;
            float _Quality;
            float _Strength;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float4 col = float4(0,0,0,0);

                float weight_total = 0;
                float quality = clamp(_Quality, 0.01, 1.0); // 화질 설정에 따른 샘플링 간격 조정
                float diameter = _BlurSize * 2; // 직경 계산
                float gap = diameter / (diameter * quality); // 샘플링 간격

                // 블러 처리를 위한 샘플링 시작
                for (float x = -_BlurSize; x <= _BlurSize; x += gap)
                {
                    for (float y = -_BlurSize; y <= _BlurSize; y += gap)
                    {
                        // 현재 샘플링 위치 계산
                        float2 pos = float2(x, y);

                        float distance_normalized = 0;

                        if (_BlurSize > 0 )
                        {
                            // 중심으로부터의 정규화된 거리 계산
                            distance_normalized = length(pos) / _BlurSize;
                            if (distance_normalized > 1) continue; // 정규화된 거리가 1을 초과하면 샘플링하지 않음
                        }

                        // 텍스처 좌표에 적용할 오프셋 계산
                        float2 offsets = pos * _MainTex_TexelSize.xy;
                        // 샘플링할 UV 좌표
                        float2 sampleUV = clamp(uv + offsets, 0.0, 1.0); // [0, 1] 범위 내로 제한

                        // 가우시안 가중치 계산
                        float weight = exp(-0.5 * pow(distance_normalized, 2) * 5); // 가우시안 함수를 이용한 가중치
                        weight_total += weight; // 가중치 총합 업데이트
                        col += _MainTex.Sample(sampler_MainTex, sampleUV) * weight; // 샘플링된 색상에 가중치 적용
                    }
                }

                col /= weight_total; // 가중치 총합으로 정규화하여 최종 색상 계산
                return col * _Color; // 최종 색상에 사용자 정의 색상 적용
            }
            ENDHLSL
        }
    }
}
