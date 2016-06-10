using UnityEngine;
using System;
using System.Data;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class DataManager
{
	private DataTable dt;
	private DataColumn x, y, classifier;
	private bool testAttributes;
	private Vector2 xMinMax, yMinMax;

	public DataManager(DataTable dt, DataColumn x, DataColumn y, DataColumn classifier, Vector2 xMinMax, Vector2 yMinMax)
	{
		this.dt = dt;
		this.x = x;
		this.y = y;
		this.classifier = classifier;
		this.testAttributes = true;
		this.xMinMax = xMinMax;
		this.yMinMax = yMinMax;
	}
	public DataManager(DataTable dt, DataColumn x, DataColumn y, Vector2 xMinMax, Vector2 yMinMax)
	{
		this.dt = dt;
		this.x = x;
		this.y = y;
		this.testAttributes = false;
		this.xMinMax = xMinMax;
		this.yMinMax = yMinMax;
	}
	public Attributes[] CreateAttributes()
	{
		List<Attributes> tempAtt = new List<Attributes>();

		foreach(DataRow dr in dt.Rows)
		{
			float setX, setY;
			if(testAttributes)
			{
				if(dr[classifier] != null && dr[x] != null && dr[y] != null)
				{
					if(float.TryParse(dr[x].ToString(), out setX) && float.TryParse(dr[y].ToString(), out setY))
					{
						tempAtt.Add(new Attributes(dr[classifier].ToString(), setX, setY,xMinMax, yMinMax));
					}
				}
			}
			else
			{
				if(dr[x] != null && dr[y] != null)
				{
					if(float.TryParse(dr[x].ToString(), out setX) && float.TryParse(dr[y].ToString(), out setY))
					{
						tempAtt.Add(new Attributes(setX, setY, xMinMax, yMinMax));
					}
				}
			}
		}

		return tempAtt.ToArray();
	}
}
