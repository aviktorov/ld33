using UnityEngine;

using Google.GData.Client;
using Google.GData.Spreadsheets;

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public class InsecureSecurityCertificatePolicy {
	public static bool Validator(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyErrors) {
		return true;
	}
	
	public static void Instate() {
		ServicePointManager.ServerCertificateValidationCallback = Validator;
	}
}

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
public class GameDB : ScriptableObject {
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

	public string key = "1soI0_D12vyDzL8AZZSs71nAhPneK09uQw_Qz5JpIscU";
	
	public bool Import() {
		InsecureSecurityCertificatePolicy.Instate();
		
		SpreadsheetsService service;
		WorksheetQuery query;
		WorksheetFeed feed;
		try {
			service = new SpreadsheetsService("UnityConnect");
			query = new WorksheetQuery("https://spreadsheets.google.com/feeds/worksheets/" + key + "/public/values");
			feed = service.Query(query);
		}
		catch {
			Debug.LogError("Explosion in imort spreadsheets");
			return false;
		}
		
		ImportDescriptionsData(service, (WorksheetEntry)feed.Entries[0], descriptions);
		ImportTypesData(service, (WorksheetEntry)feed.Entries[1], names);
		ImportLists(service, (WorksheetEntry)feed.Entries[2], types, themes);
		ImportRaports(service, (WorksheetEntry)feed.Entries[3], raports);
		
		return true;
	}
	
	private void ImportDescriptionsData(SpreadsheetsService service, WorksheetEntry sheet, List<DescriptionData> descriptions) {
		if(sheet == null) return;
		AtomLink cellLink = sheet.Links.FindService(GDataSpreadsheetsNameTable.CellRel,null);
		
		CellQuery query = new CellQuery(cellLink.HRef.ToString());
		CellFeed feed = service.Query(query);
		
		descriptions.Clear();
		foreach(CellEntry cell in feed.Entries) {
			// skip header
			if(cell.Row == 1) continue;
			
			uint index = cell.Row - 2;
			if(descriptions.Count <= index) {
				descriptions.Add(new DescriptionData());
			}
			
			DescriptionData data = descriptions.ToArray()[index];
			if(cell.Column == 1) data.type = cell.Value;
			if(cell.Column == 2) {
				float probability = 0.0f;
				float.TryParse(cell.Value, out probability);
				data.probability = probability;
			}
			if(cell.Column == 3) data.text = cell.Value;
			if(cell.Column == 4) {
				int group = 0;
				int.TryParse(cell.Value, out group);
				data.group = group;
			}
			if(cell.Column == 5) {
				int mimic = 0;
				int.TryParse(cell.Value, out mimic);
				data.mimic = mimic == 1;
			}
			if(cell.Column == 6) data.theme = (cell.Value == null ? "" : cell.Value);
			data.theme = (data.theme == null ? "" : data.theme);
		}
	}
	
	private void ImportTypesData(SpreadsheetsService service, WorksheetEntry sheet, List<NameData> names) {
		if(sheet == null) return;
		AtomLink cellLink = sheet.Links.FindService(GDataSpreadsheetsNameTable.CellRel,null);
		
		CellQuery query = new CellQuery(cellLink.HRef.ToString());
		CellFeed cells = service.Query(query);
		
		names.Clear();
		foreach(CellEntry cell in cells.Entries) {
			// skip header
			if(cell.Row == 1) continue;
			
			uint index = cell.Row - 2;
			if(names.Count <= index) {
				names.Add(new NameData());
			}
			
			NameData data = names.ToArray()[index];
			if(cell.Column == 1) data.type = cell.Value;
			if(cell.Column == 2) data.name = cell.Value;
			if(cell.Column == 3) data.price = cell.Value;
		}
	}

	private void ImportLists(SpreadsheetsService service, WorksheetEntry sheet, List<string> types, List<string> themes) {
		if(sheet == null) return;
		AtomLink cellLink = sheet.Links.FindService(GDataSpreadsheetsNameTable.CellRel,null);
		
		CellQuery query = new CellQuery(cellLink.HRef.ToString());
		CellFeed cells = service.Query(query);
		
		types.Clear();
		themes.Clear();
		foreach(CellEntry cell in cells.Entries) {
			// skip header
			if(cell.Row == 1) continue;
			
			if(cell.Column == 1) types.Add(cell.Value);
			if(cell.Column == 2) themes.Add(cell.Value);
		}
	}

	private void ImportRaports(SpreadsheetsService service, WorksheetEntry sheet, List<string> raports) {
		if(sheet == null) return;
		AtomLink cellLink = sheet.Links.FindService(GDataSpreadsheetsNameTable.CellRel,null);
		
		CellQuery query = new CellQuery(cellLink.HRef.ToString());
		CellFeed cells = service.Query(query);
		
		raports.Clear();
		foreach(CellEntry cell in cells.Entries) {
			// skip header
			if(cell.Row == 1) continue;
			
			if(cell.Column == 2) raports.Add(cell.Value);
		}
	}

}
