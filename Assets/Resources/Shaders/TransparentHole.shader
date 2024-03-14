Shader "FluffyDuck/TransparentHole"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0

        _Rect ("Transparent Rect (x, y, width, height)", Vector) = (0,0,1,1)
        _RangeRect ("Range Rect (x, y, width, height)", Vector) = (0,0,1,1)
        _CornerRadius ("Corner Radius", Float) = 0.1 // 라운딩된 모서리의 반지름
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            Name "Default"
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            float4 _MainTex_ST;
            float4 _Rect;
            float4 _RangeRect;
            float _CornerRadius;

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

                OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

                OUT.color = v.color * _Color;
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                // Convert the UVs to a 0-1 range based on the _Rect
                float2 uv = (IN.texcoord - _Rect.xy) / _Rect.zw;
                float2 range_uv = (IN.texcoord - _RangeRect.xy) / _RangeRect.zw;

                // Check if the UVs fall outside the transparent rectangle
                if (range_uv.x > 0 && range_uv.x < 1 && uv.y > 0 && uv.y < 1) {
                    discard; // Make this pixel transparent
                }
                if (uv.x > 0 && uv.x < 1 && range_uv.y > 0 && range_uv.y < 1) {
                    discard; // Make this pixel transparent
                }

                float2 radius = ((_RangeRect.zw - _Rect.zw) / _Rect.zw) / 2.0f;

                if (range_uv.x > 0 && range_uv.x < 1 && range_uv.y > 0 && range_uv.y < 1)
                {
                    float2 r_xy = uv.xy;
                    if (uv.x >= 1.0)
                    {
                        r_xy.x -= 1.0;
                    }
                    if (uv.y >= 1.0)
                    {
                        r_xy.y -= 1.0f;
                    }

                    if (r_xy.x * r_xy.x + r_xy.y * r_xy.y < radius.x * radius.x)
                    {
                        discard;
                    }
                }

                half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;

                #ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif

                return color;
            }
        ENDCG
        }
    }
}
