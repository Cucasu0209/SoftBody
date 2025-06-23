using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaConstraint : MonoBehaviour
{
    [SerializeField] private List<Rigidbody2D> Points;
    private float TargetArea = 0;
    private void Start()
    {
        TargetArea = CalculateArea();
    }
    private void Update()
    {
        ApplyConstraint();
    }
    private float CalculateArea()
    {
        float area = 0;
        for (int i = 0; i < Points.Count; i++)
        {
            int i_1 = (i + 1) % Points.Count;

            area += (Points[i].position.x + Points[i_1].position.x) * 0.5f * (Points[i].position.y - Points[i_1].position.y);

        }
        return area;
    }

    private void ApplyConstraint()
    {
        //float currentArea = CalculateArea();

        //for (int i = 0; i < Points.Count; i++)
        //{
        //    int i_2 = (i + 2) % Points.Count;
        //    int i_1 = (i + 1) % Points.Count;

        //    Vector2 v = Points[i_2].position - Points[i].position;
        //    v = new Vector2((currentArea > TargetArea) ? v.y : -v.y, v.x);

        //    Points[i_1].velocity = v.normalized * Mathf.Abs(currentArea - TargetArea);

        //}
    }
}
