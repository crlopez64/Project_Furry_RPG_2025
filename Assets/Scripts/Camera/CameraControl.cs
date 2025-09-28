using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraControl : MonoBehaviour
{
    private CameraAction cameraAction;
    private UnitMove focusUnit;
    private readonly float cameraSmoothing = 0.25f;
    private readonly float cameraStaticZposition = -20f;
    private Vector3 battleFocus = new Vector3(4.25f, 0.85f, 0); //To add on top of hero position
    private Vector3 centerOfBattle;
    private Vector3 positionGoal;
    private Vector3 velocityRef;
    //TODO: Add camera clamps

    public bool useBattleCamera;

    private enum CameraAction
    {
        NONE,
        FOLLOW_UNIT,
        IN_BATTLE
    }

    private void Start()
    {
        if (!useBattleCamera)
        {
            OverworldCamera(FindAnyObjectByType<PlayerMove>());
        }
    }
    void Update()
    {
        switch(cameraAction)
        {
            case CameraAction.FOLLOW_UNIT:
                Vector3 playerPosition = focusUnit.transform.position;
                transform.position = new Vector3(playerPosition.x, playerPosition.y + 2f, -10);
                break;
            case CameraAction.IN_BATTLE:
                transform.position = Vector3.SmoothDamp(transform.position, positionGoal, ref velocityRef, cameraSmoothing);
                break;
        }
    }

    /// <summary>
    /// To prevent weird bugs, set position goal to its current immediate position.
    /// </summary>
    public void BattleBegin(List<GameObject> heroes, List<GameObject> enemies)
    {
        positionGoal = transform.position;
        FindCenterOfMainSetting(heroes, enemies);
        FocusCameraToCenterOfBattle();
    }

    /// <summary>
    /// Set the camera for the overworld.
    /// </summary>
    public void OverworldCamera(PlayerMove player)
    {
        focusUnit = player;
        cameraAction = CameraAction.FOLLOW_UNIT;
        Vector3 playerPosition = player.transform.position;
        transform.position = new Vector3(playerPosition.x, playerPosition.y + 10f, -10);
    }

    public void FocusOnUnit(UnitMove unitMove)
    {
        focusUnit = unitMove;
        cameraAction = CameraAction.FOLLOW_UNIT;
        Vector3 playerPosition = unitMove.transform.position;
        transform.position = new Vector3(playerPosition.x, playerPosition.y + 10f, -10);
    }

    /// <summary>
    /// Set the Camera Position goal such that it's in the center of the left half of the screen.
    /// </summary>
    /// <param name="hero"></param>
    public void FocusCameraToHeroDuringMenus(GameObject hero)
    {
        cameraAction = CameraAction.IN_BATTLE;
        positionGoal = new Vector3(hero.gameObject.transform.position.x, hero.gameObject.transform.position.y, cameraStaticZposition) + battleFocus;
    }

    /// <summary>
    /// Set the Camera Position goal such that it's in the middle of everyone in center.
    /// </summary>
    public void FocusCameraToCenterOfBattle()
    {
        cameraAction = CameraAction.IN_BATTLE;
        positionGoal = centerOfBattle;
    }

    /// <summary>
    /// Find the center of Main Setting.
    /// </summary>
    /// <param name="heroes"></param>
    /// <param name="enemies"></param>
    private void FindCenterOfMainSetting(List<GameObject> heroes, List<GameObject> enemies)
    {
        Bounds bounds = new Bounds();
        for (int i = 0; i < heroes.Count; i++)
        {
            bounds.Encapsulate(heroes[i].gameObject.transform.position);
        }
        for(int i = 0; i < enemies.Count; i++)
        {
            bounds.Encapsulate(enemies[i].transform.position);
        }
        centerOfBattle = new Vector3(bounds.center.x, bounds.center.y, cameraStaticZposition);
    }
}
