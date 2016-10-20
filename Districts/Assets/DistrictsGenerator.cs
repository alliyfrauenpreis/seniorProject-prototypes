using UnityEngine;
using System.Collections;

public class DistrictsGenerator : MonoBehaviour {

	[SerializeField]
	private float[,] districtEndPoints;

	[SerializeField]
	private float    cityCenterX, cityCenterY;


	/// <summary>
	/// Update the text to face the camera if shown.
	/// </summary>
	void Start () {
		// generateDistrictPoints (3, 500);
	}

	/// <summary>
	/// Generates city center and district edges. Edges of districts are defined as (cityCenterX,cityCenterY)->(districtEndPoints[i,0],districtEndPoints[i,1])
	/// <param name="numDistricts">Number of districts desired -- currently supports 3.</param>
	/// <param name="districtMaxSpan">District max span for one side from center point to edge.</param>
	/// </summary>
	void generateDistrictPoints(int numDistricts, int districtMaxSpan){

		int[,] initialSeedPoints = new int	[numDistricts,2];
		int[,] seedMidPoints	 = new int	[numDistricts,2];
			   districtEndPoints = new float[numDistricts,2];

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
		cityCenterX = 0;
		cityCenterY = 0;

		for (int i = 0; i < numDistricts; ++i){
			cityCenterX += seedMidPoints[i,0];
			cityCenterY += seedMidPoints[i,1];
		}

		cityCenterX = cityCenterX/numDistricts;
		cityCenterY = cityCenterY/numDistricts;

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
	}
}
