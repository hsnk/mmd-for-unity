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
		Color[] argb = new Color[square_size * square_size];

		return argb;
	}
}

public class TextureConverter
{
	int[] base_indices;
	int vertices_count;
	int square_size;
	List<SkinUnit> packs = new List<SkinUnit>();

	public TextureConverter(int vertices_count, int[] base_indices)
	{
		this.vertices_count = vertices_count;
		this.base_indices = base_indices;
		this.square_size = MorphUtil.SquareSize(vertices_count);
	}

	public void AddSkin(string name, int[] target_indices, Vector3[] vectors)
	{
		SkinUnit pack = new SkinUnit(name, square_size, base_indices, target_indices, vectors);
		packs.Add(pack);
	}

	
}
