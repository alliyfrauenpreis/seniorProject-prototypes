using System;
using UnityEngine;
using System.Collections;
using AssemblyCSharp;

namespace AssemblyCSharp
{
	public class District
	{
		[SerializeField]
		private Vector2[] edgeVerticies;

		[SerializeField]
		private Vector2   cityCenter;

		[SerializeField]
		private String 	  districtName;


		public District (Vector2 center)
		{
			cityCenter = center;
		}

		public void setVerticies(Vector2[] verts){

			edgeVerticies = verts;
		}

		public Vector2[] getVerticies( ){

			return edgeVerticies;
		}

		public void setName(String name){

			districtName = name;
		}

		public String getName(){

			return districtName;
		}

		public bool containsPoint(Vector2 point){

			// check whether this point's x value is between the district's edges
			if (point.x < edgeVerticies [0].x && point.x < edgeVerticies[1].x)
				return false;

			if (point.x > edgeVerticies [0].x && point.x > edgeVerticies[1].x)
				return false;

			// check whether this point's y value is between the district's edges
			if (point.y < edgeVerticies [0].y && point.y < edgeVerticies[1].y)
				return false;

			if (point.y > edgeVerticies [0].y && point.y > edgeVerticies[1].y)
				return false;

			return true;
		}
	}
}

