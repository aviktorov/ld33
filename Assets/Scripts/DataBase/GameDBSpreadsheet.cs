using UnityEngine;

using Google.GData.Client;
using Google.GData.Spreadsheets;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

//
public class InsecureSecurityCertificatePolicy {
	public static bool Validator(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyErrors) {
		return true;
	}
	
	public static void Instate() {
		ServicePointManager.ServerCertificateValidationCallback = Validator;
	}
}

[System.Serializable]
public class GameDBSpreadsheet : GameDB {
	public string key = "1soI0_D12vyDzL8AZZSs71nAhPneK09uQw_Qz5JpIscU";
	
	//
	public void Import() {
		InsecureSecurityCertificatePolicy.Instate();
		
		SpreadsheetsService service = new SpreadsheetsService("UnityConnect");
		WorksheetQuery query = new WorksheetQuery("https://spreadsheets.google.com/feeds/worksheets/" + key + "/public/values");
		WorksheetFeed feed = service.Query(query);
		
		ImportDescriptionsData(service,(WorksheetEntry)feed.Entries[0], descriptions);
		ImportTypesData(service,(WorksheetEntry)feed.Entries[1], names);
	}
	
	private void ImportDescriptionsData(SpreadsheetsService service, WorksheetEntry sheet, List<DescriptionData> entities) {
		if(sheet == null) return;
		AtomLink cellLink = sheet.Links.FindService(GDataSpreadsheetsNameTable.CellRel,null);
		
		CellQuery query = new CellQuery(cellLink.HRef.ToString());
		CellFeed feed = service.Query(query);
		
		entities.Clear();
		foreach(CellEntry cell in feed.Entries) {
			// skip header
			if(cell.Row == 1) continue;
			
			uint index = cell.Row - 2;
			if(entities.Count <= index) {
				entities.Add(new DescriptionData());
			}
			
			DescriptionData data = entities.ToArray()[index];
			if(cell.Column == 1) data.type = cell.Value;
			if(cell.Column == 2) data.price = cell.Value;
			if(cell.Column == 3) data.probability = cell.Value;
			if(cell.Column == 4) data.text = cell.Value;
			if(cell.Column == 5) data.group = cell.Value;
			if(cell.Column == 6) data.mimic = cell.Value;
		}
	}
	
	private void ImportTypesData(SpreadsheetsService service, WorksheetEntry sheet, List<NameData> entities) {
		if(sheet == null) return;
		AtomLink cellLink = sheet.Links.FindService(GDataSpreadsheetsNameTable.CellRel,null);
		
		CellQuery query = new CellQuery(cellLink.HRef.ToString());
		CellFeed cells = service.Query(query);
		
		entities.Clear();
		foreach(CellEntry cell in cells.Entries) {
			// skip header
			if(cell.Row == 1) continue;
			
			uint index = cell.Row - 2;
			if(entities.Count <= index) {
				entities.Add(new NameData());
			}
			
			NameData data = entities.ToArray()[index];
			if(cell.Column == 1) data.name = cell.Value;
			if(cell.Column == 2) data.type = cell.Value;
		}
	}
}
