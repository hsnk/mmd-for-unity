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


public class MorphingTextureWriter
{
	int vertices_count;
	List<Transform> children;
	Transform child_base;

	Texture2D base_texture;
	int[] base_indices;

	public MorphingTextureWriter(GameObject expression, Vector3[] vertices, int vertices_count)
	{
		children = GetChildren(expression);
		child_base = children.Find(x => { return x.name == "base"; });	// baseだけ取り除く 
		children.Remove(child_base);
		this.vertices_count = vertices_count;
	}

	List<Transform> GetChildren(GameObject target)
	{
		var list = new List<Transform>();
		var transform = target.transform;
		for (int i = 0; i < transform.childCount; i++)
			list.Add(transform.GetChild(i));
		return list;
	}

	public class TexturePack
	{
		public string name;
		public Texture2D[] textures;

		public TexturePack(Transform target, Texture2D[] textures)
		{
			this.name = target.name;
			this.textures = textures;
		}
	}

	public TexturePack[] BakeSkinnedTextureFromExpression()
	{
		var textures = new List<TexturePack>();
		foreach (var child in children)
		{
			textures.Add(BakeSkinnedTexture(child));
		}
		return textures.ToArray();
	}

	/// <summary>
	/// ベクトルの情報を入れているテクスチャを返す
	/// </summary>
	/// <param name="target"></param>
	/// <param name="vertices_count"></param>
	/// <returns>要素0はx, y (それぞれ16bit)，要素1はベクトルの長さ (32bit)</returns>
	TexturePack BakeSkinnedTexture(Transform target)
	{
		var script = target.GetComponent<MMDSkinsScript>();
		var indices = ConvertBaseIndicesToIndices(script.targetIndices);
		var vectors = script.morphTarget;
		int size = SquareSize(vertices_count);

		Texture2D[] textures = new Texture2D[2];
		textures[0] = MakeBakedTexture(indices, vectors, size, WriteXYToColors);			// 0 ... x, y (それぞれ16bit)
		textures[1] = MakeBakedTexture(indices, vectors, size, WriteVectorLengthToColors);	// 1 ... ベクトルの長さ

		return new TexturePack(target, textures);
	}

	delegate void WriteMethod(Color[] colors, ref Vector3 vector, int size, int x, int y);

	Texture2D MakeBakedTexture(int[] converted_indices, Vector3[] vectors, int size, WriteMethod method)
	{
		var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
		Color[] colors = GetSkinnedColors(size);
		for (int i = 0; i < converted_indices.Length; i++)
		{
			int y = converted_indices[i] / size;
			int x = converted_indices[i] % size;

			method(colors, ref vectors[i], size, x, y);
		}
		texture.SetPixels(colors);
		return texture;
	}

	void WriteVectorLengthToColors(Color[] colors, ref Vector3 vector, int size, int x, int y)
	{
		var bytes = BitConverter.GetBytes(vector.magnitude);
		float div = 1f / 255f;
		var color = new Color(bytes[0] * div, bytes[1] * div, bytes[2] * div, bytes[3] * div);
		colors[y * size + x] = color;
	}

	void WriteXYToColors(Color[] colors, ref Vector3 vector, int size, int x, int y)
	{
		var normalized = vector.normalized;
		byte[] xx = EncodeFloatTo2BytesFromHalf(normalized.x);	// 16bitで情報量減ると困るので，こうして情報量増やす 
		byte[] yy = EncodeFloatTo2BytesFromHalf(normalized.y);	// 非常に小さい数値でも表現できると思う 

		float div = 1f / 255f;
		var color = new Color(xx[0] * div, xx[1] * div, yy[0] * div, yy[1] * div);
		colors[y * size + x] = color;
	}

	/// <summary>
	/// float型からhalf型へ変換し，byte[2]を返す
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	byte[] EncodeFloatTo2BytesFromHalf(float value)
	{
		value = Mathf.Sqrt(value);
		return Half.GetBytes(new Half(value));
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

	/// <summary>
	/// baseに登録されている頂点番号をテクスチャへ焼きこむ 
	/// テクスチャはアルファ値のみ持っている
	/// </summary>
	/// <param name="base_transform"></param>
	/// <param name="vertices_count"></param>
	/// <returns></returns>
	public Texture2D BakeBaseTexture(Transform base_transform)
	{
		var script = base_transform.GetComponent<MMDSkinsScript>();
		var indices = script.targetIndices;
		base_indices = indices;
		int size = SquareSize(vertices_count);

		Texture2D texture = new Texture2D(size, size, TextureFormat.Alpha8, false);

		// indicesを色に変換する作業
		var skinned_colors = GetSkinnedColors(size);
		for (int i = 0; i < indices.Length; i++)
			skinned_colors[indices[i]] = new Color(1, 1, 1, 1);
		texture.SetPixels(skinned_colors);

		return texture;
	}

	/// <summary>
	/// SetPixelsに渡すためのColor配列（全部黒）を生成する
	/// </summary>
	/// <param name="size"></param>
	/// <returns></returns>
	Color[] GetSkinnedColors(int size)
	{
		Color[] skinned_colors = new Color[size * size];
		var square = size * size;
		for (int i = 0; i < square; i++)
			skinned_colors[i] = new Color(0, 0, 0, 0);
		return skinned_colors;
	}

	/// <summary>
	/// 頂点数から2^nとなるテクスチャの辺の長さを返す
	/// </summary>
	/// <param name="vertices_count"></param>
	/// <returns></returns>
	int SquareSize(int vertices_count)
	{
		int non_square = (int)Mathf.Sqrt(vertices_count);
		int log2size = (int)Mathf.Log(non_square, 2) + 1;
		return 1 << (log2size + 1);
	}
}
