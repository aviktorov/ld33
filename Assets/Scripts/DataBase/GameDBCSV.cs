using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GameDBCSV : ScriptableObject
{
	[HideInInspector]
	public List<DescriptionData> descriptions = new List<DescriptionData>();

	[HideInInspector]
	public List<NameData> names = new List<NameData>();

	[HideInInspector]
	public List<string> types = new List<string>();

	[HideInInspector]
	public List<string> themes = new List<string>();

	[HideInInspector]
	public List<string> raports = new List<string>();

	public TextAsset descriptionData;
	public TextAsset nameData;
	public TextAsset themeData;
	public TextAsset raportData;
}
