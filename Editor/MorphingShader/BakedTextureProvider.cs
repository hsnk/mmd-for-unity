using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using System.IO;

public class BakedTextureProvider
{
	/// <summary>
	/// あるパスにテクスチャのアセットを保存しておく
	/// </summary>
	/// <param name="texture"></param>
	/// <param name="path"></param>
	public static void SaveTexture2D(Texture2D texture, string path)
	{
		byte[] data = texture.EncodeToPNG();

		// Asset/Assetというパスになるのでここで修正
		var spl = path.Split('/');
		var new_str = "";
		for (int i = 1; i < spl.Length - 1; i++) new_str += spl[i] + "/";
		new_str += spl[spl.Length - 1];
		path = new_str;

		File.WriteAllBytes(Application.dataPath + "/" + path, data);
	}
}
