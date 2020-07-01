using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class SnakeTail : MonoBehaviour {

    LineRenderer lineRenderer;
    EdgeCollider2D edgeCollider;
    //MeshCollider meshCollider;

    List<Vector2> linePoints;
    public int offset = 6;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
        lineRenderer.useWorldSpace = true;

        linePoints = new List<Vector2>();

        edgeCollider = GetComponent<EdgeCollider2D>();
        //meshCollider = lineRenderer.GetComponent<MeshCollider>();
    }

    public void SetColor(Color color)
    {
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
    }

    public void SetWidth(float width)
    {
        edgeCollider.edgeRadius = width / 2.0f - 0.1f;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
    }

    public void SetOffset(int i)
    {
        offset = i;
    }
  
    public void UpdateTail(Vector2 position)
    {
        // Should move collider to left and right? 

        //if (linePoints.Count > 1)
        //{
        //    edgeCollider.points = linePoints.ToArray<Vector2>();
        //}

        linePoints.Add(position);
        lineRenderer.positionCount = linePoints.Count;
        lineRenderer.SetPosition(linePoints.Count - 1, linePoints[linePoints.Count - 1]);

        //Mesh mesh = new Mesh();
        //lineRenderer.BakeMesh(mesh, true);
        //meshCollider.sharedMesh = mesh;

        if (linePoints.Count > offset)
        {
            var edgePoints = linePoints.Take(linePoints.Count - offset);
            edgeCollider.points = edgePoints.ToArray<Vector2>();
        }
    }

    public void AdjustCollider()
    {
        edgeCollider.points = linePoints.ToArray<Vector2>();
    }

}
