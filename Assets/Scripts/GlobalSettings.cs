using UnityEngine;
using System.Collections;

public static class GlobalSettings {
	public static bool win = false;

	public static GameDBCSV db {
		get {
			if (_db == null) {
				// TODO: Remove
				_db = GameObject.FindWithTag("LoadDB").GetComponent<LoadDB>().db;
				//GameDB loadDB = ScriptableObject.CreateInstance<GameDB>();
				//if (loadDB.Import()) {
				//	_db = loadDB;
				//}
				//else {
				//	_db = AssetDatabase.LoadAssetAtPath<GameDB>("Assets/DataBase/Items.asset");
				//}
			}

			return _db;
		}
	}

	private static GameDBCSV _db = null;
}
