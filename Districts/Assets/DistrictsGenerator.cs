using UnityEngine;
using System.Collections;
using AssemblyCSharp;


public class DistrictsGenerator : MonoBehaviour {

	[SerializeField]
	private float[,] districtEndPoints;

	[SerializeField]
	private float[,] districtEdges;

	[SerializeField]
	private float[,] cityCenter;

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

		int[,] initialSeedPoints = new int	[numDistricts,2];
		int[,] seedMidPoints	 = new int	[numDistricts,2];
			   districtEndPoints = new float[numDistricts,2];
			   cityCenter 		 = new float[1, 2];
			   districts 		 = new AssemblyCSharp.District[numDistricts];

		// generate initial random seed points
		for (int i = 0; i < numDistricts; ++i) {
			initialSeedPoints[i,0] = Random.Range(1, districtMaxSpan);
			initialSeedPoints[i,1] = Random.Range(1, districtMaxSpan);
		}

		// calculate midpoints of all initial seeds
		for (int i = 0; i < numDistricts; ++i){
			for (int j = 0; j  < numDistricts; ++j){
					int x = (initialSeedPoints[i,0]+initialSeedPoints[j,0])/2;
					int y = (initialSeedPoints[i,1]+initialSeedPoints[j,1])/2;
					seedMidPoints[j,0] = x;
					seedMidPoints[j,1] = y;
			}
		}

		// using these midpoints, get the center of the city
		float cityCenterX = 0;
		float cityCenterY = 0;

		for (int i = 0; i < numDistricts; ++i){
			cityCenterX += seedMidPoints[i,0];
			cityCenterY += seedMidPoints[i,1];
		}

		cityCenterX = cityCenterX/numDistricts;
		cityCenterY = cityCenterY/numDistricts;

		cityCenter [0, 0] = cityCenterX;
		cityCenter [0, 1] = cityCenterY;

		// for each point, get the slope and it's length based districtSpan and its position relative to city center
		for (int i = 0; i < numDistricts; ++i){

			float slope = ((float)(cityCenterY-seedMidPoints[i,1]))/((float)(cityCenterX-seedMidPoints[i,0]));
			float k = districtMaxSpan/(Mathf.Sqrt(1+Mathf.Pow(slope, 2.0f)));

			// account for position of point relative in space when assigning end point
			float currentEndpointX = 0.0f;
			float currentEndpointY = 0.0f;

			if (seedMidPoints[i,0] < cityCenterX)	currentEndpointX = (float)cityCenterX - k;
			else 									currentEndpointX = (float)cityCenterX + k;
		
			if (seedMidPoints[i,1] < cityCenterY)	currentEndpointY = (float)cityCenterY - (k*slope);
			else 									currentEndpointY = (float)cityCenterY + (k*slope);

			districtEndPoints [i, 0] = currentEndpointX;
			districtEndPoints [i, 1] = currentEndpointY;

		}

		for (int i = 0; i < numDistricts; i++) {
			districts [i] = new District (cityCenter);
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

		int numVerts = Random.Range (minVerts, maxVerts);
		float[,] points = new float [numVerts, 2];

		// generate randomly angled points & add to vector
		for (int i = 0; i < numVerts; i++){

			float angleFromCenter = Random.Range (0, 2 * Mathf.PI);
			float furthestX = Mathf.Cos(angleFromCenter)*maxDistFromCenter;
			float furthestY = Mathf.Sin(angleFromCenter)*maxDistFromCenter;

			float cityCenterX = cityCenter [0, 0];
			float cityCenterY = cityCenter [0, 1];

			float percentageLength = Mathf.PerlinNoise (cityCenterX, cityCenterY);
			float distanceFromCenter = percentageLength * maxDistFromCenter;

			float newX = cityCenterX + (distanceFromCenter) * Mathf.Cos (angleFromCenter);
			float newY = cityCenterY + (distanceFromCenter) * Mathf.Sin (angleFromCenter);

			points [i, 0] = newX;
			points [i, 1] = newY;
		}
	}
}


// http://stackoverflow.com/questions/1638437/given-an-angle-and-length-how-do-i-calculate-the-coordinates