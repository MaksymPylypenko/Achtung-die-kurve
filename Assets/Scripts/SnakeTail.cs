using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(EdgeCollider2D))]
public class SnakeTail : MonoBehaviour {

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

  
    public void UpdateTail(Vector2 position)
    {
        //if (linePoints.Count > 1)
        //{
        //    edgeCollider.points = linePoints.ToArray<Vector2>();
        //}

        linePoints.Add(position);
        line.positionCount = linePoints.Count;
        line.SetPosition(linePoints.Count - 1, linePoints[linePoints.Count - 1]);


        if (linePoints.Count > 2)
        {
            var edgePoints = linePoints.Take(linePoints.Count - 2);
            edgeCollider.points = edgePoints.ToArray<Vector2>();
        }
    }

    public void AdjustCollider()
    {
        edgeCollider.points = linePoints.ToArray<Vector2>();
    }

}
