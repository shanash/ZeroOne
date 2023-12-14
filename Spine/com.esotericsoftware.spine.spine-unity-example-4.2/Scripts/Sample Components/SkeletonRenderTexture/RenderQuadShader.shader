// Simple shader for e.g. a Quad that renders a RenderTexture.
// Texture color is multiplied by a color property, mostly for alpha fadeout.
Shader "Spine/RenderQuad" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		[NoScaleOffset] _MainTex("MainTex", 2D) = "white" {}
		_Cutoff("Shadow alpha cutoff", Range(0,1)) = 0.1
		[HideInInspector] _StencilRef("Stencil Reference", Float) = 1.0
		[HideInInspector][Enum(UnityEngine.Rendering.CompareFunction)] _StencilComp("Stencil Comparison", Float) = 8 // Set to Always as default
		//	피격시 등 tint 효과가 아닌, 전체 모습이 단일 색상으로 보여지도록 하기 위한 변수
		[HideInInspector] _AllChangeColor("All Change Color", Color) = (0,0,0,1)	//	추가 [2023-12-07] by forestj
		[HideInInspector] _IsChange("Is Change", Float) = 0	//	추가 [2023-12-07] by forestj
	}
	SubShader{
		Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "PreviewType" = "Plane" }
		Blend One OneMinusSrcAlpha
		Cull Off
		ZWrite Off
		Lighting Off

		Stencil {
			Ref[_StencilRef]
			Comp[_StencilComp]
			Pass Keep
		}

		Pass {
			Name "Normal"

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			sampler2D _MainTex;
			float4 _Color;

			float _IsChange;			//	추가 [2023-12-07] by forestj
			float4 _AllChangeColor;		//	추가 [2023-12-07] by forestj

			struct VertexInput {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 vertexColor : COLOR;
			};

			struct VertexOutput {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 vertexColor : COLOR;
			};

			VertexOutput vert(VertexInput v) {
				VertexOutput o = (VertexOutput)0;
				o.uv = v.uv;
				o.vertexColor = v.vertexColor;
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
			}

			float4 frag(VertexOutput i) : SV_Target {
				float4 texColor = tex2D(_MainTex,i.uv);
				//_Color.rgb *= _Color.a;			//	수정 [2023-12-07] by forestj
				//return texColor * _Color;			//	수정 [2023-12-07] by forestj


				//	추가 [2023-12-07] by forestj Begin
				if(_IsChange == 0){
					_Color.rgb *= _Color.a;
					return texColor * _Color;
				}
				else{
					if(texColor.a > 0){
						return _AllChangeColor;
					}
					else{
						_AllChangeColor.rgb *= _Color.a;
						return texColor * _AllChangeColor;
					}
					
				}
				//	추가 [2023-12-07] by forestj Finish
			}
			ENDCG
		}

		Pass {
			Name "Caster"
			Tags { "LightMode" = "ShadowCaster" }
			Offset 1, 1
			ZWrite On
			ZTest LEqual

			Fog { Mode Off }
			Cull Off
			Lighting Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_shadowcaster
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"
			sampler2D _MainTex;
			fixed _Cutoff;

			struct VertexOutput {
				V2F_SHADOW_CASTER;
				float4 uvAndAlpha : TEXCOORD1;
			};

			VertexOutput vert(appdata_base v, float4 vertexColor : COLOR) {
				VertexOutput o;
				o.uvAndAlpha = v.texcoord;
				o.uvAndAlpha.a = vertexColor.a;
				TRANSFER_SHADOW_CASTER(o)
				return o;
			}

			float4 frag(VertexOutput i) : SV_Target {
				fixed4 texcol = tex2D(_MainTex, i.uvAndAlpha.xy);
				clip(texcol.a* i.uvAndAlpha.a - _Cutoff);
				SHADOW_CASTER_FRAGMENT(i)
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
