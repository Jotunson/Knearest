using UnityEngine;
using System;
using System.Linq;
using System.Data;
using System.Collections;
using System.Collections.Generic;

public class Calculator
{	
	private float eucli, cheby, manhatt;

	public Calculator(Vector2 one, Vector2 two)
	{
		eucli = EuclidaenDistance(one,two);
		cheby = ChebyshevDistance(one,two);
		manhatt = ManhattanDistance(one,two);
	}
	private float ManhattanDistance(Vector2 one, Vector2 two)
	{
		return Mathf.Abs(one.x - two.x) + Mathf.Abs(one.y - two.y);
	}
	private float ChebyshevDistance(Vector2 one, Vector2 two)
	{
		return Mathf.Max(Mathf.Abs(one.x - two.x), Mathf.Abs(one.y - two.y));
	}
	private float EuclidaenDistance(Vector2 one, Vector2 two)
	{
		return Mathf.Sqrt(Mathf.Pow(one.x - two.x, 2) + Mathf.Pow(one.y - two.y, 2));	
	}
	public float getEuclidaen{get{return eucli;}}
	public float getChebyshev{get{return cheby;}}
	public float getManhattan{get{return manhatt;}}

	public static Vector2 minmax(DataTable dt, DataColumn dc)
	{
		Vector2 minmax = new Vector2();
		foreach(DataRow dr in dt.Rows)
		{
			float i = 0;
			if(float.TryParse(dr[dc].ToString(), out i))
			{
				if(i > minmax.y)
				{
					minmax.y = i;
				}
				else if(i < minmax.x)
				{
					minmax.x = i;
				}
			}			
		}
		return minmax;
	}
	public static float toScreenConversion(float modifier, Vector2 minmax)
	{
		return (Screen.width*(modifier-minmax.x)/(minmax.y-minmax.x));
	}
}
