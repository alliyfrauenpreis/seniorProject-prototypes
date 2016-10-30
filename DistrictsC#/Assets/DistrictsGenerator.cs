using UnityEngine;
using System.Collections;

public class DistrictsGenerator : MonoBehaviour {

	// Use this for initialization
	void Start () {

		generateDistricts (3, 500);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void generateDistricts(int numDistricts, int districtsSpan){

		int[][] 	initialPoints = new int[3][];
		int[][] 	initialMidPoints;		
		float[][] 	endPointsFromCenter;


 		for (int i = 0; i < numDistricts; ++i) {
			initialPoints [i][0] = Random.Range (0, districtsSpan);
		}
			
		int x = 0;

	}

}
