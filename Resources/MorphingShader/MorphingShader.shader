Shader "Debug/Test 1" {
    Properties {
        _MainTex ("Main", 2D) = "white" {}
        _TextureSize ("Size", Range(0.0, 65536.0)) = 0
        _X1 ("name X", 2D) = "black" {}
        _Y1 ("name Y", 2D) = "black" {}
        _L1 ("name Length", 2D) = "black" {}
        _W1 ("name Weight", Range(0, 0)) = 0
    }

    SubShader {
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // 宣言 //////////////////////////////////
            sampler2D _MainTex;
            float _TextureSize;
            sampler2D _X1;
            sampler2D _Y1;
            sampler2D _L1;
            float _W1;
            //////////////////////////////////////////

            struct v2f {
                float4 pos : SV_POSITION;
                float3 color : COLOR0;
            };
            
            // Colorに圧縮したfloatを戻す
            float UnpackFloat4(float4 _packed) {
                float4 rgba = _packed * 255;
                
                float sign = step(-128.0, -rgba.y) * 2.0 - 1.0;
                float exponent = rgba.x - 127.0;
            
                if (abs(exponent - 127.0) < 0.001)
                    return 0.0;
            
                float mantissa = fmod(rgba.x, 128.0) * 65536.0 + rgba.z * 256.0 + rgba.w + (0x800000);
                return sign * exp2(exponent - 23.0) * mantissa;
            }

            inline int ColorToInt(float4 argb) {
                float unpacked_float = UnpackFloat4(argb);
                return (int)unpacked_float;
            }

            // 頂点インデックスからテクスチャを参照するUV座標を取得する
            inline float4 IndexToUV(int id, int size) {
                float u = (id % size) / size;
                float v = (id / size) / size;
                return float4(u, v, 0, 0);
            }

            // テクスチャを参照してUnpackされたデータを取り出す
            inline float GetData(sampler2D _texture, float4 uv) {
                float4 rgba = tex2Dlod(_texture, uv);
                return UnpackFloat4(rgba);
            }

            // sqrt(x^2 + y^2 + z^2) = 1
            // x^2 + y^2 + z^2 = 1
            // x^2 + y^2 = 1 - z^2
            // -z^2 = x^2 + y^2 - 1
            //  z^2 =-x^2 - y^2 + 1
            float4 GetDirection(sampler2D _x, sampler2D _y, sampler2D _len, float4 _uv) {
                float xxx = GetData(_x, _uv);
                float yyy = GetData(_y, _uv);
                float zzz = xxx * xxx - yyy * yyy + 1.0;
                float len = GetData(_len, _uv);
                return float4(xxx, yyy, sqrt(zzz), 0) * len;
            }

            v2f vert(appdata_full v) {
                v2f o;
                int id = ColorToInt(v.color);
                int size = (int)_TextureSize;


                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                return o;
            }

            half4 frag (v2f i) : COLOR {
                return half4(i.color, 1);
            }
            ENDCG
        }
    }
    Fallback "VertexLit"
}