using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FileList
{
	private int selectedItemIndex = -1;
	private Vector2 scrollPos = new Vector2(0,0);
	private GUIStyle listStyle = new GUIStyle();
	private Rect rect, listRect, scrollViewRect;
	private string thisName;

	public FileList(string thisName)
	{
		this.thisName = thisName;
		listStyle.normal.textColor = Color.white; 
		listStyle.padding.left = listStyle.padding.right = listStyle.padding.top = listStyle.padding.bottom = 4;
	}
	public string Draw(string[] listContentIn, string currentSelection, Rect rect)
	{
		GUI.Box(new Rect(rect.x, rect.y, rect.width, rect.height), thisName);
		this.rect = rect;
		if(listContentIn.Length > 0)
		{
			GUIContent[] listContent = Array.ConvertAll(listContentIn, item => new GUIContent(item));
			bool choice = false;
			int controlID = GUIUtility.GetControlID(FocusType.Passive);
			listRect = new Rect(rect.x, rect.y+20, rect.width-16, listStyle.CalcHeight(listContent[0], 1.0f)*listContent.Length);
			scrollViewRect = new Rect(listRect.x, listRect.y, listRect.width+16, rect.height - listStyle.CalcHeight(listContent[0], 1.0f));

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

			int newSelectedItemIndex = 0;


			scrollPos = GUI.BeginScrollView(scrollViewRect, scrollPos, listRect);
			newSelectedItemIndex = GUI.SelectionGrid(listRect, selectedItemIndex, listContent, 1, listStyle);
			GUI.EndScrollView ();

			if(choice && newSelectedItemIndex != selectedItemIndex)
			{
				selectedItemIndex = newSelectedItemIndex;
				choice = false;
				return listContent[selectedItemIndex].text;
			}
		}
		return currentSelection;
	}
	public Rect getRect
	{
		get{return rect;}
	}
}
