using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LumenWorks.Framework.IO.Csv;

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
		
		CsvReader parser = new CsvReader(new StringReader(csv.text),true);
		
		descriptions.Clear();
		
		while (parser.ReadNextRecord())
		{
			DescriptionData data = new DescriptionData();
			data.type = parser[0];
			
			float probability = 0.0f;
			float.TryParse(parser[1], out probability);
			data.probability = probability;
			
			data.text = parser[2];
			
			int group = 0;
			int.TryParse(parser[3], out group);
			data.group = group;
			
			int mimic = 0;
			int.TryParse(parser[4], out mimic);
			data.mimic = (mimic == 1);
			
			data.theme = (parser[5] == null ? "" : parser[5]);
			
			descriptions.Add(data);
		}
	}
	
	void ImportTypesData(TextAsset csv,List<NameData> names)
	{
		if(csv == null) return;
		
		CsvReader parser = new CsvReader(new StringReader(csv.text),true);
		
		names.Clear();
		
		while (parser.ReadNextRecord())
		{
			NameData data = new NameData();
			
			data.type = parser[0];
			data.name = parser[1];
			data.price = parser[2];
			
			names.Add(data);
		}
	}
	
	void ImportListData(TextAsset csv,List<string> types,List<string> themes)
	{
		if(csv == null) return;
		
		CsvReader parser = new CsvReader(new StringReader(csv.text),true);
		
		types.Clear();
		themes.Clear();
		
		while (parser.ReadNextRecord())
		{
			if (parser[0] != "")
				types.Add(parser[0]);
			if (parser[1] != "")
				themes.Add(parser[1]);
		}
	}
	
	void ImportTextTranslationData(TextAsset csv,List<TextTranslationData> translations)
	{
		if(csv == null) return;
		
		CsvReader parser = new CsvReader(new StringReader(csv.text),true);
		
		translations.Clear();

		while (parser.ReadNextRecord())
		{
			TextTranslationData data = new TextTranslationData();
			
			data.label = parser[0];
			data.translation = parser[1];
			
			translations.Add(data);
		}
	}

	void ImportLabelData(TextAsset csv,List<string> labels)
	{
		if(csv == null) return;
		
		CsvReader parser = new CsvReader(new StringReader(csv.text),true);
		
		labels.Clear();
		
		while (parser.ReadNextRecord())
		{
			labels.Add(parser[0]);
		}
	}
}
