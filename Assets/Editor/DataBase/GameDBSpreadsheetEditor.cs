using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(GameDBSpreadsheet))]
public class GameDBSpreadsheetEditor : GameDBEditor
{
	public override void OnInspectorGUI() {
		GameDBSpreadsheet data = (GameDBSpreadsheet) target;
		
		EditorGUILayout.BeginVertical("box");
		data.key = EditorGUILayout.TextField("Key", data.key);
		EditorGUILayout.EndVertical();
		
		base.OnInspectorGUI();
	}
	
	public override void ImportClicked() {
		GameDBSpreadsheet data = (GameDBSpreadsheet) target;
		data.Import();
	}
}
