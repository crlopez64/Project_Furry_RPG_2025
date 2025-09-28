using UnityEngine;

/// <summary>
/// Script in charge of exposing the Character Signal to visually show who's turn it is.
/// </summary>
public class UnitCurrentSignal : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private BattleManager battleManager;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingLayerName = "UnitTarget";
    }

    /// <summary>
    /// Set the Signal to a specific character.
    /// </summary>
    /// <param name="unit"></param>
    public void SetSignalToCharacter(GameObject unit)
    {
        Vector3 getUnitPosition = unit.transform.position;
        Bounds bounds = new Bounds();
        bounds.Encapsulate(getUnitPosition);
        float getY = (bounds.extents.y/8);
        transform.position = new Vector3(getUnitPosition.x, getUnitPosition.y - getY, getUnitPosition.z);
        transform.SetParent(unit.transform);
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Turn off the Signal.
    /// </summary>
    public void ResetSignalToCharacter()
    {
        transform.SetParent(battleManager.transform);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Set the Battle Manager.
    /// </summary>
    /// <param name="battleManager"></param>
    public void SetBattleManager(BattleManager battleManager)
    {
        this.battleManager = battleManager;
    }
}
