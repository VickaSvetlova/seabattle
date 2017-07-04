Shader "Custom/Light vertexLit Blended" 
{
    Properties 
	{
        _Color ("Main Color", Color) = (1,1,1,0)
        _MainTex ("Base (RGB)", 2D) = "white" {}
        [Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull", Float) = 2 //"Back"
    }

	SubShader 
	{
		Cull [_Cull]
		Blend SrcAlpha OneMinusSrcAlpha
			Pass 
			{
				Material 
				{
					Diffuse [_Color]
					Ambient [_Color]
				}
				Lighting On
				SeparateSpecular Off
				SetTexture [_MainTex] 
				{
					Combine texture * primary DOUBLE, texture * primary
				}
			}
		}
	}
