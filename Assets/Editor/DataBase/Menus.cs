#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public static class Menus {
	[MenuItem("Assets/Create/Room Game/New Game CSV DB")]
	public static void CreateGameDBCSV() {
		GameDBCSV db = ScriptableObject.CreateInstance<GameDBCSV>();
		AssetDatabase.CreateAsset(db,"Assets/NewGameDBCSV.asset");
		AssetDatabase.SaveAssets();
		
		EditorUtility.FocusProjectWindow();
		Selection.activeObject = db;
	}
}

#endif
