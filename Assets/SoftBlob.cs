using System.Collections.Generic;
using UnityEngine;

public class SoftBlob : MonoBehaviour
{
    public int points = 12;
    public float radius = 50f;

    List<Vector2> current;
    List<Vector2> previous;
    LineRenderer line;

    float area;
    float circumference;
    float segmentLength;

    Vector2 center;
    Vector2 mousePosition;
    bool mouseColliding = false;

    void Start()
    {
        current = new List<Vector2>();
        previous = new List<Vector2>();

        area = radius * radius * Mathf.PI;
        circumference = radius * 2 * Mathf.PI;
        segmentLength = (circumference * 1.15f) / points;

        center = new Vector2(Screen.width, Screen.height) / 2;

        for (int i = 0; i < points; i++)
        {
            float angle = (360f / points) * i * Mathf.Deg2Rad;
            Vector2 pt = center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
            current.Add(pt);
            previous.Add(pt);
        }

        line = gameObject.AddComponent<LineRenderer>();
        line.positionCount = points + 1;
        line.loop = true;
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.widthMultiplier = 3f;
        line.startColor = Color.black;
        line.endColor = Color.black;
    }

    void Update()
    {
        center = new Vector2(Screen.width, Screen.height) / 2;

        if (Input.GetMouseButtonDown(0)) mouseColliding = true;
        if (Input.GetMouseButtonUp(0)) mouseColliding = false;

        mousePosition = Input.mousePosition;

        // Verlet integration + gravity
        for (int i = 0; i < points; i++)
        {
            Vector2 temp = current[i];
            current[i] += (current[i] - previous[i]) + Vector2.down * Time.deltaTime * 30f;
            previous[i] = temp;
        }

        // 10 iterations constraint solve
        for (int iteration = 0; iteration < 10; iteration++)
        {
            // Keep segment distance
            for (int i = 0; i < points; i++)
            {
                int next = (i + 1) % points;
                Vector2 delta = current[next] - current[i];
                float dist = delta.magnitude;
                float diff = (dist - segmentLength) / dist;
                Vector2 offset = delta * 0.5f * diff;
                current[i] += offset;
                current[next] -= offset;
            }

            // Area pressure
            float currentArea = ComputePolygonArea(current);
            float deltaArea = area - currentArea;
            float dilation = deltaArea / circumference;

            for (int i = 0; i < points; i++)
            {
                int prev = (i - 1 + points) % points;
                int next = (i + 1) % points;
                Vector2 normal = (current[next] - current[prev]).normalized;
                Vector2 perp = new Vector2(-normal.y, normal.x);
                current[i] += perp * dilation;
            }

            // Collision with mouse + circular bounds
            for (int i = 0; i < points; i++)
            {
                if (mouseColliding && (current[i] - mousePosition).magnitude < 40)
                    current[i] = mousePosition + (current[i] - mousePosition).normalized * 40;

                if ((current[i] - center).magnitude > 150)
                    current[i] = center + (current[i] - center).normalized * 150;
            }
        }

        DrawBlob();
    }

    void DrawBlob()
    {
        for (int i = 0; i < points; i++)
            line.SetPosition(i, Camera.main.ScreenToWorldPoint(new Vector3(current[i].x, current[i].y, 10f)));

        line.SetPosition(points, line.GetPosition(0)); // loop
    }

    float ComputePolygonArea(List<Vector2> pts)
    {
        float a = 0;
        for (int i = 0; i < pts.Count; i++)
        {
            Vector2 p1 = pts[i];
            Vector2 p2 = pts[(i + 1) % pts.Count];
            a += p1.x * p2.y - p2.x * p1.y;
        }
        return Mathf.Abs(a) / 2f;
    }
}
