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
	int size2;
	int counter = 0;

	public TextureUnit(int size)
	{
		this.Size = size;
		this.texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
		this.pixels = this.texture.GetPixels();
		this.size2 = Size * Size;
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
		if (counter < size2)
		{
			pixels[counter] = FloatConverter.Encode32(value);
			counter++;
			return true;
		}
		return false;
	}
}

