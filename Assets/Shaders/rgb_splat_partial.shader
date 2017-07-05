// Alpha of color - intensity of mask
Shader "Custom/RGB_splat_partial" {

    Properties {
        _MainTex ("Main texture", 2D) = "white" {}
		_SkinTex ("Mask texture", 2D) = "white" {}
		_MainColor ("Main color (red)", Color) = (1,1,1,1)
		_AddColor ("Additional 1st color (green)", Color) = (1,1,1,1)
		_ExtColor ("Additional 2nd color (blue)", Color) = (1,1,1,1)
		[Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull", Float) = 2 //"Back"
    }

    SubShader {
		Cull [_Cull]
		Blend SrcAlpha OneMinusSrcAlpha
        Pass {
			CGPROGRAM
			#pragma fragment MyFragmentProgram
			#pragma vertex MyVertexProgram

			#include "UnityCG.cginc"
				
			sampler2D _MainTex;
			sampler2D _SkinTex;
			float4 _MainTex_ST;
			float4 _MainColor;
			float4 _AddColor;
			float4 _ExtColor;

			struct Interpolators {
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
				float2 uvSplat : TEXCOORD1;
			};

			struct VertexData {
				float4 position : POSITION;
				float2 uv : TEXCOORD0;
			};

			Interpolators MyVertexProgram (VertexData v) {
				Interpolators i;
				i.position = UnityObjectToClipPos(v.position);
				i.uv = TRANSFORM_TEX(v.uv, _MainTex);
				i.uvSplat = v.uv;
				return i;
			}
			float4 MyFragmentProgram (Interpolators i) : SV_TARGET {
				float4 splat = tex2D(_SkinTex, i.uvSplat);
				float4 tex = tex2D(_MainTex, i.uv);
				float4 color = float4(tex.rgb * (1 - splat.r - splat.g - splat.b) +
					lerp (tex.rgb, _MainColor.rgb, _MainColor.a) * splat.r +
					lerp (tex.rgb, _AddColor.rgb, _AddColor.a) * splat.g +
					lerp (tex.rgb, _ExtColor.rgb, _ExtColor.a) * splat.b, splat.a);
				return color;
			}

			ENDCG
		}
    }
}