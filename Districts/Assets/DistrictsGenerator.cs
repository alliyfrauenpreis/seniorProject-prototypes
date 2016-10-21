using UnityEngine;
using System.Collections;
using AssemblyCSharp;


public class DistrictsGenerator : MonoBehaviour {

	[SerializeField]
	private Vector2[] districtEndPoints;

	[SerializeField]
	private Vector2[] districtEdges;

	[SerializeField]
	private Vector2 cityCenter;

	private District[] districts;

	/// <summary>
	/// Start the program
	/// </summary>
	void Start () {
		generateDistrictPoints (3, 500);
	}

	/// <summary>
	/// Generates city center and district edges. Edges of districts are defined as (cityCenterX,cityCenterY)->(districtEndPoints[i,0],districtEndPoints[i,1])
	/// </summary>
	/// <param name="numDistricts">Number of districts desired -- currently supports 3.</param>
	/// <param name="districtMaxSpan">District max span for one side from center point to edge.</param>
	void generateDistrictPoints(int numDistricts, int districtMaxSpan){

		Vector2[] initialSeedPoints = new Vector2[numDistricts];
		Vector2[] seedMidPoints		= new Vector2[numDistricts];
				  districtEndPoints = new Vector2[numDistricts];
				  cityCenter 	    = new Vector2();
			      districts 		= new AssemblyCSharp.District[numDistricts];

		// generate initial random seed points
		for (int i = 0; i < numDistricts; ++i) {
			initialSeedPoints[i].x = Random.Range(1, districtMaxSpan);
			initialSeedPoints[i].y = Random.Range(1, districtMaxSpan);
		}

		// calculate midpoints of all initial seeds
		for (int i = 0; i < numDistricts; ++i){
			for (int j = 0; j  < numDistricts; ++j){
				float x = (initialSeedPoints[i].x+initialSeedPoints[j].x)/2;
				float y = (initialSeedPoints[i].y+initialSeedPoints[j].y)/2;
				seedMidPoints[j].x = x;
				seedMidPoints[j].y = y;
			}
		}

		// using these midpoints, get the center of the city
		float cityCenterX = 0;
		float cityCenterY = 0;

		for (int i = 0; i < numDistricts; ++i){
			cityCenterX += seedMidPoints [i].x;
			cityCenterY += seedMidPoints[i].y;
		}

		cityCenter.x = cityCenterX/numDistricts;
		cityCenter.y = cityCenterY/numDistricts;

		// for each point, get the slope and it's length based districtSpan and its position relative to city center
		for (int i = 0; i < numDistricts; ++i){

			float slope = ((float)(cityCenterY-seedMidPoints[i].y))/((float)(cityCenterX-seedMidPoints[i].x));
			float k = districtMaxSpan/(Mathf.Sqrt(1+Mathf.Pow(slope, 2.0f)));

			// account for position of point relative in space when assigning end point
			float currentEndpointX = 0.0f;
			float currentEndpointY = 0.0f;

			if (seedMidPoints[i].x < cityCenterX)	currentEndpointX = (float)cityCenterX - k;
			else 									currentEndpointX = (float)cityCenterX + k;
		
			if (seedMidPoints[i].y < cityCenterY)	currentEndpointY = (float)cityCenterY - (k*slope);
			else 									currentEndpointY = (float)cityCenterY + (k*slope);

			districtEndPoints [i].x = currentEndpointX;
			districtEndPoints [i].y = currentEndpointY;

		}

		for (int i = 0; i < numDistricts; i++) {
			districts [i] = new District (cityCenter);
		//	districts [i].setVerticies (districtEndPoints [i]);
		}

		generateCityEdges (50, 100, 800, 300);
	}

	/// <summary>
	/// Generates the city edge vertices.
	/// </summary>
	/// <param name="minVerts">Minimum # of vertices for edges of city.</param>
	/// <param name="maxVerts">Maximum # of vertices for edges of city</param>
	/// <param name="maxDistFromCenter">Maximum distance of all verticies from center.</param>
	/// <param name="minDistFromCenter">Minimum distance of all verticies from center.</param>
	/// 
	void generateCityEdges(int minVerts, int maxVerts, float maxDistFromCenter, float minDistFromCenter){

		// for each district, generate random # of randomly angled points within the current area of the district & add to vector
		for (int i = 0; i < districts.Length; i++){

			int numVerts = Random.Range (minVerts, maxVerts);
			Vector2[] points = new Vector2 [numVerts];

			Vector2[] currentVerts = districts [i].getVerticies();

			for (int j = 0; j < numVerts; ++j){

				float percentageLength = Mathf.PerlinNoise (cityCenter.x, cityCenter.y);
				float distanceFromCenter = percentageLength * maxDistFromCenter;

				/*float newX = cityCenterX + (distanceFromCenter) * Mathf.Cos (angleFromCenter);
				float newY = cityCenterY + (distanceFromCenter) * Mathf.Sin (angleFromCenter);

				points [i, 0] = newX;
				points [i, 1] = newY;*/

			}
		}
	}
}


// http://stackoverflow.com/questions/1638437/given-an-angle-and-length-how-do-i-calculate-the-coordinates