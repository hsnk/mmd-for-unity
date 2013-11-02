using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

public class Node : IDrawInterface
{
	static Vector3 prev_mouse_position;

	string title;
	Rect rect;
	bool remove_flag = false;
	bool on_node_flag = false;

	public bool OnNode { get { return on_node_flag; } }
	public bool CanRemove { get { return remove_flag; } }
	public string Title { get { return title; } }

	public Node(Vector2 position, string title)
	{
		this.title = title;
		Vector2 size = MakeNodeSize();
		rect = new Rect(position.x, position.y, size.x, size.y);
	}

	Vector2 MakeNodeSize()
	{
		const float const_width = 8;
		const float const_height = 32;
		return new Vector2(title.Length * const_width, const_height);
	}

	public static void UpdateMousePosition()
	{
		prev_mouse_position = Input.mousePosition;
	}

	bool CheckMousePositionOnRect()
	{
		// マウスポインタと箱の当たり判定
		var pos = Input.mousePosition;
		return
			pos.x > rect.x && pos.x < rect.x + rect.width &&
			pos.y > rect.y && pos.y < rect.y + rect.height;
	}

	void RemoveNode(object obj)
	{
		remove_flag = true;
	}

	void PointerOnNode()
	{
		if (CheckMousePositionOnRect())
		{
			on_node_flag = true;
			if (Input.GetMouseButton(0))
			{
				DragNode();
			}
			if (Input.GetMouseButton(1))
			{
				ShowContextMenu();
			}
		}
		else
		{
			on_node_flag = false;
		}
	}

	void DragNode()
	{
		// 四角形の中で左クリックされたら移動させる
		var speed = Input.mousePosition - prev_mouse_position;
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
		EditorGUI.DrawRect(rect, Color.white);
		GUI.Label(rect, title);
		PointerOnNode();
	}
}