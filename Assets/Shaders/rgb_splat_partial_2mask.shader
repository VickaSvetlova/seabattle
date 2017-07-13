// Alpha of color - intensity of mask
Shader "Custom/RGB_splat_partial_2mask" {

    Properties {
        _MainTex ("Main texture", 2D) = "white" {}
		_SkinTex ("Mask texture", 2D) = "white" {}
		_MainColor ("Main color (red)", Color) = (1,1,1,1)
		_AddColor ("Additional 1st color (green)", Color) = (1,1,1,1)
		_ExtColor ("Additional 2nd color (blue)", Color) = (1,1,1,1)
		_SkinTex2 ("2nd mask texture", 2D) = "white" {}
		_MainColor2 ("2nd main color (red)", Color) = (1,1,1,1)
		_AddColor2 ("2nd additional 1st color (green)", Color) = (1,1,1,1)
		_ExtColor2 ("2nd additional 2nd color (blue)", Color) = (1,1,1,1)
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
			sampler2D _SkinTex2;
			float4 _MainTex_ST;
			float4 _MainColor;
			float4 _AddColor;
			float4 _ExtColor;
			float4 _MainColor2;
			float4 _AddColor2;
			float4 _ExtColor2;

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
				float4 splat2 = tex2D(_SkinTex2, i.uvSplat);
				float4 tex = tex2D(_MainTex, i.uv);
				float4 color = float4(tex.rgb * (1 - splat.r - splat.g - splat.b) +
					lerp (tex.rgb, _MainColor.rgb, _MainColor.a) * splat.r +
					lerp (tex.rgb, _AddColor.rgb, _AddColor.a) * splat.g +
					lerp (tex.rgb, _ExtColor.rgb, _ExtColor.a) * splat.b, splat.a);

				color = float4(color.rgb * (1 - splat2.r - splat2.g - splat2.b) +
					lerp (color.rgb, _MainColor2.rgb, _MainColor2.a) * splat2.r +
					lerp (color.rgb, _AddColor2.rgb, _AddColor2.a) * splat2.g +
					lerp (color.rgb, _ExtColor2.rgb, _ExtColor2.a) * splat2.b, splat2.a);
				return color;
			}

			ENDCG
		}
    }
}