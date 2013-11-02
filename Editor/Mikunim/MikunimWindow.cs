using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class MikunimWindow : EditorWindow {

	Dictionary<string, Node> nodes = new Dictionary<string, Node>();
	bool is_selected_animation_clip = false;
	AnimationClip[] selected_clips;

	void OnGUI()
	{
		EditorGUI.DrawRect(GetRoundRectFromWindowBox(8), Color.black);
		EditorGUI.DrawRect(GetRoundRectFromWindowBox(10), Color.gray);

		DrawNodes();
		ShowContextMenu();
	}

	// ノードの上にポインターが乗ってるかチェックする
	bool CheckMousePointerOnNode()
	{
		foreach (var e in nodes)
		{
			if (e.Value.OnNode)
				return true;
		}
		return false;
	}

	void DrawNodes()
	{
		foreach (var e in nodes)
		{
			e.Value.Draw();
		}
	}

	Rect GetRoundRectFromWindowBox(float thickness)
	{
		float x = thickness;
		float y = thickness;
		float w = position.width - thickness;
		float h = position.height - thickness;
		var rect = new Rect(x, y, w, h);
		rect.center = new Vector2(position.width * 0.5f, position.height * 0.5f);
		return rect;
	}

	void CreateNode(object obj)
	{
		
	}

	void ShowContextMenu()
	{
		MouseDriver.MenuItem[] items = {
											new MouseDriver.MenuItem("Create Node", CreateNode, null)
										};
		MouseDriver.ShowContextMenu(items);
	}

	public void CatchCallbackForSelectionFlag(AnimationClip[] clips)
	{
		is_selected_animation_clip = true;
		selected_clips = clips;
	}
}
