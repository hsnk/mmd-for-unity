Shader "Debug/UV 1" {
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