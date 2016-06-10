using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Circle 
{
	private Vector3 center;
	private float radius;
	private Camera cam;
	private Material mat;
	private Texture2D lineTex;
	
	public Circle(Vector2 center, float radius)
	{
		this.cam = Camera.main;
		this.center = new Vector3(center.x, center.y, cam.nearClipPlane);
		this.radius = radius;
		this.mat = new Material( "Shader \"Lines/Colored Blended\" {" +
		                            "SubShader { Pass {" +
		                            "   BindChannels { Bind \"Color\",color }" +
		                            "   Blend SrcAlpha OneMinusSrcAlpha" +
		                            "   ZWrite Off Cull Off Fog { Mode Off }" +
		                            "} } }");
	}
	public void DrawCircle(Color color)
	{
		GL.PushMatrix();
		mat.SetPass(0);
		GL.Begin(GL.LINES);
		GL.Color(color);
		float degRad = Mathf.PI / 180;
		for(float theta = 0.0f; theta < (2*Mathf.PI); theta += 0.01f)
		{
			Vector3 ci = (new Vector3(Mathf.Cos(theta) * radius + center.x, Mathf.Sin(theta) * radius + center.y, center.z));
			GL.Vertex3(ci.x, ci.y, ci.z);
		}
		GL.End();
		GL.PopMatrix();
	}
}