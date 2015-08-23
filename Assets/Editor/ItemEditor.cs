using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Item))]
public class ItemEditor : Editor {
	private int index = 0;
	private string[] types;

	public void OnEnable () {
		GameDB db = AssetDatabase.LoadAssetAtPath<GameDB>("Assets/DataBase/Items.asset");
		types = db.types.ToArray();
	}

	public override void OnInspectorGUI() {
		Item item = (Item) target;

		index = EditorGUILayout.Popup("Type", index, types);
		item.type = types[index];
	}
}