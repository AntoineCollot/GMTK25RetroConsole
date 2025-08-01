using UnityEngine;

public class Opponent : MonoBehaviour, IDualable
{
    [Header("Components")]
    [SerializeField] GameObject detectFX;
    CharacterAnimations characterAnimations;

    [Header("Settings")]
    [SerializeField] float detectFreezeTime = 1;

    [Header("Stats")]
    [SerializeField] int baseHP = 3;
    int currentHP;
    public int CurrentHP => currentHP;
    [SerializeField] int strength = 1;
    public int Strength => strength;

    CompositeStateToken freezePlayerToken;

    public Vector2 Position => transform.position;
    public Vector2Int GridPos => GameGrid.WordPosToGrid(transform.position);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        freezePlayerToken = new CompositeStateToken();
        characterAnimations = GetComponentInChildren<CharacterAnimations>();

        currentHP = baseHP;
    }

    private void OnDestroy()
    {
    }

    public void OnPlayerDetected(Vector2Int playerPosition)
    {
        PlayerState.Instance.freezeGameplayInputState.Add(freezePlayerToken);
        freezePlayerToken.SetOn(true);
        detectFX.SetActive(true);

        Invoke("StartDuel", detectFreezeTime);
    }

    void StartDuel()
    {
        detectFX.SetActive(false);
        characterAnimations.SetSortingLayerAsDuel();
        DuelManager.Instance.StartDuel(this);
    }

    public void OnDuelFinished()
    {
        freezePlayerToken.SetOn(false);
        PlayerState.Instance.freezeGameplayInputState.Remove(freezePlayerToken);
        characterAnimations.ResetSortingLayer();

        if (currentHP < 0)
            Die();
    }

    public void Attack()
    {
        characterAnimations.Attack();
    }

    public void TakeDamages(int damage, out bool die)
    {
        currentHP -= damage;
        die = false;

        if (currentHP <= 0)
            die = true;
    }

    public void Die()
    {

    }
}

public interface IDualable
{
    int CurrentHP { get; }
    int Strength { get; }

    void TakeDamages(int damage, out bool die);
}