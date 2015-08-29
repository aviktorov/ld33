using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Item))]
public class ItemEditor : Editor {
	private string[] types;

	public void OnEnable () {
		GameDBCSV db = AssetDatabase.LoadAssetAtPath<GameDBCSV>("Assets/DataBase/English/ItemsCSV.asset");
		types = db.types.ToArray();
	}

	public override void OnInspectorGUI() {
		Item item = (Item) target;

		item.index = EditorGUILayout.Popup("Type", item.index, types);
	}
}