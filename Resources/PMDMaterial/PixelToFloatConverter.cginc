/*
float GetReferenceFlag(sampler2D tex, float2 uv)
{
	float4 color = tex2D(tex, uv);
	return color.a;
}

float2 GetReferenceUV(int id, int size)
{
	int x = id % size;
	int y = id / size;
	return float2((float)x / (float)size, (float)y / (float)size);
}

half4 GetVerticesMap(sampler2D tex, float2 uv)
{
	return tex2D(tex, uv);
}

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
	return uv.x + uv.y * 1000000;
}
*/