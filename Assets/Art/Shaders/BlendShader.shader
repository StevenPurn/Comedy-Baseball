// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Shaders101/Exercise 7"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_SecondTex("Second Texture", 2D) = "white" {}
		_Tween("Tween", Range(0, 1)) = 0
		_Color("Color", Color) = (1, 1, 1, 1)
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
			sampler2D _SecondTex;
			float4 _Color;
			float _Tween;

			float4 frag(v2f i) : SV_Target
			{
				float4 color1 = tex2D(_MainTex, i.uv);
				float4 color2 = tex2D(_SecondTex, i.uv);
				float lum = color1.r * 0.3 + color1.g * 0.59 + color1.b * 0.11;
				float4 result;
				//check if the source texture color has a blue value of over 50%
				if(color1.a > 0){
					//If so, replace it with an adjusted color
					result = float4(color1.r * _Color.r, color1.g * _Color.g, color1.b * _Color.b, color1.a * _Color.a);
				}else{
					//If not, keep the color from the other texture
					result = float4(color2.r, color2.g, color2.b, color2.a);
				}
				float4 color = lerp(result, color1, _Tween);
				return color;
			}
			ENDCG
		}
	}
}