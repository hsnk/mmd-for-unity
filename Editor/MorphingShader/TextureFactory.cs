using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TextureFactory
{
	int[] base_indices;
	int vertices_count;
	int square_size;
	List<SkinUnit> skins = new List<SkinUnit>();
	List<TexturePack> textures = new List<TexturePack>();

	public TextureFactory(int vertices_count, int[] base_indices)
	{
		this.vertices_count = vertices_count;
		this.base_indices = base_indices;
		this.square_size = MorphUtil.SquareSize(vertices_count);
	}

	public void AddSkin(string name, int[] target_indices, Vector3[] vectors)
	{
		SkinUnit skin = new SkinUnit(name, square_size, base_indices, target_indices, vectors);
		TexturePack pack = new TexturePack(name, square_size * square_size);

		skins.Add(skin);
		
	}


}
