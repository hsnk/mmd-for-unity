using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEditor;

public class SkinPack
{
	public SkinUnit Skin { get; private set; }
	public TexturePack TexturePack { get; private set; }
	public string Name { get; private set; }

	public SkinPack(string name, SkinUnit skin, TexturePack texture)
	{
		this.Skin = skin;
		this.TexturePack = texture;
		Name = name;
	}
}

public class TextureFactory
{
	int[] base_indices;
	int vertices_count;
	int square_size;
	List<SkinPack> skins = new List<SkinPack>();

	BaseUnit base_unit;
	TextureUnit base_texture;

	public TextureFactory(int vertices_count, int[] base_indices)
	{
		this.vertices_count = vertices_count;
		this.base_indices = base_indices;
		this.square_size = MorphUtil.SquareSize(vertices_count);
		InitializeBase();
	}

	void InitializeBase()
	{
		base_unit = new BaseUnit("base", this.square_size, base_indices);
		var base_color = base_unit.ConvertColors();
		base_texture = new TextureUnit(vertices_count);
		base_texture.Set(base_color);
	}

	public void SetSkin(string name, int[] target_indices, Vector3[] vectors)
	{
		SkinUnit skin = new SkinUnit(name, square_size, base_indices, target_indices, vectors);
		TexturePack pack = new TexturePack(name, square_size * square_size);
		SkinPack sknpck = new SkinPack(name, skin, pack);
		ColoringTexture(sknpck);
	}

	void ColoringTexture(SkinPack pack)
	{
		Color[] x;
		Color[] y;
		Color[] len;
		pack.Skin.ConvertColors(out x, out y, out len);
		pack.TexturePack.SetColors(x, y, len);
	}

	public SkinPack[] GetSkinTexture()
	{
		return skins.ToArray();
	}

	public void SaveTextures(string object_path)
	{
		string dir = object_path + "/Expression";
		if (!Directory.Exists(dir))
			AssetDatabase.CreateFolder(object_path, "Expression");

		foreach (var pack in skins)
			pack.TexturePack.Save(dir);	// ここでファイル3種類保存する
	}
}
