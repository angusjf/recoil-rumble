Shader "Custom/ReplaceCameraColors" {

	Properties {

		_MainTex("Texture", 2D) = "white" {}
		_Black("Black", Color) = (0.2,0.2,0.2,1)
		_White("White", Color) = (0.9,0.1,0.1,1)
		_Red("Red", Color) = (0,0,0)// = (0.9,0.2,0.1,1)
		_Blue("Blue", Color) = (0.1,0.3,0.9,1)

	} SubShader {

		Cull Off ZWrite Off ZTest Always // No culling or depth

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};
			
			struct v2f {
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v) {
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _MainTex;
			float4 _Black;
			float4 _White;
			float4 _Red;
			float4 _Blue;

			float4 frag(v2f i) : SV_Target {

				float4 col = tex2D(_MainTex, i.uv);

				if (col.x == 1 && col.y == 1 && col.z == 1) {
	    			col = _White;
				} else if (col.x == 1 && col.y == 0 && col.z == 0) {
					col = _Red;
				} else if (col.x == 0 && col.y == 0 && col.z == 1) {
					col = _Blue;
				} else if (col.x == 0 && col.y == 0 && col.z == 0) {
					col = _Black;
				}

				col.w = 1;

				return col;
			}

			ENDCG
		}
	}
}