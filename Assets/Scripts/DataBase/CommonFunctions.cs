using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public static class CommonFunctions
{
	// 
	public static bool IsEqual(float a, float b, float e = 0.001f)
	{
		return (a >= b - e && a <= b + e) ? true : false;
	}
	
	//
	public static bool IsMatchId(string data,string id)
	{
		if(string.IsNullOrEmpty(data)) return true;
		
		string[] check_array = data.Split(',');
		foreach(string wildcard in check_array)
		{
			if(IsMatchWildcard(wildcard.Trim(),id)) return true;
		}
		
		return false;
	}
	
	//
	public static bool IsMatchWildcard(string wildcard,string id)
	{
		Regex regex = new Regex(WildcardToRegex(wildcard));
		return regex.IsMatch(id);
	}
	
	//
	public static string WildcardToRegex(string wildcard)
	{
		return "^" + Regex.Escape(wildcard).Replace("\\*",".*").Replace("\\?",".") + "$";
	}
}