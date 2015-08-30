using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(TextTranslation))]
public class TextTranslationEditor : Editor {
	private string[] labels;

	public void OnEnable () {
		GameDBCSV db = AssetDatabase.LoadAssetAtPath<GameDBCSV>("Assets/DataBase/English/ItemsCSV.asset");
		labels = db.labels.ToArray();
	}

	public override void OnInspectorGUI() {
		TextTranslation translation = (TextTranslation) target;

		translation.index = EditorGUILayout.Popup("Label", translation.index, labels);
	}
}