using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(GameDB))]
public class GameDBEditor : Editor {
	public override void OnInspectorGUI() {
		GameDB data = (GameDB) target;

		EditorGUILayout.BeginVertical("box");
			data.key = EditorGUILayout.TextField("Key", data.key);

			EditorGUILayout.LabelField("Info", EditorStyles.boldLabel);
			EditorGUILayout.Space();
			
			EditorGUILayout.LabelField(string.Format("Description: {0}", data.descriptions.Count));
			EditorGUILayout.LabelField(string.Format("Names: {0}", data.names.Count));
			EditorGUILayout.LabelField(string.Format("Types: {0}", data.types.Count));
			EditorGUILayout.LabelField(string.Format("Themes: {0}", data.themes.Count));
			EditorGUILayout.LabelField(string.Format("Raports: {0}", data.raports.Count));

			EditorGUILayout.BeginHorizontal();
				if(GUILayout.Button("Import")) {
					data.Import();
				}
				
				if(GUILayout.Button("Clear")) {
					data.descriptions.Clear();
					data.names.Clear();
					data.types.Clear();
					data.themes.Clear();
					data.raports.Clear();
				}
			EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndVertical();
	}
}
