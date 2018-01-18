Shader "Custom/ColorSwap"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
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
			float4 _Color;

			float4 frag(v2f i) : SV_Target
			{
				float4 color1 = tex2D(_MainTex, i.uv);
				float4 result;
				if(color1.a < 1 && color1.a > 0.5){
					result = float4(color1.r * _Color.r, color1.g * _Color.g, color1.b * _Color.b, 100);
				}else{
					result = float4(color1.r, color1.g, color1.b, color1.a);
				}
				return result;
			}
			ENDCG
		}
	}
}