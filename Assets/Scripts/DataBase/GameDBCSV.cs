using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class DescriptionData {
	public string type;
	public float probability;
	public string text;
	public int group;
	public bool mimic;
	public string theme;
}

[System.Serializable]
public class NameData {
	public string type;
	public string name;
	public string price;
}

[System.Serializable]
public class TextTranslationData {
	public string label;
	public string translation;
}

[System.Serializable]
public class GameDBCSV : ScriptableObject {
	[HideInInspector]
	public List<DescriptionData> descriptions = new List<DescriptionData>();

	[HideInInspector]
	public List<NameData> names = new List<NameData>();

	[HideInInspector]
	public List<TextTranslationData> translations = new List<TextTranslationData>();

	[HideInInspector]
	public List<string> types = new List<string>();

	[HideInInspector]
	public List<string> themes = new List<string>();

	[HideInInspector]
	public List<string> labels = new List<string>();

	public TextAsset descriptionData;
	public TextAsset nameData;
	public TextAsset themeData;
	public TextAsset textTranslationData;

	public string GetTranslation(int index) {
		foreach (var t in translations) {
			if (t.label == labels[index])
				return t.translation;
		}
		return "";
	}

	public string GetTranslation(string label) {
		foreach (var t in translations) {
			if (t.label == label)
				return t.translation;
		}
		return "";
	}
}
