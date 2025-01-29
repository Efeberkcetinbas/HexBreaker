using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboSystem : MonoBehaviour
{
    [SerializeField] private GameData gameData;

    private int counter;

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnCheckCombo,OnCheckCombo);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnCheckCombo,OnCheckCombo);
    }

    private void OnCheckCombo()
    {
        if(gameData.RoundedTime==(int)gameData.maxTimerRange)
        {
            counter++;
            Debug.Log("COMBO x" + counter);
        }
        else
        {
            counter=0;
        }
    }
}
