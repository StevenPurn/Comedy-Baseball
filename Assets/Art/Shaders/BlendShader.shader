// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Shaders101/Exercise 7"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_Tween("Tween", Range(0, 1)) = 0
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"PreviewType" = "Plane"
		}

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _MainTex;
			float4 _Color;
			float _Tween;

			float4 frag(v2f i) : SV_Target
			{
				float4 color1 = tex2D(_MainTex, i.uv);
				float lum = color1.r * 0.3 + color1.g * 0.59 + color1.b * 0.11;
				float4 grayscale;
				//check if the source texture color has a blue value of over 50%
				if(color1.b > 0.5){
					//If so, replace it with an adjusted color
					grayscale = float4(color1.r * 1, color1.g * 0.6, 0, color1.a);
				}else{
					grayscale = float4(color1.r, color1.g, color1.b, color1.a);
				}
				float4 color = lerp(grayscale, color1, _Tween);
				return color;
			}
			ENDCG
		}
	}
}