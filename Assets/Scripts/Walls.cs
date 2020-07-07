using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Walls : MonoBehaviour
{
    LineRenderer line;
    EdgeCollider2D edgeCollider;


    void Awake()
    {
        line = GetComponent<LineRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();  
    }

    public void SetWalls(float range, float width)
    {
        float x = range;
        float y = range;

        // Line renderer requires 3d points.
        Vector3[] points3d = new Vector3[5];
        points3d[0] = new Vector3(-x-width/2, -y, 0f);
        points3d[1] = new Vector3(x, -y, 0f);
        points3d[2] = new Vector3(x, y, 0f);
        points3d[3] = new Vector3(-x, y, 0f);
        points3d[4] = new Vector3(-x, -y, 0f);
        line.positionCount = 5;
        line.SetPositions(points3d);

        // but edge collider requires 2d points so ...   
        Vector2[] points2d = new Vector2[5];
        points2d[0] = new Vector3(-x, -y);
        points2d[1] = new Vector3(x, -y);
        points2d[2] = new Vector3(x, y);
        points2d[3] = new Vector3(-x, y);
        points2d[4] = new Vector3(-x, -y);
        edgeCollider.points = points2d;

        // Is there a better way ?

        edgeCollider.edgeRadius = width / 2.0f;
        line.startWidth = width;
        line.endWidth = width;

    }
}
