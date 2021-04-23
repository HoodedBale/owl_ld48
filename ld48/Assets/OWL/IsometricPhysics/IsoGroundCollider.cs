using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoGroundCollider : IsoPolygonCollider
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CheckCollision(List<Vector3> line)
    {

        if(CheckWithinZ(line))
        {
            //Debug.Log("hello");
            return CheckLineIntersection(line);
        }

        return false;
    }

    bool CheckWithinZ(List<Vector3> line)
    {
        float height = realPoints[0].y - realPoints[2].y;

        for(int i = 0; i < line.Count; ++i)
        {
            if(line[i].z < transform.position.z && line[i].z > transform.position.z - height) return true;
        }

        return false;
    }

    bool CheckLineIntersection(List<Vector3> line)
    {
        if (line.Count < 2) return false;

        for(int i = 0; i < realPoints.Count; ++i)
        {
            Vector3 normal = realPoints[(i + 1) % realPoints.Count] - realPoints[i];
            normal = new Vector3(normal.y, -normal.x, normal.z);

            Vector3 refPos = transform.position + realPoints[i];
            Vector3 dir0 = line[0] - refPos;
            Vector3 dir1 = line[1] - refPos;

            if ((normal.x * dir0.x + normal.y * dir0.y) * (normal.x * dir1.x + normal.y * dir1.y) < 0) return true;

            if(i == 0)
            {
                Debug.DrawLine(refPos, line[0], Color.red);
                Debug.DrawLine(refPos, line[1], Color.red);
                Debug.DrawLine(refPos, refPos + normal, Color.blue);
            }
        }

        return false;
    }
}
