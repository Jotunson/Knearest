using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class FileSelectWindow
{
	private Rect windowRect;
	private FileList DirectoryList, FileList;
	private bool complete, draw, filter;
	private DirectoryInfo directoryInfo, directorySelection;
	private FileInfo fileSelection;
	private string location, previousLocation, previousFile;

	public FileSelectWindow()
	{
		complete = filter = false;
		draw = false;
		location = previousLocation = Application.dataPath;
		directorySelection = new DirectoryInfo(location);
		DirectoryList = new FileList("Directories");
		FileList = new FileList("Files");
	}
	public bool Draw(ref bool complete, bool draw)
	{
		if(draw)
		{
			Vector2 heightWidthDynamics = new Vector2(Screen.width, Screen.height);
			windowRect = new Rect((heightWidthDynamics.x - heightWidthDynamics.x)/2, (heightWidthDynamics.x - heightWidthDynamics.x)/2, heightWidthDynamics.x, heightWidthDynamics.y);
			windowRect = GUI.Window(1, windowRect, DoMyWindow, "Find File");
		}
		this.draw = draw;

		complete = this.complete;

		return draw;
	}
	private void DoMyWindow(int windowID) 
	{
		if(draw)
		{
			complete = false;
		}

		fileSelection = new FileInfo(location);
		if(((fileSelection.Attributes & FileAttributes.Directory) == FileAttributes.Directory) && getAccess(new DirectoryInfo(location)))
		{
			directoryInfo = new DirectoryInfo(location);
		}

		GUI.Box(new Rect(10.0f, DirectoryList.getRect.y - 21.0f, windowRect.y + windowRect.width - 20.0f, 20.0f), "");
		if(GUI.Button(new Rect(10.0f, DirectoryList.getRect.y - 21.0f, (windowRect.y + windowRect.width)/2 - 10.0f, 20.0f), "To Parent Directory") && location != directoryInfo.Root.FullName)
		{
			directoryInfo = directoryInfo.Parent;
			location = directoryInfo.FullName;
		}

		directorySelection = new DirectoryInfo(DirectoryList.Draw(Array.ConvertAll(directoryInfo.GetDirectories(), item => (string)item.FullName), directorySelection.FullName, new Rect(10.0f, windowRect.y + 40.0f, (windowRect.width)/2 -10.0f, (windowRect.height) - 70.0f)));
		if(directorySelection.FullName != previousLocation)
		{
			location = directorySelection.FullName;
			previousLocation = location;
		}

		fileSelection = new FileInfo(FileList.Draw(Array.ConvertAll((filter ? filterCsvFiles(directoryInfo.GetFiles()) : directoryInfo.GetFiles()), item => (string)item.Name), fileSelection.FullName, new Rect((windowRect.width)/2, windowRect.y + 40.0f, (windowRect.width)/2 -10.0f, (windowRect.height) - 70.0f)));
		if(fileSelection.FullName != previousFile)
		{
			location = directoryInfo.FullName + "\\" + fileSelection.Name;
			previousFile = location;
		}
		GUI.BeginGroup(new Rect(10.0f, DirectoryList.getRect.y + DirectoryList.getRect.height, windowRect.y + windowRect.width -20.0f, 21.0f));
		location = GUI.TextArea(new Rect(0.0f, 1.0f, windowRect.y + windowRect.width -70.0f, 20.0f), location);
		if(GUI.Button(new Rect(windowRect.y + windowRect.width -70.0f, 1.0f, 50.0f, 20.0f), "Get"))
		{
			draw = filter = false;
			complete = true;
		}
		GUI.EndGroup();

		if(GUI.Button(new Rect(windowRect.y + windowRect.width - 35.0f, DirectoryList.getRect.y - 21.0f, 25.0f, 20.0f), "X"))
		{
			draw = filter = false;
			//complete = true;
		}
		filter = GUI.Toggle(new Rect((windowRect.y + windowRect.width)/2 + 1.0f, DirectoryList.getRect.y - 21.0f, 110.0f, 20.0f), filter, "Csv File Filter");
	}
	public string getLocation
	{
		get{return location;}
	}
	private bool getAccess(DirectoryInfo dirInf)
	{
		if(dirInf.Exists)
		{
			try
			{ 
				dirInf.GetFiles(); 
				return true; 
			} 
			catch (UnauthorizedAccessException)
			{ 
				return false;
			}
		}
		return false;
	}
	private FileInfo[] filterCsvFiles(FileInfo[] fileInfo)
	{
		List<FileInfo> fInf = new List<FileInfo>();
		foreach(FileInfo f in fileInfo)
		{
			if(f.Extension == ".csv")
			{
				fInf.Add(f);
			}
		}
		return fInf.ToArray();
	}
}
