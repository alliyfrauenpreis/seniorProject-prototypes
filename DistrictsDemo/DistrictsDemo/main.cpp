//
//  main.cpp
//  DistrictsDemo
//
//  Created by Allison Frauenpreis on 10/6/16.
//  Copyright © 2016 Allison Frauenpreis. All rights reserved.
//

#include <iostream>
#include <vector>
#include <math.h>

using namespace std;


/*
 
 Find 3 random points.
 
1. Find the exact midpoint between each of these points.
 
2. Create a triangle with verticies at each of these midpoints.
 
3. Find the center point of the triangle.
 
4. Connect a line from the center point of the triangle to each verticie of the triangle.
 
 This gives us our edges between the districts.
 
 For any additional points, find the two points closet to the new point and repeat the above process.
 
 for vectors, [i][0] is x value, [1][1] is y value
 
 http://stackoverflow.com/questions/2512978/how-to-delete-duplicate-vectors-within-a-multidimensional-vector
 */




/// Returns a vector of 50-100 points who are randomly distributed around center point x,y and whose distance is based on distFromXY (will be perlin noise later)
/// http://stackoverflow.com/questions/9879258/how-can-i-generate-random-points-on-a-circles-circumference-in-javascript

vector<vector<float>> perlinNoise(float x, float y){
    
    // determine how many points to generate (50-100)
    vector<vector<float>> points;
    srand(time(0));
    int numPoints = rand()%(100-50 + 1) + 50;
    
    // set rough distance from center (until we have perlin noise func)
    float distFromXY = 500.0f;
    
    // generate random points & add to vector
    srand(time(0));
    for (int i = 0; i < numPoints; i++){
        
        float angle = rand() * 3.14 * 2;
        float newX = cos(angle)*distFromXY;
        float newY = sin(angle)*distFromXY;
        
        vector<float> point = {newX, newY};
        points.push_back(point);
    }
    
    return points;
}


int main(int argc, const char * argv[]) {
    
    int numDistricts = 3;
    
    // atoi(argv[1]);   // passed argument for number of districts; currently only support 3
    int districtSpan = 500;             // the length of one vector that creates a district barrier.
    
    vector<vector<int>> startPoints(numDistricts);  // randomly initialized district points
    vector<vector<int>> midPoints(numDistricts);    // midpoints between all random district points
    vector<vector<float>> endPoints(numDistricts);  // end of line segments from center point that split districts
    
    startPoints.reserve(numDistricts);
    
    for (int i = 0; i < numDistricts; i++){
        
        startPoints[i].reserve(numDistricts);
        midPoints[i].reserve(numDistricts);
        endPoints[i].reserve(numDistricts);
    }
    
    // create numDistricts random points based on district span
    srand(time(0));
    
    for (int i = 0; i < numDistricts; i++){
        
        // x value
        startPoints[i][0] = rand() % districtSpan;
        
        // y value
        startPoints[i][1] = rand() % districtSpan;
        
        // this code ensures that neither the x nor the y values of this random point
        // are too close to another existing random point. it will re-randomize if so.
        if (!startPoints.empty()){
            for (int j = 0; j < startPoints.size(); j++){
                if (startPoints[i][0] != startPoints[j][0]){
                    if (abs(startPoints[i][0] - startPoints[j][0]) < districtSpan/10){
                        while(abs(startPoints[i][0] - startPoints[j][0]) < 100){
                            startPoints[i][0] = rand() % districtSpan;
                        }
                    }
                }
                    
                if (startPoints[i][1] != startPoints[j][1]){
                    if (abs(startPoints[i][1] - startPoints[j][1]) < districtSpan/10){
                        while(abs(startPoints[i][1] - startPoints[j][1]) < 100){
                            startPoints[i][1] = rand() % districtSpan;
                        }
                    }
                }
            }
        }
    }
    
    
    // to save triangle center point
    int tCenterX = 0;
    int tCenterY = 0;
    
    // calculate midpoints of all random initial points
    for (int i = 0; i < numDistricts; i++){
        
        for (int j = 0; j  < numDistricts; j++){
            
            if (i!=j){
                int x = (startPoints[i][0]+startPoints[j][0])/2;
                int y = (startPoints[i][1]+startPoints[j][1])/2;
                
                vector<int> point { x,y };
                midPoints.push_back(point);
            }
        }
    }
    
    // remove unnecessary duplicate points
    sort(midPoints.begin(), midPoints.end());
    midPoints.erase(unique(midPoints.begin(), midPoints.end()), midPoints.end());
    
    // using these midpoints, get the center of the triangle
    for (int i = 0; i < midPoints.size(); i++){
        
        tCenterX += midPoints[i][0];
        tCenterY += midPoints[i][1];
    }
    
    tCenterX = tCenterX/numDistricts;
    tCenterY = tCenterY/numDistricts;
    
    cout << "midpoint for triangle is " << tCenterX << ", " << tCenterY << endl;
    
    
    // for each point, get the slope and it's length based districtSpan and its position in relation to the triangle center
    for (int i = 1; i < numDistricts+1; i++){
        
        float slope = ((float)(tCenterY-midPoints[i][1]))/((float)(tCenterX-midPoints[i][0]));
        
        cout << "(" << tCenterX << "," << tCenterY << ") to (" << midPoints[i][0] << "," << midPoints[i][1] << ")" << endl;
        
        
        // compute k for endpoint equation
        float k = districtSpan/(sqrt(1+pow(slope, 2.0)));
        
        // account for position of point relative in space when assigning end point
        float x,y;
        
        if (midPoints[i][0] < tCenterX){
            x = (float)tCenterX - k;
        } else {
            x = (float)tCenterX + k;
        }
        
        if (midPoints[i][1] < tCenterY){
            y = (float)tCenterY - (k*slope);
        } else {
            y = (float)tCenterY + (k*slope);
        }
        
        vector<float> point { x,y };
        
        endPoints.push_back(point);
        
    }
    
    // remove unecessary duplicates
    sort(endPoints.begin(), endPoints.end());
    endPoints.erase(unique(endPoints.begin(), endPoints.end()), endPoints.end());
    
    // lines are just (tCenterX,tCenterY)->endPoints(i);
    // to print to file, pipe cout to file name
    for (int i = 1; i<endPoints.size(); i++){
        
        cout << "(" << tCenterX << "," << tCenterY << "),(" << endPoints[i][0] << ", " << endPoints[i][1] << ")" << endl;
    }
    
    
    // PERLIN NOISE TIME
    vector<vector<float>> districtEdges = perlinNoise(tCenterX, tCenterY);
    
    return 0;
}


