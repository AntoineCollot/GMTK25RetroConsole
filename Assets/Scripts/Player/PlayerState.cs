using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance;
    public CompositeState freezeInputsState;
    Rigidbody body;

    private void Awake()
    {
        freezeInputsState = new CompositeState();

        Instance = this;
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(PlayerState), true)]
public class PlayerStateEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlayerState behaviour = target as PlayerState;

        if (behaviour.freezeInputsState != null)
            EditorGUILayout.Toggle("Freeze Inputs", behaviour.freezeInputsState.IsOn);

        // Write back changed values
        // This also handles all marking dirty, saving, undo/redo etc
        serializedObject.ApplyModifiedProperties();
    }
}
#endif