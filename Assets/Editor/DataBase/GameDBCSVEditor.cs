using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[CustomEditor(typeof(GameDBCSV))]
public class GameDBCSVEditor : Editor
{
	public override void OnInspectorGUI()
	{
		GameDBCSV data = (GameDBCSV)target;
		
		EditorGUILayout.BeginVertical("box");
			EditorGUILayout.LabelField("Info", EditorStyles.boldLabel);
			EditorGUILayout.Space();
			
			EditorGUILayout.LabelField(string.Format("Description: {0}", data.descriptions.Count));
			EditorGUILayout.LabelField(string.Format("Names: {0}", data.names.Count));
			EditorGUILayout.LabelField(string.Format("Types: {0}", data.types.Count));
			EditorGUILayout.LabelField(string.Format("Themes: {0}", data.themes.Count));
			EditorGUILayout.LabelField(string.Format("Translations: {0}", data.translations.Count));
			EditorGUILayout.LabelField(string.Format("Labels: {0}", data.labels.Count));
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.BeginVertical("box");
			EditorGUILayout.LabelField("CSV Data", EditorStyles.boldLabel);
			EditorGUILayout.Space();
			data.descriptionData = (TextAsset)EditorGUILayout.ObjectField("Description",data.descriptionData,typeof(TextAsset),false);
			data.nameData = (TextAsset)EditorGUILayout.ObjectField("Names",data.nameData,typeof(TextAsset),false);
			data.themeData = (TextAsset)EditorGUILayout.ObjectField("Themes",data.themeData,typeof(TextAsset),false);
			data.textTranslationData = (TextAsset)EditorGUILayout.ObjectField("Translations",data.textTranslationData,typeof(TextAsset),false);
			EditorGUILayout.Space();
			EditorGUILayout.BeginHorizontal();
				if(GUILayout.Button("Import")) {
					ImportClicked();
				}
				
				if(GUILayout.Button("Clear")) {
					data.descriptions.Clear();
					data.names.Clear();
					data.types.Clear();
					data.themes.Clear();
					data.translations.Clear();
					data.labels.Clear();
				}
			EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndVertical();
		
		EditorUtility.SetDirty(data);
	}
	
	public void ImportClicked()
	{
		GameDBCSV data = (GameDBCSV)target;
		
		ImportDescriptionData(data.descriptionData,data.descriptions);
		ImportTypesData(data.nameData,data.names);
		ImportListData(data.themeData,data.types,data.themes);
		ImportTextTranslationData(data.textTranslationData,data.translations);
		ImportLabelData(data.textTranslationData,data.labels);
	}
	
	void ImportDescriptionData(TextAsset csv,List<DescriptionData> descriptions)
	{
		if(csv == null) return;

        List<List<string>> parser = fgCSVReader.LoadFromString(csv.text);

        descriptions.Clear();
        for (int i = 0; i < parser.Count; i++) {
			DescriptionData data = new DescriptionData();
			data.type = parser[i][0];
			
			float probability = 0.0f;
			float.TryParse(parser[i][1], out probability);
			data.probability = probability;
			
			data.text = parser[i][2];
			
			int group = 0;
			int.TryParse(parser[i][3], out group);
			data.group = group;
			
			int mimic = 0;
			int.TryParse(parser[i][4], out mimic);
			data.mimic = (mimic == 1);
			
			data.theme = (parser[i][5] == null ? "" : parser[i][5]);
			
			descriptions.Add(data);
		}
	}
	
	void ImportTypesData(TextAsset csv,List<NameData> names)
	{
		if(csv == null) return;
		
        List<List<string>> parser = fgCSVReader.LoadFromString(csv.text);
		
		names.Clear();
		
        for (int i = 0; i < parser.Count; i++) {
			NameData data = new NameData();
			
			data.type = parser[i][0];
			data.name = parser[i][1];
			data.price = parser[i][2];
			
			names.Add(data);
		}
    }

    void ImportListData(TextAsset csv,List<string> types,List<string> themes)
	{
		if(csv == null) return;
		
        List<List<string>> parser = fgCSVReader.LoadFromString(csv.text);
		
		types.Clear();
		themes.Clear();
		
        for (int i = 0; i < parser.Count; i++) {
            if (parser[i][0] != "")
				types.Add(parser[i][0]);
			if (parser[i][1] != "")
				themes.Add(parser[i][1]);
		}
	}
	
	void ImportTextTranslationData(TextAsset csv,List<TextTranslationData> translations)
	{
		if(csv == null) return;
		
        List<List<string>> parser = fgCSVReader.LoadFromString(csv.text);
		
		translations.Clear();

        for (int i = 0; i < parser.Count; i++) {
			TextTranslationData data = new TextTranslationData();
			
			data.label = parser[i][0];
			data.translation = parser[i][1];
			
			translations.Add(data);
		}
	}

	void ImportLabelData(TextAsset csv,List<string> labels)
	{
		if(csv == null) return;
		
        List<List<string>> parser = fgCSVReader.LoadFromString(csv.text);

        labels.Clear();
		
        for (int i = 0; i < parser.Count; i++) {
			labels.Add(parser[i][0]);
		}
	}
}
