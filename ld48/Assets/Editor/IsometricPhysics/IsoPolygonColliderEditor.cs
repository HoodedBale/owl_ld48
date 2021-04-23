using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(IsoPolygonCollider))]
public class IsoPolygonColliderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        IsoPolygonCollider obj = (IsoPolygonCollider)target;

        if (GUILayout.Button("Generate Collider"))
        {
            obj.GenerateCollider();
        }
    }
}
