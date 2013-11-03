using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

public class Node : IDrawInterface
{
	static Vector2 prev_mouse_position;

	string title;
	Rect rect;
	bool remove_flag = false;
	bool on_node_flag = false;
	AnimationClip clip;

	public bool OnNode { get { return on_node_flag; } }
	public bool CanRemove { get { return remove_flag; } }
	public string Title { get { return title; } }

	public Node(Vector2 position, string title, AnimationClip clip)
	{
		this.title = title;
		Vector2 size = MakeNodeSize();
		rect = new Rect(position.x, position.y, size.x, size.y);
		this.clip = clip;
	}

	Vector2 MakeNodeSize()
	{
		const float const_width = 8;
		const float const_height = 32;
		return new Vector2(title.Length * const_width, const_height);
	}

	public static void UpdateMousePosition()
	{
		prev_mouse_position = Event.current.mousePosition;
	}

	void RemoveNode(object obj)
	{
		remove_flag = true;
	}

	void PointerOnNode()
	{
		if (MouseDriver.PointerOnRect(ref rect, DragNode, ShowContextMenu))
			on_node_flag = true;
		else
		{
			on_node_flag = false;
		}
	}

	void DragNode()
	{
		// 四角形の中で左クリックされたら移動させる
		var speed = Event.current.mousePosition - prev_mouse_position;
		Debug.Log(speed.ToString());
		rect.x += speed.x;
		rect.y += speed.y;
	}

	void ShowContextMenu()
	{
		MouseDriver.MenuItem[] menu = {
										  new MouseDriver.MenuItem("Remove", RemoveNode, null)
									  };
		MouseDriver.ShowContextMenu(menu);
	}

	public void Draw()
	{
		PointerOnNode();
		UpdateMousePosition();
		EditorGUI.DrawRect(rect, Color.white);
		GUI.Label(rect, title);
	}
}