using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Attributes
{
	private Color classifierColor;
	private bool hasClassifier;
	private string strVal;
	private float thisX, thisY;
	private Circle thisPoint, KECircle, KCCircle, KMCircle;
	private List<KeyValuePair<float, Attributes>> Euclidaen, Chebyshev, Manhattan;
	private Vector2 xMinMax, yMinMax;

	public Attributes(float X, float Y, Vector2 xMinMax, Vector2 yMinMax)
	{
		classifierColor = Color.black;
		hasClassifier = true;
		setValues(X, Y, xMinMax, yMinMax);
	}
	public Attributes(string Value, float X, float Y, Vector2 xMinMax, Vector2 yMinMax)
	{
		strVal = Value;
		classifierColor = ClassifierColor.instantiate().getColor(strVal);
		hasClassifier = false;
		setValues(X, Y, xMinMax, yMinMax);
	}
	private void setValues(float X, float Y, Vector2 xMinMax, Vector2 yMinMax)
	{
		NeighborDirection(new Vector2(X, Y -50));
		thisX = X;
		thisY = Y;
		this.xMinMax = xMinMax;
		this.yMinMax = yMinMax;
		thisPoint = new Circle(new Vector2(Calculator.toScreenConversion(X, xMinMax),Calculator.toScreenConversion(Y, yMinMax)), 5.0f);
		KECircle = KCCircle = KMCircle = null;
	}
	public void importK(List<KeyValuePair<float, Attributes>> e, List<KeyValuePair<float, Attributes>> c, List<KeyValuePair<float, Attributes>> m, bool classify)
	{
		this.Euclidaen = e;
		this.Chebyshev = c;
		this.Manhattan = m;
		KECircle = new Circle(new Vector2(Calculator.toScreenConversion(thisX, xMinMax),Calculator.toScreenConversion(thisY, yMinMax)), Calculator.toScreenConversion(e[e.Count - 1].Key, yMinMax)); 
		KCCircle = new Circle(new Vector2(Calculator.toScreenConversion(thisX, xMinMax),Calculator.toScreenConversion(thisY, yMinMax)), Calculator.toScreenConversion(c[c.Count - 1].Key, yMinMax));
		KMCircle = new Circle(new Vector2(Calculator.toScreenConversion(thisX, xMinMax),Calculator.toScreenConversion(thisY, yMinMax)), Calculator.toScreenConversion(m[m.Count - 1].Key, yMinMax));
		if(classify)
		{
			string ct = setClassifier(distanceType.Eucli);
			classifierColor = ClassifierColor.instantiate().getColor(ct);
			strVal = ct;
		}
		Debug.Log(strVal + " " + classifierColor);
	}
	public void draw(distanceType dType)
	{
		thisPoint.DrawCircle(classifierColor);

		switch(dType)
		{
		case distanceType.Eucli:
			KECircle.DrawCircle(Color.red);
			break;
		case distanceType.Cheby:
			KCCircle.DrawCircle(Color.red);
			break;
		case distanceType.Manhatt:
			KMCircle.DrawCircle(Color.red);
			break;
		default:
			break;
		}
	}
	public string setClassifier(distanceType dt)
	{
		string newClassification = "";
		switch(dt)
		{
		case distanceType.Eucli:
			newClassification = thisNewClass(Euclidaen);
			break;
		case distanceType.Cheby:
			newClassification = thisNewClass(Chebyshev);
			break;
		case distanceType.Manhatt:
			newClassification = thisNewClass(Manhattan);
			break;
		}
		return newClassification;
	}
	private string thisNewClass(List<KeyValuePair<float, Attributes>> inp)
	{
		List<string> inL = new List<string>();
		List<KeyValuePair<int, string>> occurenceCount = new List<KeyValuePair<int, string>>();
		foreach(KeyValuePair<float, Attributes> i in inp)
		{
			inL.Add(i.Value.getClassifier);
		}
		foreach(string s in inL)
		{
			occurenceCount.Add(new KeyValuePair<int, string>(inL.Count(f => f == s), s));
		}

		return ((List<KeyValuePair<int, string>>)occurenceCount.OrderByDescending(o => o.Key))[0].Value;
	}
	private void NeighborDirection(Vector2 neighhbor)
	{
		float t = xy.x * neighhbor.x + xy.y * neighhbor.y;
		Debug.Log(t);
			//return null;
		/*Vector2 heading = neighhbor - xy;
		float distance = heading.magnitude;
		Vector2 direction = heading / distance;*/
	}
	public Vector2 xy{get{return new Vector2(thisX, thisY);}}
	public bool isClassified{get{return hasClassifier;}}
	public Circle dataPoint{get{return thisPoint;}}
	public string getClassifier{get{return strVal;}}
	public List<KeyValuePair<float, Attributes>> eucli{get{return Euclidaen;}}
	public List<KeyValuePair<float, Attributes>> cheby{get{return Chebyshev;}}
	public List<KeyValuePair<float, Attributes>> manhatt{get{return Manhattan;}}
}
