using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



[CustomEditor(typeof(ThermalManager))]
[CanEditMultipleObjects]

public class CustomThermalManager : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var t = (target as ThermalManager);
        if (GUILayout.Button("Update Buffers"))
        {
            t.UpdateThermalBufferEditor();
        }

        if(GUILayout.Button("Change View Mode"))
        {
            t.ChangeViewMode();
            if(t.isThermalModeActive)
                t.UpdateThermalBufferEditor();
        }
    }
}
