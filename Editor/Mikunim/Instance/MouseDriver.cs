using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

/// <summary>
/// マウスがRectの上に乗った状態で動作する何か
/// </summary>
public class MouseDriver
{
	delegate void DrivenMethod();

	static bool CheckMousePositionOnRect(ref Rect rect)
	{
		// マウスポインタと箱の当たり判定
		var pos = Input.mousePosition;
		return
			pos.x > rect.x && pos.x < rect.x + rect.width &&
			pos.y > rect.y && pos.y < rect.y + rect.height;
	}

	public static bool PointerOnRect(ref Rect rect, DrivenMethod left_click, DrivenMethod right_click)
	{
		bool on_node_flag = false;
		if (CheckMousePositionOnRect(ref rect))
		{
			on_node_flag = true;
			if (Input.GetMouseButton(0))
			{
				left_click();
			}
			if (Input.GetMouseButton(1))
			{
				right_click();
			}
		}
		return on_node_flag;
	}

	public class MenuItem
	{
		public GenericMenu.MenuFunction2 method;
		public object argument;
		public string title;

		public MenuItem(string title, GenericMenu.MenuFunction2 method, object argument)
		{
			this.method = method;
			this.title = title;
			this.argument = argument;
		}
	}

	public static void ShowContextMenu(MenuItem[] item)
	{
		Event evt = Event.current;
		if (evt.type == EventType.ContextClick)
		{
			// ノード上で右クリックした
			GenericMenu menu = new GenericMenu();
			for (int i = 0; i < item.Length; i++)
			{
				menu.AddItem(new GUIContent(item[i].title), false, item[i].method, item[i].argument);
			}
			menu.ShowAsContext();
			evt.Use();
		}
	}
}
