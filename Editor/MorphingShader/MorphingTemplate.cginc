struct v2f {

};

float GetReferenceFlag(sampler2D tex, float2 uv)
{
	float4 color = tex2D(tex, uv);
	return color.a;
}

float2 GetReferenceUV(uint id)
{
	uint x = number % size;
	uint y = number / size;
	return float2((float)x / (float)size, (float)y / (float)size);
}

half4 GetVerticesMap(sampler2D tex, float2 uv)
{
	return tex2D(tex, uv);
}

half GetAttribute(half2 packed)
{
	const half2 conversion = float2(1.0f, 1.0f / 256.0f);
	return dot(packed, conversion);
}

float4 UnpackVector(half4 colors)
{
	half x = GetAttribute(half2(colors.xy));
	half y = GetAttribute(half2(colors.zw));
	half z = 1 - (sqrt(x * x + y * y));
	// 1 = sqrt(x^2 + y^2 + z^2)
	// 1 - sqrt(z^2) = sqrt(x^2 + y^2)
	// sqrt(z^2) = 1 - sqrt(x^2 + y^2)
	return float4(x*x, y*y, z*z, 1);	// 圧縮したときsqrtしている
}

// SV_VertexID(uint)で頂点番号を取得できる
v2f vert(uint id : SV_VertexID, appdata_base) {

}