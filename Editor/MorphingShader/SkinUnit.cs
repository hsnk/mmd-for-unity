using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public abstract class AbstractSkinUnit
{
	protected string name;
	protected int square_size;
	protected int[] base_indices;

	public AbstractSkinUnit(string name, int square_size, int[] base_indices)
	{
		this.square_size = square_size;
		this.base_indices = base_indices;
		this.name = name;
	}
}

public class SkinUnit : AbstractSkinUnit
{
	int[] target_indices;
	Vector3[] vectors;

	public SkinUnit(string name, int square_size, int[] base_indices, int[] target_indices, Vector3[] vectors)
		: base(name, square_size, base_indices)
	{
		this.target_indices = target_indices;
		this.vectors = vectors;
		RestructRealIndices();
	}

	void RestructRealIndices()
	{
		for (int i = 0; i < target_indices.Length; i++)
		{
			int index = target_indices[i];
			target_indices[i] = base_indices[index];
		}
	}

	public void ConvertColors(out Color[] x, out Color[] y, out Color[] length)
	{
		int count = square_size * square_size;
		x = new Color[count];
		y = new Color[count];
		length = new Color[count];

		int iii = 0;
		for (int i = 0; i < count; i++)
		{
			if (target_indices[iii] == i)
			{
				int index = GetIndex(target_indices[iii]);
				Vector3 normal = vectors[index].normalized;
				float vector_length = vectors[index].magnitude;

				x[index] = FloatConverter.Encode32(normal.x);
				y[index] = FloatConverter.Encode32(normal.y);
				length[index] = FloatConverter.Encode32(vector_length);

				iii++;
			}
			else
			{
				x[i] = Color.black;
				y[i] = Color.black;
				length[i] = Color.black;
			}
		}
	}

	int GetIndex(int count)
	{
		int index = count;
		int u = index % square_size;
		int v = index / square_size;
		return v * square_size + u;
	}
}

public class BaseUnit : AbstractSkinUnit
{
	public BaseUnit(string name, int square_size, int[] base_indices)
		: base(name, square_size, base_indices)
	{

	}

	public Color[] ConvertColors()
	{
		int size = square_size * square_size;
		Color[] argb = new Color[size];

		int count = 0;
		for (int i = 0; i < size; i++)
		{
			if (base_indices[count] == i)
				argb[base_indices[count++]] = new Color(1, 1, 1, 1);
			else
				argb[i] = new Color(0, 0, 0, 0);
		}
		return argb;
	}
}

