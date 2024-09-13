using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Polygon : MonoBehaviour
{
    // Start is called before the first frame update
    public int side = 4;
    public float radius = 1;
    public LineRenderer lineRenderer;
    public float width;

    void Start()
    {
        DrawLooped();
    }

    // Update is called once per frame
    void Update()
    {
       lineRenderer.SetWidth(width,width);
       DrawLooped(); 
    }

    void DrawLooped(){
        lineRenderer.positionCount = side;
        float TAU = 2*Mathf.PI;

        for(int currentPoints = 0; currentPoints<side; currentPoints++){
            float currentRadian = ((float)currentPoints/side) * TAU;
            float x = Mathf.Cos(currentRadian) * radius;
            float y = Mathf.Sin(currentRadian) * radius;
            lineRenderer.SetPosition(currentPoints, new Vector3(x,y,0));
        }
        lineRenderer.loop = true;
    }
}
