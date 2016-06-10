using UnityEngine;
using System.Data;
using System.Collections;
using System.Collections.Generic;

public class ClassifierColor 
{
	private static ClassifierColor instance = null;
	private static Dictionary<string, Color> SC = new Dictionary<string, Color>();

	private ClassifierColor(DataTable dt, DataColumn dc)
	{
		foreach(DataRow dr in dt.Rows)
		{
			if(!SC.ContainsKey(dr[dc].ToString()))
			{
				bool gotColor = false;
				while(!gotColor)
				{
					Color c = new Color(Random.value, Random.value, Random.value, 1.0f);
					if(!SC.ContainsValue(c) && c != Color.black && c != Color.red)
					{
						SC.Add(dr[dc].ToString(), c);
						gotColor = true;
					}
				}
			}
		}
	}
	public static ClassifierColor instantiate(DataTable dt, DataColumn dc)
	{
		if(instance == null)
		{
			instance = new ClassifierColor(dt, dc);
		}
		return instance;
	}
	public static ClassifierColor instantiate()
	{
		return instance;
	}
	public Color getColor(string search)
	{
		Color c = Color.black;
		if(SC.TryGetValue(search, out c))
		{
			return c;
		}
		return c;
	}
}
