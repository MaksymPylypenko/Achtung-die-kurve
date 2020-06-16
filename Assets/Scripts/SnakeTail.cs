using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(EdgeCollider2D))]
public class SnakeTail : MonoBehaviour {

    public float pointSpacing = .1f;

    LineRenderer line;
    EdgeCollider2D edgeCollider;

    List<Vector2> linePoints;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 0;
        line.useWorldSpace = true;

        linePoints = new List<Vector2>();

        edgeCollider = GetComponent<EdgeCollider2D>();
    }

    public void SetColor(Color color)
    {
        line.startColor = color;
        line.endColor = color;
    }

    public void SetWidth(float width)
    {
        line.startWidth = width;
        line.endWidth = width;
    }

    //Vector3.Distance(linePoints.Last(), position) > pointSpacing;
    

    public void UpdateTail(Vector2 position)
    {
        //if (linePoints.Count > 1)
        //{
        //    edgeCollider.points = linePoints.ToArray<Vector2>();
        //}
           
        linePoints.Add(position);
        line.positionCount = linePoints.Count;
        line.SetPosition(linePoints.Count - 1, linePoints[linePoints.Count - 1]);

        if (linePoints.Count > 6)
        {
            var edgePoints = linePoints.Take(linePoints.Count - 6);
            edgeCollider.points = edgePoints.ToArray<Vector2>();
        }
        
    }

}
