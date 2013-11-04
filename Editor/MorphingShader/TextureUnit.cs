using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using System.IO;

public class TextureUnit
{
	Texture2D texture;
	int vertices_count;

	public int Size { get; private set; }
	public Texture2D Texture { get { return texture; } }

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
	public TextureUnit X { get; private set; }
	public TextureUnit Y { get; private set; }
	public TextureUnit Length { get; private set; }

	public TexturePack(string name, int vertices_count)
	{
		this.name = name;
		X = new TextureUnit(vertices_count);
		Y = new TextureUnit(vertices_count);
		Length = new TextureUnit(vertices_count);
	}

	public void SetColors(Color[] x, Color[] y, Color[] length)
	{
		this.X.Set(x);
		this.Y.Set(y);
		this.Length.Set(length);
	}

	public void Save(string expression_path)
	{
		expression_path += "/" + name;
		X.Save(expression_path + "_x.png");
		Y.Save(expression_path + "_y.png");
		Length.Save(expression_path + "_length.png");
	}
}