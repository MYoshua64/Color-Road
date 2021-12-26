using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LaunchSlope))]
public class LaunchSlopeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        LaunchSlope myTarget = (LaunchSlope)target;
        if (GUILayout.Button("Snap To Path"))
        {
            myTarget.SnapToPath();
        }
    }
}
