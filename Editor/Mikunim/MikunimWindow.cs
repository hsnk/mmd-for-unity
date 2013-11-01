﻿using UnityEngine;
using System.Collections;
using UnityEditor;

public class MikunimWindow : EditorWindow {

	void OnGUI()
	{
		EditorGUI.DrawRect(GetRoundRectFromWindowBox(8), Color.black);
		EditorGUI.DrawRect(GetRoundRectFromWindowBox(10), Color.gray);
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
}
