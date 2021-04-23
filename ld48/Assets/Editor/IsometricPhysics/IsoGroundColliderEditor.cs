using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(IsoGroundCollider)), CanEditMultipleObjects]
public class IsoGroundColliderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        IsoGroundCollider obj = (IsoGroundCollider)target;

        if (GUILayout.Button("Generate Collider"))
        {
            obj.GenerateCollider();
        }
    }
}
