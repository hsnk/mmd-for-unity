using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using System.IO;

public class TextureUnit
{
	public int Size { get; private set; }

	Texture2D texture;
	int vertices_count;
	int counter = 0;

	public TextureUnit(int vertices_count)
	{
		this.Size = MorphUtil.SquareSize(vertices_count);
		this.texture = new Texture2D(Size, Size, TextureFormat.RGBA32, false);
		this.vertices_count = vertices_count;
	}

	public void Save(string path)
	{
		var bytes = texture.EncodeToPNG();
		File.WriteAllBytes(path, bytes);
	}

	public void Set(Color[] pixels)
	{
		if (vertices_count != pixels.Length)
			throw new IndexOutOfRangeException("与えられたpixels.Lengthとvertices_countの数値がなぜか合ってない");
		texture.SetPixels(pixels);
	}
}

public class TexturePack
{
	string name;
	int vertices_count;
	TextureUnit x;
	TextureUnit y;
	TextureUnit length;

	public TexturePack(string name, int vertices_count)
	{
		this.name = name;
		this.vertices_count = vertices_count;
		x = new TextureUnit(vertices_count);
		y = new TextureUnit(vertices_count);
		length = new TextureUnit(vertices_count);
	}

	public void SetColors(Color[] x, Color[] y, Color[] length)
	{
		this.x.Set(x);
		this.y.Set(y);
		this.length.Set(length);
	}

	public void Save(string animation_path)
	{
		animation_path += "/" + name;
		x.Save(animation_path + "_x.png");
		y.Save(animation_path + "_y.png");
		length.Save(animation_path + "_length.png");
	}
}