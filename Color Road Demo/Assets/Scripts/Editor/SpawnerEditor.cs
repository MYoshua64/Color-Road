using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BallSpawner))]
public class SpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        BallSpawner myTarget = (BallSpawner)target;
        if (GUILayout.Button("Snap To Path"))
        {
            myTarget.SnapToPath();
        }
    }
}
