using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ColorSlope))]
public class SlopeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        ColorSlope myTarget = (ColorSlope)target;

        if (GUILayout.Button("Snap To Path"))
        {
            myTarget.SnapToPath();
        }
    }
}
