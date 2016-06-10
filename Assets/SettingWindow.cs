using UnityEngine;
using System;
using System.IO;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class SettingWindow
{
	private Rect windowRect;
	private Datahandler tdh, cdh;
	private FileSelectWindow tfsw, cfsw;
	private DrawDatasetValues tddv;
	private Attributes[] tAtts, cAtts;
	private int k;
	private bool enabling, nextEnabled, drawTw, drawCw, completeT, completeC;
	private programStates pgState;
	private string locationT, locationC, sK;

	public SettingWindow(programStates pgState)
	{
		this.pgState = pgState;
		tfsw = new FileSelectWindow(); cfsw = new FileSelectWindow();
		locationT = "";locationC = ""; sK = "";
		k = 0;
		enabling = true;
		drawTw = drawCw = completeT = completeC = nextEnabled = false;
		cdh = null;
	}
	public void Draw()
	{
		GUI.enabled = enabling;
		Vector2 heightWidthDynamics = new Vector2(Screen.width, Screen.height);
		windowRect = new Rect((heightWidthDynamics.x - heightWidthDynamics.x)/2, (heightWidthDynamics.x - heightWidthDynamics.x)/2, heightWidthDynamics.x, heightWidthDynamics.y);
		windowRect = GUI.Window(0, windowRect, DoMyWindow, "");
		GUI.enabled = true;

		drawTw = tfsw.Draw(ref completeT, drawTw);
		drawCw = cfsw.Draw(ref completeC, drawCw);

	}
	private void DoMyWindow(int windowID)
	{
		switch(pgState)
		{
		case programStates.StartWindow:

			if(completeT)
			{
				drawTw = false;
				locationT = tfsw.getLocation; 
				completeT = false;
			}
			if(completeC)
			{
				drawCw = false;
				locationC = cfsw.getLocation; 
				completeC = false;
			}
			Rect groupRect = new Rect(windowRect.x + windowRect.width/2-20.0f, windowRect.y + windowRect.height/2 - 20.0f, windowRect.width/2+20.0f, 60.0f);
			GUI.BeginGroup(groupRect);
			locationT = GUI.TextArea(new Rect(-5.0f, 0.0f, groupRect.width - groupRect.width/7, 20.0f), locationT);
			if(GUI.Button(new Rect(groupRect.width - groupRect.width/7 - 5.0f, 0.0f, groupRect.width/7, 20.0f), "Get test set"))
			{
				drawTw = true;
				enabling = false;
			}
			locationC = GUI.TextArea(new Rect(-5.0f, 40.0f, groupRect.width - groupRect.width/7, 20.0f), locationC);
			if(GUI.Button(new Rect(groupRect.width - groupRect.width/7 - 5.0f, 40.0f, groupRect.width/7, 20.0f), "Classifier set"))
			{
				drawCw = true;
				enabling = false;
			}
			GUI.EndGroup();
			GUI.Label(new Rect(groupRect.x + groupRect.width/2 - 10.0f, groupRect.y + groupRect.height + 20.0f, 20.0f, 20.0f), "K");
			sK = k.ToString();
			sK = GUI.TextField(new Rect(groupRect.x + groupRect.width/2 - 10.0f, groupRect.y + groupRect.height + 40.0f, 20.0f, 20.0f), sK, 1);
			sK = Regex.Replace(sK, "[^a-zA-Z0-9 #!@]", "0");
			k = int.Parse(sK);

			if(locationT != "")
			{
				if(k > 0 && new FileInfo(locationT).Extension == ".csv")
				{
					nextEnabled = true;
				}
			}

			GUI.enabled = nextEnabled;
			if(GUI.Button(new Rect(windowRect.width - windowRect.width/8, windowRect.height - 20.0f, groupRect.width/8, 20.0f), "Next"))
			{
				tdh = new Datahandler(locationT);
				if(locationC != "" && (new FileInfo(locationC).Extension == ".csv"))
				{
					cdh = new Datahandler(locationC);
				}
				pgState = programStates.LoadSecond;
			}
			GUI.enabled = true;

			break;
		case programStates.LoadSecond:

			tddv = new DrawDatasetValues(new Vector2(windowRect.x + windowRect.width - windowRect.width/2 - 205.0f, windowRect.y + windowRect.height - windowRect.height/2 - 150.0f), tdh.getDataTable);
			drawTw = drawCw = completeT = completeC = nextEnabled = false;
			pgState = programStates.SecondWindow;

			break;
		case programStates.SecondWindow:

			tddv.Draw();
			if(tddv.theX != "" && tddv.theY != "" && tddv.theClassifier != "")
			{
				nextEnabled = true;
			}
			GUI.enabled = nextEnabled;
			if(GUI.Button(new Rect(windowRect.width - windowRect.width/8, windowRect.height - 20.0f, (windowRect.width/2)/8, 20.0f), "Next"))
			{
				pgState = programStates.LoadDraw;
			}
			GUI.enabled = true;

			break;
		case programStates.LoadDraw:

			DataTable tTable = tdh.getDataTable;
			DataManager tdm = new DataManager(tTable, tTable.Columns[tddv.theX], tTable.Columns[tddv.theY], tTable.Columns[tddv.theClassifier], Calculator.minmax(tTable, tTable.Columns[tddv.theX]), Calculator.minmax(tTable, tTable.Columns[tddv.theY]));
			ClassifierColor.instantiate(tTable, tTable.Columns[tddv.theClassifier]);
			Attributes[] tdmAtts, cdmAtts; 
			tdmAtts = tdm.CreateAttributes();
			if(cdh != null)
			{
				DataManager cdm = new DataManager(cdh.getDataTable, tTable.Columns[tddv.theX], tTable.Columns[tddv.theY], Calculator.minmax(tTable, tTable.Columns[tddv.theX]), Calculator.minmax(tTable, tTable.Columns[tddv.theY]));
				cdmAtts = cdm.CreateAttributes();
				foreach(Attributes cAtt in cdmAtts)
				{
					SpecialK Knearest = new SpecialK(tdmAtts, cAtt, k);
					cAtt.importK(Knearest.eucli, Knearest.cheby, Knearest.manhatt, true);
				}  
				cAtts = cdmAtts;
			}
			else
			{
				foreach(Attributes tAtt in tdmAtts)
				{
					SpecialK Knear = new SpecialK(tdmAtts, tAtt, k);
					tAtt.importK(Knear.eucli, Knear.cheby, Knear.manhatt, false);
				}
				tAtts = tdmAtts;
			}
			pgState = programStates.Diagram;

			break;
		}
	}
	public programStates getPState{get{return pgState;}}
	public Attributes[] getTestAttributes{get{return tAtts;}}
	public Attributes[] getClassAttributes{get{return cAtts;}}
}
