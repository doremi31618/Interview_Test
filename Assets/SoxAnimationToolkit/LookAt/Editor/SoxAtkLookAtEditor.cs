﻿using UnityEngine;
//using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SoxAtkLookAt))]
public class SoxAtkLookAtEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SoxAtkLookAt tentacle = (SoxAtkLookAt)target;

        // GUI레이아웃 시작=======================================================
        Undo.RecordObject(target, "LookAt Changed Settings");

        EditorGUILayout.LabelField("-- The 'LookAt' is under development. --");

        Undo.FlushUndoRecordObjects();
        
        //DrawDefaultInspector();

        // GUI레이아웃 끝========================================================
    } // end of OnInspectorGUI()
}
