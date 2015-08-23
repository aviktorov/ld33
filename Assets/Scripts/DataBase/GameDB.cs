using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class DescriptionData {
	public string type;
	public string price;
	public string probability;
	public string text;
	public string group;
	public string mimic;
}

[System.Serializable]
public class NameData {
	public string type;
	public string name;
}

[System.Serializable]
public class GameDB : ScriptableObject {
	[HideInInspector]
	public List<DescriptionData> descriptions = new List<DescriptionData>();

	[HideInInspector]
	public List<NameData> names = new List<NameData>();
}
