using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance;
    public CompositeState freezeInputsState;
    Rigidbody body;

    int freezeInputFrame;
    public bool AreInputsFrozen => freezeInputsState.IsOn || Time.frameCount<=freezeInputFrame;

    PlayerMovement movement;
    public event Action<Vector2Int> onPlayerPositionChanged;

    private void Awake()
    {
        freezeInputsState = new CompositeState();
        movement = GetComponent<PlayerMovement>();

        Instance = this;
    }

    private void OnDestroy()
    {
        if(movement!=null)
        {
            movement.onMovementFinished -= OnMovementFinished;
        }
    }

    private void Start()
    {
        movement.onMovementFinished += OnMovementFinished;
    }

    private void OnMovementFinished(Vector2Int pos)
    {
        onPlayerPositionChanged?.Invoke(pos);
    }

    /// <summary>
    /// 0 being for current frame only
    /// </summary>
    /// <param name="count"></param>
    public void FreezeInputsForCurrentFrame(int count = 0)
    {
        freezeInputFrame = Time.frameCount+count;
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