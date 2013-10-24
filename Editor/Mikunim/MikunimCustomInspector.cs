using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;



[CustomEditor(typeof(MikunimScript))]
public class MikunimCustomInspector : Editor {

	MikunimWindow window = null;
	SerializedObject serializedObject;
	SerializedProperty states;
	

	void OnEnable()
	{
		serializedObject = new SerializedObject(target);
		states = serializedObject.FindProperty("states");
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		if (window == null)
		{
			if (GUILayout.Button("Open Mikunim"))
			{
				InitWindow();
			}
		}
		
		serializedObject.ApplyModifiedProperties();
	}

	void InitWindow()
	{
		window = EditorWindow.GetWindow<MikunimWindow>();
		var window_rect = window.position;
		window_rect.x += 30;
		window_rect.y += 30;
		window_rect.width = 640;
		window_rect.height = 480;
		window.position = window_rect;
	}
}
