using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkinLine : MonoBehaviour
{
    [SerializeField] private LineRenderer Line;
    [SerializeField] private List<Transform> Points;

    private void Update()
    {
        Line.positionCount = Points.Count + 1;
        for (int i = 0; i < Points.Count; i++)
        {
            Line.SetPosition(i, Points[i].position);
        }
        Line.SetPosition(Points.Count, Points[0].position);
    }
}
