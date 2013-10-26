float GetReferenceFlag(sampler2D tex, float2 uv)
{
	float4 color = tex2D(tex, uv);
	return color.a;
}

float4 GetVerticesMap(sampler2D tex, float4 pos, int size)
{
	int number = pos.w;
	int x = number % size;
	int y = number / size;
	float2 uv = float2((float)x / (float)size, (float)y / (float)size);

	return tex2D(float2(u, v));
}

half GetX(float4 color)
{
	int r = int(color.r * 255);
	int g = int(color.g * 255);
	int rg = r + (g << 8);
}

