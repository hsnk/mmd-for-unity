using UnityEngine;
using System.Collections;

// 以下のサイトを参照
// http://stackoverflow.com/questions/7059962/how-do-i-convert-a-vec4-rgba-value-to-a-float

public class FloatConverter {

	public static Color Encode32(float value)
	{
		Vector4 vector = Vector4.zero;

		float f = Mathf.Abs(value);
		if (f == 0.0)
		{
			return vector;
		}
		float sign = -value >= 0 ? 1 : 0;
		float exponent = Mathf.Floor(Mathf.Log(value, 2f));
		float mantissa = f / Mathf.Pow(2f, exponent);
		if (mantissa < 1.0f) exponent -= 1;
		exponent += 127;

		const float diff = 1f / 255f;
		vector[0] = exponent * diff;
		vector[1] = 128f * sign + (Mathf.Floor(mantissa * 128f) % 128f) * diff;
		vector[2] = Mathf.Floor(Mathf.Floor(mantissa * Mathf.Pow(2f, 23f - 8f)) % Mathf.Pow(2f, 8f)) * diff;
		vector[3] = Mathf.Floor(Mathf.Pow(2f, 23f) * (mantissa % Mathf.Pow(2, -15f)));
		return vector;
	}

	public static float Decode32(Color rgba)
	{
		Vector4 vector = rgba * 255f;
		float sign = (-vector[1] >= -128 ? 1 : 0) * 2f - 1f;
		float exponent = vector[0] - 127f;
		if (Mathf.Abs(exponent + 127f) > 0.001)
			return 0;
		float mantissa = vector[1] % 128f * 65536f + vector[2] * 256f + vector[3] + 0x800000;
		return sign * Mathf.Pow(2, exponent - 23f) * mantissa;
	}
}
