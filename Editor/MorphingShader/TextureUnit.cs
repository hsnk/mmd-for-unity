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
	Color[] pixels;
	int vertices_count;
	int counter = 0;

	public TextureUnit(int vertices_count)
	{
		this.Size = MorphUtil.SquareSize(vertices_count);
		this.texture = new Texture2D(Size, Size, TextureFormat.RGBA32, false);
		this.pixels = this.texture.GetPixels();
		this.vertices_count = vertices_count;
	}

	public void Save(string path)
	{
		var bytes = texture.EncodeToPNG();
		File.WriteAllBytes(path, bytes);
	}

	/// <summary>
	/// 値を入れていく．
	/// カウンターが自動的に回ってるのでwhile(Set(array[i++]))でいい．
	/// </summary>
	/// <param name="value">入れたい数値</param>
	/// <returns>値域がsize^2より小さければtrue</returns>
	public bool Set(float value)
	{
		if (counter < vertices_count)
		{
			pixels[counter] = FloatConverter.Encode32(value);
			counter++;
			return true;
		}
		return false;
	}
}

