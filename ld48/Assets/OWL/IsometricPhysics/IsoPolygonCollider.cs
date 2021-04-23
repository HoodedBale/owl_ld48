using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoPolygonCollider : MonoBehaviour
{
    public List<Vector3> points = new List<Vector3>();

    public bool CollisionCheck(IsoPolygonCollider collider)
    {
        bool touch = true;

        foreach(var point in collider.points)
        {
            touch = true;

            for(int i = 0; i < points.Count; ++i)
            {
                Vector3 normal = points[(i + 1) % points.Count] - points[i];
                normal = new Vector3(normal.y, -normal.x, 0);
                Vector3 posVec = point - points[i];

                if(posVec.x * normal.x + posVec.y * normal.y < 0)
                {
                    touch = false;
                    break;
                }
            }

            if (touch) return true;
        }

        return false;
    }

    public void GenerateCollider()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (!sr) return;

        Vector2 pivot = sr.sprite.pivot;
        pivot.x /= sr.sprite.rect.width;
        pivot.y /= sr.sprite.rect.height;
        float width = sr.sprite.rect.width / 100.0f;
        float height = sr.sprite.rect.height / 100.0f;

        points = new List<Vector3>();
        points.Add(new Vector3(-pivot.x * width, (1 - pivot.y) * height, transform.position.z));
        points.Add(new Vector3((1 - pivot.x) * width, (1 - pivot.y) * height, transform.position.z));
        points.Add(new Vector3((1 - pivot.x) * width, -pivot.y * height, transform.position.z));
        points.Add(new Vector3(-pivot.x * width, -pivot.y * height, transform.position.z));
    }

    public List<Vector3> bottomCollider
    {
        get
        {
            if (realPoints.Count < 2) return null;

            List<Vector3> line = new List<Vector3>();

            Dictionary<int, bool> foundPoints = new Dictionary<int, bool>();
            for (int i = 0; i < 2; ++i)
            {
                float yMin = Mathf.Infinity;
                int minPoint = -1;

                for (int j = 0; j < realPoints.Count; ++j)
                {
                    if (foundPoints.ContainsKey(j)) continue;
                    if (realPoints[j].y < yMin)
                    {
                        yMin = realPoints[j].y;
                        minPoint = j;
                    }
                }

                line.Add(realPoints[minPoint] + transform.position);
                foundPoints.Add(minPoint, true);
            }

            return line;
        }
    }

    public List<Vector3> realPoints
    {
        get
        {
            List<Vector3> pointsList = new List<Vector3>();
            for(int i = 0; i < points.Count; ++i)
            {
                Vector3 realPoint = new Vector3(points[i].x * transform.localScale.x, points[i].y * transform.localScale.y, points[i].z);
                pointsList.Add(realPoint);
            }

            return pointsList;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (points.Count < 2) return;

        for(int i = 0; i < points.Count; ++i)
        {
            Vector3 realStart = points[i];
            realStart.x *= transform.localScale.x;
            realStart.y *= transform.localScale.y;
            realStart.z *= transform.localScale.z;
            realStart += transform.position;
            Vector3 realEnd = points[(i + 1) % points.Count];
            realEnd.x *= transform.localScale.x;
            realEnd.y *= transform.localScale.y;
            realEnd.z *= transform.localScale.z;
            realEnd += transform.position;
            Debug.DrawLine(realStart, realEnd, Color.green);
        }
    }
}
