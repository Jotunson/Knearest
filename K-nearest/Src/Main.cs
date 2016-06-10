using UnityEngine;
using System.IO;
using System.Collections;

public class Main : MonoBehaviour 
{
	private	SettingWindow sw;
	private programStates pg;

	// Use this for initialization
	void Start () {
		pg = programStates.StartWindow;
		sw = new SettingWindow(pg);
	}

	// Update is called once per frame
	void Update () {
		pg = sw.getPState;
	}

	void OnGUI()
	{
		if(pg != programStates.Diagram)
		{
			sw.Draw();
		}
		if(pg == programStates.Diagram)
		{
			foreach(Attributes att in sw.getTestAttributes)
			{
				att.draw(distanceType.Eucli);
			}
		}
	
	}
	/*void OnPostRender() 
	{
		if(pg == programStates.Diagram)
		{
			foreach(Attributes att in sw.getTestAttributes)
			{
				att.draw(distanceType.Eucli);
			}
		}
	}*/
}
public enum programStates{StartWindow, LoadSecond, SecondWindow, LoadDraw, Diagram};
public enum distanceType{Eucli, Cheby, Manhatt};
