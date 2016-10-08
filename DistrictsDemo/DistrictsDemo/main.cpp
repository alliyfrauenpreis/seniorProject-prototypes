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
 
 http://stackoverflow.com/questions/2512978/how-to-delete-duplicate-vectors-within-a-multidimensional-vector
 */

int main(int argc, const char * argv[]) {
    
    
    
    int numDistricts = 3;
    int sceneWidth = 500;
    int sceneHeight = 500;
    
    int edgeBuffer = 100; // this is the buffer of how far we want the points to at least be from the edge.
    vector<vector<int>> startPoints(numDistricts);
    vector<vector<int>> midPoints(numDistricts);
    vector<vector<float>> endPoints(numDistricts);
    
    startPoints.reserve(numDistricts);
    
    startPoints.reserve(numDistricts);
    for (int i = 0; i < numDistricts; i++){
        
        startPoints[i].reserve(numDistricts);
        midPoints[i].reserve(2);
        endPoints[i].reserve(2);
    }
    
    // create 3 random points
    srand(time(0));
    
    for (int i = 0; i < numDistricts; i++){
        
        // x value
        startPoints[i][0] = rand() % sceneWidth;
        
        // y value
        startPoints[i][1] = rand() % sceneHeight;
    }
    

    
    for (int i = 0; i < numDistricts; i++){
        
        cout << startPoints[i][0] << "," << startPoints[i][1] << endl;
        
    }
    
    int tCenterX = 0;
    int tCenterY = 0;
    
    // calculate midpoints, ignoring pre-existing pairs
    for (int i = 0; i < numDistricts; i++){
        
        for (int j = 0; j  < numDistricts; j++){
            
            if (i!=j){
                
                int x = (startPoints[i][0]+startPoints[j][0])/2;
                int y = (startPoints[i][1]+startPoints[j][1])/2;
                
                vector<int> point { x,y };
                
                midPoints.push_back(point);
                    cout << "midpoint for points " << i << " and " << j << " is " << x << ", " << y << endl;
            }
            
        }
        
    }
    
    for (int i = 0; i < midPoints.size(); i++){
        
        cout << midPoints[i][0] << "," << midPoints[i][1] << endl;
    }
    
    sort(midPoints.begin(), midPoints.end());
    midPoints.erase(unique(midPoints.begin(), midPoints.end()), midPoints.end());
    
    for (int i = 0; i < midPoints.size(); i++){
        
        cout << midPoints[i][0] << "," << midPoints[i][1] << endl;
        
        tCenterX += midPoints[i][0];
        tCenterY += midPoints[i][1];
    }
    
    // using these midpoints, get the center of the triangle
    tCenterX = tCenterX/numDistricts;
    tCenterY = tCenterY/numDistricts;
    
    cout << "midpoint for triangle is " << tCenterX << ", " << tCenterY << endl;
    
    // for each point, get the slope and it's length based on distance from the edge of the buffer
    for (int i = 1; i < numDistricts+1; i++){
        
        float slope = ((float)(tCenterY-midPoints[i][1]))/((float)(tCenterX-midPoints[i][0]));
        
        cout << "(" << tCenterX << "," << tCenterY << ") to (" << midPoints[i][0] << "," << midPoints[i][1] << ")" << endl;
        cout << " slope is " << slope << endl;
        
        int d = 300;
        
        float k = d/(sqrt(1+pow(slope, 2.0)));
        cout << k << endl;
        
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
    
    
    sort(endPoints.begin(), endPoints.end());
    endPoints.erase(unique(endPoints.begin(), endPoints.end()), endPoints.end());
    
    for (int i = 1; i<endPoints.size(); i++){
        
        cout << "(" << endPoints[i][0] << ", " << endPoints[i][1] << ")" << endl;
    }
    
    
    
    // lines are just (tCenterX,tCenterY)->endPoints(i);
    
    
    
    
    
    
    return 0;
}
