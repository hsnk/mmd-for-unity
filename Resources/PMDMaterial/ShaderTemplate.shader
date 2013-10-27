Shader "Debug/UV 1" {
  Properties {
    _Skin1 ("Skin 1", 2D) = "black" {}
  }
  SubShader {
    Pass {
      Fog { Mode Off }
      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag
	  #include "UnityCG.cginc"
	  //#include "PixelToFloatConverter.cginc"
      
      // vertex input: position, UV
      struct appdata {
        float4 vertex : POSITION;
        float4 texcoord : TEXCOORD0;
		float4 index : TEXCOORD1;
      };
      
      struct v2f {
        float4 pos : SV_POSITION;
        float4 uv : TEXCOORD0;
      };

	  float GetFloat(float4 color) {
        const float4 conversion = float4(1.0, 1.0 / 256.0, 1.0 / (256.0 * 256.0), 1.0 / (256.0 * 256.0 * 256.0));
        return dot(color, conversion);
      }
      
      float GetHalf(float2 color)
      {
        const float2 conversion = float2(1.0, 1.0 / 256.0);
        return dot(color, conversion);
      }
      
      float4 UnpackVector(half4 colors)
      {
        float x = GetHalf(float2(colors.xy));
        float y = GetHalf(float2(colors.zw));
        float z = 1 - (sqrt(x * x + y * y));
        // 1 = sqrt(x^2 + y^2 + z^2)
        // 1 - sqrt(z^2) = sqrt(x^2 + y^2)
        // sqrt(z^2) = 1 - sqrt(x^2 + y^2)
        return float4(x*x, y*y, z*z, 1);	// 圧縮したときsqrtしている, 16bitで精度が犠牲になっているので代替策
      }
      
      float UnpackLength(half4 colors) {
        return GetFloat(colors);
      }

	  int GetIndex(float4 uv) {
        return (int)uv.x + (int)uv.y * 1000000;
      }
      
      v2f vert (appdata v) {
        v2f o;
		int id = GetIndex(v.index);
        o.pos = mul( UNITY_MATRIX_MVP, v.vertex );
        o.uv = float4( v.texcoord.xy, 0, 0 );
        return o;
      }
      
      half4 frag( v2f i ) : COLOR {
        half4 c = frac( i.uv );
        if (any(saturate(i.uv) - i.uv))
          c.b = 0.5;
        return c;
      }
      ENDCG
    }
  }
}