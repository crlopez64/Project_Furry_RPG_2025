using UnityEngine;

/// <summary>
/// Script in charge of entering Battle if a Player enters its trigger zone.
/// </summary>
public class EnemyBattleTrigger : MonoBehaviour
{
    private CircleCollider2D triggerToBattleZone;

    private void Awake()
    {
        triggerToBattleZone = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerMove>() != null)
        {
            Debug.Log("Enter Battle now!!");
            GameManager.AddEnemyToListToBattle(GetComponentInParent<EnemyOverworld>().enemyName);
            // How to bring over enemies nearby?
            GameManager.MoveToBattleScene();
        }
    }

    /// <summary>
    /// Disable the Battle Trigger.
    /// </summary>
    public void DisableBattleTrigger()
    {
        triggerToBattleZone.enabled = false;
    }

    /// <summary>
    /// Enable the Battle Trigger.
    /// </summary>
    public void EnableBattleTrigger()
    {
        triggerToBattleZone.enabled = true;
    }
}
