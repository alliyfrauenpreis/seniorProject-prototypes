using System;
using UnityEngine;
using System.Collections;
using AssemblyCSharp;

namespace AssemblyCSharp
{
	public class District
	{
		private Vector2[] edgeVerticies;
		private Vector2   cityCenter;

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

		public bool containsPoint(Vector2[] point){

			return false;
		}
	}
}

