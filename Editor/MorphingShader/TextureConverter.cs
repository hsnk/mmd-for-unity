using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

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
