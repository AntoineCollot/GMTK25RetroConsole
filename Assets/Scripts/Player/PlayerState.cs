using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlayerState : MonoBehaviour, IDualable
{
    public static PlayerState Instance;
    public CompositeState freezeGameplayInputState;
    Rigidbody body;

    int freezeInputFrame;
    public bool AreGameplayInputsFrozen => freezeGameplayInputState.IsOn || Time.frameCount<=freezeInputFrame || !RetroGameManager.Instance.GameIsPlaying;

    [Header("Duel")]
    [SerializeField] int baseHP = 5;
    [SerializeField] int baseStrength = 1;
    int currentHP;
    public int CurrentHP => currentHP;
    public int Strength => baseStrength;

    PlayerMovement movement;
    public event Action<Vector2Int> onPlayerPositionChanged;

    private void Awake()
    {
        freezeGameplayInputState = new CompositeState();
        movement = GetComponent<PlayerMovement>();

        Instance = this;
    }

    private void Start()
    {
        movement.onMovementFinished += OnMovementFinished;
        currentHP = baseHP;
    }

    private void OnDestroy()
    {
        if(movement!=null)
        {
            movement.onMovementFinished -= OnMovementFinished;
        }
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

    public void TakeDamages(int damage, out bool die)
    {
        currentHP -= damage;
        die = false;

        if (currentHP <= 0)
        {
            die = true;
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Player Died");
    }

    public void AddHP(int amount)
    {
        currentHP+= amount;
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

        if (behaviour.freezeGameplayInputState != null)
            EditorGUILayout.Toggle("Freeze Inputs", behaviour.freezeGameplayInputState.IsOn);

        // Write back changed values
        // This also handles all marking dirty, saving, undo/redo etc
        serializedObject.ApplyModifiedProperties();
    }
}
#endif