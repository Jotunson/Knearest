using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class SpecialK 
{
	private Vector2 originPoint;
	private List<KeyValuePair<float, Attributes>> Euclidaen, Chebyshev, Manhattan;

	public SpecialK(Attributes[] testAttributes, Attributes tuner, int k)
	{
		Euclidaen = Chebyshev = Manhattan = new List<KeyValuePair<float, Attributes>>();
		FindNearestNeighbors(testAttributes, tuner, k);
	}
	private void FindNearestNeighbors(Attributes[] testAttributes, Attributes tuner, int k)
	{
		foreach(Attributes trainingInstance in testAttributes)
		{
			if(trainingInstance.xy != tuner.xy)
			{
				Calculator tCalc = new Calculator(tuner.xy, trainingInstance.xy);
				Euclidaen.Add(new KeyValuePair<float, Attributes>(tCalc.getEuclidaen, trainingInstance));
				Chebyshev.Add(new KeyValuePair<float, Attributes>(tCalc.getChebyshev, trainingInstance));
				Manhattan.Add(new KeyValuePair<float, Attributes>(tCalc.getManhattan, trainingInstance));
			}
		}
		Euclidaen = Euclidaen.OrderBy(o => o.Key).Take(k).ToList();
		Chebyshev = Chebyshev.OrderBy(o => o.Key).Take(k).ToList();
		Manhattan = Manhattan.OrderBy(o => o.Key).Take(k).ToList();
	}
	public List<KeyValuePair<float, Attributes>> eucli{get{return Euclidaen;}}
	public List<KeyValuePair<float, Attributes>> cheby{get{return Chebyshev;}}
	public List<KeyValuePair<float, Attributes>> manhatt{get{return Manhattan;}}
}
