using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

public class MorphUtil
{
	public static int SquareSize(int vertices_count)
	{
		return (int)(Mathf.Pow(2, Mathf.Floor(Mathf.Log(vertices_count, 2)) + 1));
	}
}
