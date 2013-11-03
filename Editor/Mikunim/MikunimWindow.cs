using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class MikunimWindow : EditorWindow {

	Dictionary<string, Node> nodes = new Dictionary<string, Node>();
	bool do_create_node_flag = false;
	Dictionary<string, AnimationClip> selected_clips;

	void OnGUI()
	{
		EditorGUI.DrawRect(GetRoundRectFromWindowBox(8), Color.black);
		EditorGUI.DrawRect(GetRoundRectFromWindowBox(10), Color.gray);

		DrawNodes();
		ShowContextMenu();
		CheckCreateNodeFlag();
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

	void CheckCreateNodeFlag()
	{
		if (do_create_node_flag)
		{
			foreach (var e in selected_clips)
			{
				Node node = new Node(Input.mousePosition, e.Key, e.Value);
				nodes.Add(e.Key, node);
			}
			do_create_node_flag = false;
			this.Repaint();
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

	CreateNodeWindow create_window;
	void CallCreateNodeWindow(object obj)
	{
		create_window = GetWindow<CreateNodeWindow>();
		create_window.mikunim = this;
	}

	void ShowContextMenu()
	{
		MouseDriver.MenuItem[] items = {
											new MouseDriver.MenuItem("Create Node", CallCreateNodeWindow, null)
										};
		MouseDriver.ShowContextMenu(items);
	}

	public void CatchCallbackForCreateNodeFlag(Dictionary<string, AnimationClip> clips)
	{
		do_create_node_flag = true;
		selected_clips = clips;
	}
}
