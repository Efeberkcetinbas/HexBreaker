using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public GameData gameData;


    private WaitForSeconds waitForSeconds;


    private void Awake() 
    {
        ClearData(true);
    }

    private void Start()
    {
        waitForSeconds=new WaitForSeconds(2);
    }

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.AddHandler(GameEvent.OnRestartLevel,OnRestartLevel);
        EventManager.AddHandler(GameEvent.OnHexDestroyed,OnHexDestroyed);
        EventManager.AddHandler(GameEvent.OnCheckCredit,OnCheckCredit);

    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.RemoveHandler(GameEvent.OnRestartLevel,OnRestartLevel);
        EventManager.RemoveHandler(GameEvent.OnHexDestroyed,OnHexDestroyed);
        EventManager.RemoveHandler(GameEvent.OnCheckCredit,OnCheckCredit);

    }

    
    private void OnHexDestroyed()
    {
        gameData.LevelHexParentNumber--;
        if(gameData.LevelHexParentNumber <=0)
        {
            Debug.Log("ALL HEX's DESTROYED");
        }
    }

    private void OnNextLevel()
    {
        ClearData(false);
        
    }

    private void OnRestartLevel()
    {
        ClearData(false);
    }

    private void OnConditionSuccess()
    {
        Debug.Log("PERFECT. CONG");
        EventManager.Broadcast(GameEvent.OnSuccess);
        StartCoroutine(OpenSuccess());

        ///////////////////////////
        
        Debug.Log("END FAIL");
        EventManager.Broadcast(GameEvent.OnGameOver);
    }

    
    private void OnCheckCredit()
    {
        int totalTowerValueSum = 0;
        HexParent[] hexParents = FindObjectsOfType<HexParent>(); // Finds all HexParent instances

        foreach (HexParent hexParent in hexParents)
        {
            totalTowerValueSum += hexParent.towerValue;
        }

        Debug.Log("TOTAL TOWER VALUE" + totalTowerValueSum);

        // Checking if the credit-based value is insufficient
        if ((gameData.CriticalHitDamage + 100) * gameData.Credit < totalTowerValueSum)
        {
            Debug.LogWarning("FAIL EVENT: Not enough credit!");
        }
        
    }

    private void OnFailUI()
    {
        gameData.isGameEnd=true;
        StartCoroutine(OpenFail());
    }

    private void ClearData(bool val)
    {
        gameData.isGameEnd=val;
    }

    private IEnumerator OpenSuccess()
    {
        yield return waitForSeconds;
        OpenSuccessPanel();
    }


    private void OpenSuccessPanel()
    {
        EventManager.Broadcast(GameEvent.OnSuccessUI);
    }

    private IEnumerator OpenFail()
    {
        yield return waitForSeconds;
        OpenFailPanel();
    }


    private void OpenFailPanel()
    {
        //effektif
        EventManager.Broadcast(GameEvent.OnFailUI);
    }



    
}
