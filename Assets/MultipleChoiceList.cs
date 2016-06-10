using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MultipleChoiceList
{
	private int selectedItemIndex = 0;
	private Rect rect;
	private GUIContent[] listContent;
	private Vector2 scrollPos = new Vector2(0,0);
	private GUIStyle listStyle = new GUIStyle();
	private bool stopAdding = false;
	private int maxChoices;
	private Rect listRect;
	private Rect scrollViewRect;
	private string tName;

	public MultipleChoiceList(Rect rect, string[] listContent, int maxChoices, string listtName)
	{
		this.tName = listtName;
		instantiate(rect, maxChoices);
		this.listContent = Array.ConvertAll(listContent, item => new GUIContent(item));
	}
	public List<string> Draw(List<string> resultList)
	{
		bool choice = false;
		int controlID = GUIUtility.GetControlID(FocusType.Passive);
		listRect = new Rect(rect.x, rect.y, rect.width-16, listStyle.CalcHeight(listContent[0], 1.0f) * listContent.Length+10);
		scrollViewRect = new Rect(listRect.x, listRect.y, listRect.width+16, listStyle.CalcHeight(listContent[0], 1.0f) * 10);
		
		switch(Event.current.GetTypeForControl(controlID))
		{
			case EventType.mouseUp:
			{
				if(listRect.Contains(Event.current.mousePosition))
				{
					choice = true;
				}
			}
			break;
		} 
		
		int newSelectedItemIndex;

		GUI.Box(new Rect(scrollViewRect.x, rect.y, rect.width, scrollViewRect.height), "");
		scrollPos = GUI.BeginScrollView(scrollViewRect, scrollPos, listRect);
		newSelectedItemIndex = GUI.SelectionGrid(listRect, selectedItemIndex, listContent, 1, listStyle);
		if(newSelectedItemIndex != selectedItemIndex)
		{
			selectedItemIndex = newSelectedItemIndex;
			choice = true;
		}
		GUI.EndScrollView ();
		
		if(choice)
		{
			int i = 0;
			bool match = false;
			foreach(string r in resultList)
			{
				if(r == listContent[selectedItemIndex].text)
				{
					match = true;
					resultList.RemoveAt(i);
					break;
				}
				i++;
			}
			if(!match && !stopAdding && (maxChoices == 0 || resultList.Count < maxChoices))
			{
				resultList.Add(listContent[selectedItemIndex].text);
			}
		}
		
		return resultList;
	}
	private void instantiate(Rect rect, int maxChoice)
	{
		this.maxChoices = maxChoice;
		this.rect = rect;
		listStyle.normal.textColor = Color.white; 
		listStyle.padding.left =
			listStyle.padding.right =
				listStyle.padding.top =
				listStyle.padding.bottom = 4;
	}
	public Rect posGetter
	{
		get
		{
			return new Rect(scrollViewRect.x, rect.y, rect.width, scrollViewRect.height+20.0f);
		}
	}
	public string getTableName
	{
		get
		{
			return tName;
		}
	}
}
