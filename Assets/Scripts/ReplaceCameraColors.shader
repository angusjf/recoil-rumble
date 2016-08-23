Shader "Custom/ReplaceCameraColors"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
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
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _MainTex;

			float4 frag(v2f i) : SV_Target
			{
				float4 col = tex2D(_MainTex, i.uv);
				//col += float4(0, 0.2, 0.2, 1); //more blue
				//col *= float4(1.1, 1.3, 1.3, 1); // more contrast

				if (col.x == 1 && col.y == 1 && col.z == 1) {
					//white
					col.x = 0.9;
					col.y = 0.9;
					col.z = 0.9;
					//col.x = 0.3;
					//col.y = 0.2;
					//col.z = 0.2;
				} else if (col.x == 1 && col.y == 0 && col.z == 0) {
					//red
					col.x = 1;
					col.y = 0.4;
					col.z = 0.4;
				} else if (col.x == 0 && col.y == 0 && col.z == 1) {
					//blue
					col.x = 0;
					col.y = 0.7;
					col.z = 0.9;
				} else if (col.x == 0 && col.y == 0 && col.z == 0) {
					//black
					col.x = 0.2;
					col.y = 0.3;
					col.z = 0.4;
					//col.x = 0.9;
					//col.y = 0.9;
					//col.z = 0.9;
				}
				return col;
			}

			ENDCG
		}
	}
}