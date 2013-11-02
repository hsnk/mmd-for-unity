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
	List<AnimationClip> clips = new List<AnimationClip>();

	void OnGUI()
	{


		GUILayout.Label("Select Animation Clip");

		// 毎フレーム初期化する
		objects = Selection.objects;
		if (objects.Length > 0)
		{
			clips = new List<AnimationClip>();

			foreach (var obj in objects)
			{
				var asset = obj as AnimationClip;
				if (asset != null)
				{
					// AnimationClipに変換できる
					clips.Add(asset);
				}
			}
		}

		if (GUILayout.Button("Convert VMD Files"))
		{
			if (clips.Count > 0)
				mikunim.CatchCallbackForSelectionFlag(clips.ToArray());
		}
	}
}
