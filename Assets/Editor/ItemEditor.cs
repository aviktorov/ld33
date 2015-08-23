using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Item))]
public class ItemEditor : Editor {
	private string[] types;

	public void OnEnable () {
		GameDB db = AssetDatabase.LoadAssetAtPath<GameDB>("Assets/DataBase/Items.asset");
		types = db.types.ToArray();
	}

	public override void OnInspectorGUI() {
		Item item = (Item) target;

		item.index = EditorGUILayout.Popup("Type", item.index, types);
		item.type = types[item.index];
	}
}