using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class SkinUnit
{
	int[] base_indices;

	class SkinPack
	{
		string name;
		int[] target_indices;
		Vector3[] vectors;

		public SkinPack(string name, int[] target_indices, Vector3[] vectors)
		{
			this.name = name;
			this.target_indices = target_indices;
			this.vectors = vectors;
		}
	}
	List<SkinPack> packs = new List<SkinPack>();

	public SkinUnit(int[] base_indices)
	{
		this.base_indices = base_indices;
	}

	public void AddSkin(string name, int[] target_indices, Vector3[] vectors)
	{
		SkinPack pack = new SkinPack(name, target_indices, vectors);
		packs.Add(pack);
	}


}
