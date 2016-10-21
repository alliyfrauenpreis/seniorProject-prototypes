using System;

namespace AssemblyCSharp
{
	public class District
	{
		private float[,] edgeVerticies;
		private float[,] cityCenter;

		public District (float[,] center)
		{
			cityCenter = center;
		}

		public void setVerticies(float[,] verts){

			edgeVerticies = verts;
		}


		public bool containsPoint(float[,] point){

			return false;
		}
	}
}

