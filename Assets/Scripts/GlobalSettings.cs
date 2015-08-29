using UnityEngine;
using System.Collections;

[System.Serializable]
public enum Language {
	English,
	Russian
}

public class GlobalSettings : MonoBehaviour {
	private static GlobalSettings _instance;

	public static GlobalSettings instance {
		get {
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType<GlobalSettings>();
				DontDestroyOnLoad(_instance.gameObject);
			}

			return _instance;
		}
	}

	public bool win = false;
	public Language language = Language.English;

	[Header("DataBase")]
	public GameDBCSV englishDB;
	public GameDBCSV russianDB;

	[HideInInspector]
	public GameDBCSV db {
		get {
			switch (language) {
				case Language.English: return englishDB;
				case Language.Russian: return russianDB;
			}

			return englishDB;
		}
	}

	private void Awake() {
		if(_instance == null) {
			_instance = this;
			DontDestroyOnLoad(this);
		}
		else {
			if (this != _instance)
				Destroy(gameObject);
		}
	}
}