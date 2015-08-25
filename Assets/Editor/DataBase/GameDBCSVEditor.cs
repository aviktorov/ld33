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
			EditorGUILayout.LabelField(string.Format("Raports: {0}", data.raports.Count));
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.BeginVertical("box");
			EditorGUILayout.LabelField("CSV Data", EditorStyles.boldLabel);
			EditorGUILayout.Space();
			data.descriptionData = (TextAsset)EditorGUILayout.ObjectField("Description",data.descriptionData,typeof(TextAsset),false);
			data.nameData = (TextAsset)EditorGUILayout.ObjectField("Names",data.nameData,typeof(TextAsset),false);
			data.themeData = (TextAsset)EditorGUILayout.ObjectField("Themes",data.themeData,typeof(TextAsset),false);
			data.raportData = (TextAsset)EditorGUILayout.ObjectField("Raports",data.raportData,typeof(TextAsset),false);
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
					data.raports.Clear();
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
		ImportRaportData(data.raportData,data.raports);
		/*
		foreach(DescriptionData desc in data.descriptions) {
			Debug.Log(string.Format("Desc: {0}, {1}, {2}, {3}, {4}, {5}",desc.type,desc.probability,desc.text,desc.group,desc.mimic,desc.theme));
		}
		
		foreach(NameData n in data.names) {
			Debug.Log(string.Format("Name: {0}, {1}, {2}",n.type,n.name,n.price));
		}
		
		foreach(string type in data.types) {
			Debug.Log(string.Format("Type: {0}",type));
		}
		
		foreach(string theme in data.themes) {
			Debug.Log(string.Format("Theme: {0}",theme));
		}
		
		foreach(string raport in data.raports) {
			Debug.Log(string.Format("Raport: {0}",raport));
		}
		/**/
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
			types.Add(parser[0]);
			themes.Add(parser[1]);
		}
	}
	
	void ImportRaportData(TextAsset csv,List<string> raports)
	{
		if(csv == null) return;
		
		CsvReader parser = new CsvReader(new StringReader(csv.text),true);
		
		raports.Clear();
		
		while (parser.ReadNextRecord())
		{
			raports.Add(parser[1]);
		}
	}
}
