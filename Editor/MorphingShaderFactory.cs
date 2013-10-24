using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

/*
 * MEMO:
 * キャメル = メンバ変数
 * スネーク = ローカル変数
 * で，書いていたけど気持ち悪い人もいるらしいので
 * 試験的にスネークで統一しています
 */


public class MorphingShaderFactory
{
	int vertices_count;
	List<Transform> children;
	Transform child_base;
	Vector3[] vertices;

	Texture2D base_texture;
	int[] base_indices;

	public MorphingShaderFactory(GameObject expression, Vector3[] vertices, int vertices_count)
	{
		children = GetChildren(expression);
		child_base = children.Find(x => { return x.name == "base"; });	// baseだけ取り除く
		children.Remove(child_base);
		this.vertices = vertices;
	}

	List<Transform> GetChildren(GameObject target)
	{
		var list = new List<Transform>();
		var transform = target.transform;
		for (int i = 0; i < transform.childCount; i++)
			list.Add(transform.GetChild(i));
		return list;
	}

	Texture2D[] BakeSkinnedTexture(Transform target, int vertices_count)
	{
		var script = target.GetComponent<MMDSkinsScript>();
		var indices = ConvertBaseIndicesToIndices(script.targetIndices);
		var vectors = script.morphTarget;
		int size = SquareSize(vertices_count);
		int square = size * size;

		Texture2D[] textures = new Texture2D[2];
		textures[0] = new Texture2D(size, size, TextureFormat.RGBA32, false);	// 0...x要素とy要素(それぞれ16bit)
		textures[1] = new Texture2D(size, size, TextureFormat.RGB24, false);	// 1...ベクトルの長さ, alphaがないので注意
		Color[] colors = GetSkinnedColors(size);

		for (int i = 0; i < indices.Length; i++)
		{
			int index = indices[i];
			int y = indices[i] / size;
			int x = indices[i] % size;

			var normalized = vectors[index].normalized;
			float xx = Mathf.Sqrt(normalized.x);
			float yy = Mathf.Sqrt(normalized.y);
			var byte_x = BitConverter.GetBytes(xx);
			var byte_y = BitConverter.GetBytes(yy);
		}

		return textures;
	}

	/*
	 * 以下を参考
	 * http://www.platinumgames.co.jp/programmer_blog/?p=484
	 * http://www.platinumgames.co.jp/programmer_blog/?p=594
	 */
	byte[] Convert16bit(float value) 
	{
		// 1:5:10で表現する
		// 1:6:9の方法もあるが細かすぎるのはいらんと思う（見えない，わからない）
		var bytes = BitConverter.GetBytes(value);
		var new_bytes = new byte[16];
		new_bytes[15] = bytes[31];		// 符号部

		for (int i = 0; i < 10; i++)
			new_bytes[i] = bytes[i + 13];	// 仮数部

		var exponent_bytes = new byte[32];	// 指数部
		for (int i = 0; i < 8; i++)
			exponent_bytes[i] = bytes[i + 23];
		int exponent = BitConverter.ToInt32(exponent_bytes, 0);
		exponent -= 127;
		exponent += 15;
		exponent_bytes = BitConverter.GetBytes(exponent);

		for (int i = 0; i < 5; i++)
			new_bytes[i + 10] = exponent_bytes[i];

		return new_bytes;
	}

	int[] ConvertBaseIndicesToIndices(int[] target_indices)
	{
		for (int i = 0; i < target_indices.Length; i++)
		{
			int real_index = base_indices[target_indices[i]];
			target_indices[i] = real_index;
		}
		return target_indices;
	}

	Texture2D BakeBaseTexture(Transform base_transform, int vertices_count)
	{
		var script = base_transform.GetComponent<MMDSkinsScript>();
		var indices = script.targetIndices;
		base_indices = indices;
		int size = SquareSize(vertices_count);

		Texture2D texture = new Texture2D(size, size, TextureFormat.Alpha8, false);	// RGBAの順番なので注意

		// indicesを色に変換する作業
		var skinned_colors = GetSkinnedColors(size);
		for (int i = 0; i < indices.Length; i++)
			skinned_colors[indices[i]] = new Color(1, 1, 1, 1);
		texture.SetPixels(skinned_colors);

		return texture;
	}

	Color[] GetSkinnedColors(int size)
	{
		Color[] skinned_colors = new Color[size * size];
		var square = size * size;
		for (int i = 0; i < square; i++)
			skinned_colors[i] = new Color(0, 0, 0, 0);
		return skinned_colors;
	}

	int SquareSize(int vertices_count)
	{
		int non_square = (int)Mathf.Sqrt(vertices_count);
		int log2size = (int)Mathf.Log(non_square, 2) + 1;
		return 1 << (log2size + 1);
	}
}
