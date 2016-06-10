using UnityEngine;
using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;

public class DrawDatasetValues
{
	private DataTable dt;
	private Rect scrollRect, totalRect;
	private MultipleChoiceList mcLists, clLists;
	private Vector2 scrollPos = new Vector2(0,0);
	private string x, y, classifier, tableName;
	private List<string> mcValue = new List<string>();
	private List<string> clValue = new List<string>();

	public DrawDatasetValues(Vector2 pos, DataTable inSet)
	{
		this.dt = inSet;
		x = y = classifier = "";
		this.tableName = inSet.TableName;
		List<string> tStringlist = new List<string>();
		List<string> tClassList = new List<string>();
		foreach(DataColumn dc in dt.Columns)
		{
			if(hasValue(dt, dc))
			{
				if(!isClassifier(dt, dc))
				{
					tClassList.Add(dc.ColumnName);
				}
				else
				{
					tStringlist.Add(dc.ColumnName);
				}
			}
		}
		if(tStringlist.Count > 0)
		{
			mcLists = new MultipleChoiceList(new Rect(pos.x+5.0f, pos.y+5.0f, 205.0f, 250.0f), tStringlist.ToArray(), 2, dt.TableName); 
		}
		if(tClassList.Count > 0)
		{
			clLists = new MultipleChoiceList(new Rect(pos.x + mcLists.posGetter.width + 15.0f, pos.y+5.0f, 205.0f, 250.0f), tClassList.ToArray(), 1, dt.TableName);
		}
		
		this.scrollRect = new Rect(pos.x, pos.y, (mcLists.posGetter.width*2)+20.0f, (clLists.posGetter.y + clLists.posGetter.height)+(clLists.posGetter.y + clLists.posGetter.height)/2);
		this.totalRect = new Rect(pos.x, pos.y, (mcLists.posGetter.width*2)+20.0f, clLists.posGetter.height);
	}
	public void Draw()
	{
		GUI.Box(new Rect(scrollRect.x, scrollRect.y, scrollRect.width, scrollRect.height + 15.0f), "");
		GUI.BeginScrollView(scrollRect, scrollPos, totalRect);

		mcValue = mcLists.Draw(mcValue);
		clValue = clLists.Draw(clValue);
		string[] mcVal = mcValue.ToArray();
		x = mcVal.Length > 0 ? mcVal[0] : "";
		y = mcVal.Length > 1 ? mcVal[1] : "";
		classifier = clValue.Count > 0 ? clValue.ToArray()[0] : "";
		GUI.EndScrollView();

		GUI.BeginGroup(new Rect(scrollRect.x, scrollRect.y + scrollRect.height- 35.0f, scrollRect.width, 45.0f)); 
		GUI.Label(new Rect(5.0f, 2.5f, 100.0f, 20.0f), "X");
		GUI.Label(new Rect(110.0f, 2.5f, 100.0f, 20.0f), "Y");
		GUI.Label(new Rect(220.0f, 2.5f, 100.0f, 20.0f), "Classifier");
		GUI.Label(new Rect(325.0f, 2.5f, 100.0f, 20.0f), "Table Name");

		x = GUI.TextArea(new Rect(5.0f, 25.0f, 100.0f, 20.0f), x);
		y = GUI.TextArea(new Rect(110.0f, 25.0f, 100.0f, 20.0f), y);
		classifier = GUI.TextArea(new Rect(220.0f, 25.0f, 100.0f, 20.0f), classifier);
		tableName = GUI.TextArea(new Rect(325.0f, 25.0f, 100.0f, 20.0f), tableName);
		GUI.EndGroup();
	}
	private bool isClassifier(DataTable dt, DataColumn dc)
	{
		double min, max;
		min = max = 0.0f;
		bool numeric = false;
		foreach(DataRow dr in dt.Rows)
		{
			int i = 0;
			if(IsNumeric(dr[dc].ToString(), ref i))
			{
				numeric = true;
				if(i > max)
				{
					max = i;
				}
				else if(i < min)
				{
					min = i;
				}
			}
		}
		if(numeric)
		{
			return (max - min > 2 ? true : false);
		}
		return false;
	}
	private bool hasValue(DataTable dt, DataColumn dc)
	{
		int count = 0;

		foreach(DataRow dr in dt.Rows)
		{
			if(!string.IsNullOrEmpty(dr[dc].ToString()))
				continue;

				count++;
		}

		if(count + 5 >= dt.Rows.Count)
		{
			return false;
		}
		return true;
	}
	private bool IsNumeric(string Expression, ref int i)
	{
		if(int.TryParse(Expression, out i))
		{
			return true;
		}
		return false;
	}
	public string theX{get{return x;}}
	public string theY{get{return y;}}
	public string theClassifier{get{return classifier;}}
	public string theTable{get{return mcLists.getTableName;}}
}
