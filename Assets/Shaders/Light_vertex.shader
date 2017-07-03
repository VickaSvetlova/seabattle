Shader "Custom/Light vertexLit Blended" {
    Properties {
        _Color ("Main Color", Color) = (1,1,1,0)
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }

		SubShader {
//	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
	Blend SrcAlpha OneMinusSrcAlpha
//	Cull Off ZWrite Off
			Pass {
				Material {
					Diffuse [_Color]
					Ambient [_Color]
				}
				Lighting On
				SeparateSpecular Off
				SetTexture [_MainTex] {
					Combine texture * primary DOUBLE, texture * primary
				}
			}
		}
}