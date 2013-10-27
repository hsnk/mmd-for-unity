using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UVWrapper
{
	public static Vector2[] WrapPositionIndicesToUV2(Mesh mesh)
	{
		Vector2[] uv = new Vector2[mesh.vertices.Length];

		for (int i = 0; i < mesh.vertices.Length; i++)
		{
			uv[i].y = (int)(i / 1000000);
			uv[i].x = (int)(i % 1000000);
		}
		mesh.uv2 = uv;
		return uv;
	}
}

