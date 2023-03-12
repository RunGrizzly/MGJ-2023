using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Reflection;

[CustomEditor(typeof(Train))]
public class TrainEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var train = (Train)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Kill Train"))
        {
            train.Kill();
        }
    }
}
