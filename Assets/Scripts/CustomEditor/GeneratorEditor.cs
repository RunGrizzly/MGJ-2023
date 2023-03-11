using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


[UnityEditor.CustomEditor(typeof(TrackGrid))]
public class GeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        TrackGrid trackGrid = (TrackGrid)target;
        DrawDefaultInspector();
        if (GUILayout.Button("Generate Grid"))
        {
            trackGrid.Start();
        }
    }
}

