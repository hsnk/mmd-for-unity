Shader "Debug/UV 1" {
	Properties {
		_MainTex ("Main", 2D) = "white" {}
		_Base ("Base", 2D) = "black" {}
		_Vector1 ("Vector 1", 2D) = "black" {}
		_Length1 ("Length 1", 2D) = "black" {}
		_Weight1 ("Weight", Range(0,1)) = 0
	}
	SubShader {
		Pass {
			Fog { Mode Off }
			CGPROGRAM
			#pragma glsl
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"

			sampler2D _MainTex;

			// 共通化できる部分
			sampler2D _Base;
			sampler2D _Vector1;
			sampler2D _Length1;
			// ここまで

			// vertex input: position, UV
			struct appdata {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				float2 index : TEXCOORD1;
			};
			
			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			float GetReferenceFlag(sampler2D tex, float2 uv)
			{
				float4 color = tex2Dlod(tex, float4(uv.xy, 0, 0));
				return color.a;		
			}

			float4 GetReferenceData(sampler2D tex, float2 uv) {
				return tex2Dlod(tex, float4(uv.xy, 0, 0));
			}
			
			float2 GetReferenceUV(int id, int size)
			{
				int x = id % size;
				int y = id / size;
				return float2((float)x / (float)size, (float)y / (float)size);
			}

			float GetFloat(float4 color) {
				// 本当にこれでうまくいくのかちょっとわからない
				const float4 conversion = float4(1.0, 1.0 / 256.0, 1.0 / (256.0 * 256.0), 1.0 / (256.0 * 256.0 * 256.0));
				return dot(color, conversion);
			}
			
			float GetHalf(float2 color)
			{
				const float2 conversion = float2(1.0, 1.0 / 256.0);
				return dot(color, conversion);
			}
			
			float4 UnpackVector(float4 colors)
			{
				float x = GetHalf(float2(colors.xy));
				float y = GetHalf(float2(colors.zw));
				float z = 1 - (sqrt(x * x + y * y));
				// 1 = sqrt(x^2 + y^2 + z^2)
				// 1 - sqrt(z^2) = sqrt(x^2 + y^2)
				// sqrt(z^2) = 1 - sqrt(x^2 + y^2)
				return float4(x*x, y*y, z*z, 1); // 圧縮したときsqrtしている, 16bitで精度が犠牲になっているので代替策
			}
			
			float UnpackLength(float4 colors) {
				return GetFloat(colors);
			}
			
			int GetIndex(float2 uv) {
				return (int)uv.x + (int)uv.y * 1000000;
			}

			appdata Skinshade(appdata v) {
				int id = GetIndex(v.index);
				float2 reference_uv = GetReferenceUV(id, 512);
				float reference_flag = GetReferenceFlag(_Base, reference_uv);

				//if (reference_flag == 1) {
					// 共通化できる部分
					float4 vector1_ns = GetReferenceData(_Vector1, reference_uv);
					float4 length1_ns = GetReferenceData(_Length1, reference_uv);
					float4 vector1 = UnpackVector(vector1_ns);
					float length1 = UnpackLength(length1_ns);
					v.vertex += vector1 * length1;
					// ここまで
				//}
				return v;
			}
	 
			v2f vert (appdata v) {
				v2f o;
				v = Skinshade(v);
				o.pos = mul( UNITY_MATRIX_MVP, v.vertex );
				o.uv = v.texcoord;
				return o;
			}
			 
			half4 frag( v2f i ) : COLOR {
				half4 color = tex2D(_MainTex, i.uv);
				return color;
			}
			ENDCG
		}
	}
}