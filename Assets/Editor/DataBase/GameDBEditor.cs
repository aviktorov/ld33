using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(GameDB))]
public abstract class GameDBEditor : Editor {
	public override void OnInspectorGUI() {
		GameDB data = (GameDB) target;
		
		EditorGUILayout.BeginVertical("box");
			EditorGUILayout.LabelField("Info", EditorStyles.boldLabel);
			EditorGUILayout.Space();
			
			EditorGUILayout.LabelField(string.Format("Description: {0}", data.descriptions.Count));
			EditorGUILayout.LabelField(string.Format("Names: {0}", data.names.Count));

			EditorGUILayout.BeginHorizontal();
				if(GUILayout.Button("Import")) {
					ImportClicked();
				}
				
				if(GUILayout.Button("Clear")) {
					data.descriptions.Clear();
					data.names.Clear();
				}
			EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndVertical();
	}
	
	public abstract void ImportClicked();
}
