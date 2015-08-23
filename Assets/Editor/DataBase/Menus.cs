#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public static class Menus {
	[MenuItem("Assets/Create/Room Game/New Game DB Spreadsheet")]
	public static void CreateGameDBSpreadsheet() {
		GameDBSpreadsheet db = ScriptableObject.CreateInstance<GameDBSpreadsheet>();
		AssetDatabase.CreateAsset(db,"Assets/NewGameDBSpreadsheet.asset");
		AssetDatabase.SaveAssets();
		
		EditorUtility.FocusProjectWindow();
		Selection.activeObject = db;
	}
}

#endif
