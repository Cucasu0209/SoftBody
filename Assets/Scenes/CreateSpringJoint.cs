using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class CreateSpringJoint : MonoBehaviour
{
    [SerializeField] private int PointCount = 30;
    [SerializeField] private float Radius = 30;
    [SerializeField] private Rigidbody2D PointPrefab;
    [SerializeField] private LineRenderer Line;
    [SerializeField] private Transform mouse;

    private List<Rigidbody2D> Points;
    [SerializeField] private List<Transform> ShapeController;
    private void UpdateLine()
    {
        Line.positionCount = Points.Count + 1;
        for (int i = 0; i < Points.Count; i++)
        {
            Line.SetPosition(i, Points[i].position);
            ShapeController[i].position = Points[i].position;
        }
        Line.SetPosition(Points.Count, Points[0].position);


    }

    private void Start()
    {
        CreatePoint();

        for (int i = 0; i < Points.Count; i++)
        {
            for (int j = 1; j <= (Points.Count - 1) / 2; j++)
            {
                SpringJoint2D joint1 = Points[i].AddComponent<SpringJoint2D>();
                SpringJoint2D joint2 = Points[i].AddComponent<SpringJoint2D>();
                joint1.connectedBody = Points[(i + j) % Points.Count];
                joint2.connectedBody = Points[(i - j + Points.Count) % Points.Count];
                joint1.breakAction = JointBreakAction2D.Ignore;
                joint2.breakAction = JointBreakAction2D.Ignore;
            }
        }

    }

    private void CreatePoint()
    {
        Points = new List<Rigidbody2D>();
        for (int i = 0; i < PointCount; i++)
        {
            Rigidbody2D newPoint = Instantiate(PointPrefab, Vector3.zero, Quaternion.identity);

            float angle = i * 360f / PointCount;
            newPoint.transform.SetParent(transform);
            newPoint.transform.localPosition = new Vector2(Radius * Mathf.Cos(Mathf.Deg2Rad * angle), Radius * Mathf.Sin(Mathf.Deg2Rad * angle));

            Points.Add(newPoint);
        }
    }

    bool dragging = false;
    List<int> LockedPointIndex;
    private Vector2 MousePos;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragging = true;

        }
        if (Input.GetMouseButtonUp(0))
        {
            dragging = false;
            mouse.transform.position = Vector3.one * 999;
        }

        if (dragging)
        {
            mouse.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        }


        UpdateLine();
    }
}
