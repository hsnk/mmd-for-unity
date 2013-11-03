using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

public class CreateNodeWindow : EditorWindow
{
	public MikunimWindow mikunim;
	UnityEngine.Object[] objects;
	Dictionary<string, AnimationClip> clips = new Dictionary<string, AnimationClip>();

	void OnGUI()
	{
		GUILayout.Label("Select Animation Clip");

		// 毎フレーム初期化する
		objects = Selection.objects;
		if (objects.Length > 0)
		{
			clips = new Dictionary<string, AnimationClip>();

			foreach (var obj in objects)
			{
				var asset = obj as AnimationClip;
				if (asset != null)
				{
					// AnimationClipに変換できる
					clips.Add(asset.name, asset);
				}
			}
		}

		if (GUILayout.Button("Import"))
		{
			if (clips.Count > 0)
				mikunim.CatchCallbackForCreateNodeFlag(clips);
		}
	}
}
