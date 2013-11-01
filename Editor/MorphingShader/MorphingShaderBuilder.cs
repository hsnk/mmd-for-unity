using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using System.IO;

public class MorphingShaderBuilder
{
	public class TexturePathes
	{
		public class Pack
		{
			public Texture2D texture;
			public string path;

			public Pack(Texture2D texture, string path)
			{
				this.texture = texture;
				this.path = path;
			}
		}

		public Pack base_texture;
		public List<Pack> direction = new List<Pack>();
		public List<Pack> length = new List<Pack>();
	}

	public static TexturePathes BulidTextures(GameObject mmd_prefab, string path)
	{
		var mesh = mmd_prefab.GetComponent<SkinnedMeshRenderer>().sharedMesh;
		var expression = mmd_prefab.transform.FindChild("Expression");
		var writer = new MorphingTextureWriter(expression.gameObject, mesh.vertices, mesh.vertexCount);
		path = MakeExpressionPath(path);
		var texture_pathes = new TexturePathes();

		var base_texture = writer.BakeBaseTexture(expression.FindChild("base"));
		var skinned_textures = writer.BakeSkinnedTextureFromExpression();

		SaveTextures(path, base_texture, skinned_textures, texture_pathes);

		return texture_pathes;
	}

	static void SaveTextures(string path, Texture2D base_texture, MorphingTextureWriter.TexturePack[] skinned_textures, TexturePathes texture_pathes)
	{
		var base_path = path + "/" + "base.png";
		BakedTextureProvider.SaveTexture2D(base_texture, base_path);
		texture_pathes.base_texture = new TexturePathes.Pack(base_texture, base_path);
		foreach (var pack in skinned_textures)
		{
			SaveTextureAsAddPath(path, pack.name, "direction", pack.textures[0], texture_pathes.direction);
			SaveTextureAsAddPath(path, pack.name, "length", pack.textures[1], texture_pathes.length);
		}
	}

	static void SaveTextureAsAddPath(string path, string name, string tail, Texture2D texture, List<TexturePathes.Pack> pathes)
	{
		var dir_path = path + "/" + name + tail + ".png";
		BakedTextureProvider.SaveTexture2D(texture, dir_path);
		pathes.Add(new TexturePathes.Pack(texture, dir_path));
	}

	static string MakeExpressionPath(string path)
	{
		// Path.GetDirectoryNameすると変な名前に変換される
		var splited = path.Split('/');
		var new_str = "";
		for (int i = 0; i < splited.Length - 2; i++) new_str += splited[i] + "/";
		new_str += splited[splited.Length - 2];
		path = new_str;

		if (!Directory.Exists(path + "/Expression"))
			AssetDatabase.CreateFolder(path, "Expression");
		return path + "/Expression";
	}
}
