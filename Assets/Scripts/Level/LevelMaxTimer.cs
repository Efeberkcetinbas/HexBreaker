using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class LevelMaxTimer : MonoBehaviour
{
    [SerializeField] private TimerData timerData;
    [SerializeField] private GameData gameData;

    private int index;

    private void Start()
    {
        SetMaxTimer();
    }

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnStopTimer, OnStopTimer);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnStopTimer, OnStopTimer);
    }

    private void OnStopTimer()
    {
        SetMaxTimer();
    }

    private void SetMaxTimer()
    {
        gameData.maxTimerRange=timerData.TimerValues[index].MaxTimerValue;
        index++;

        if(index>timerData.TimerValues.Count)
            index=0;
    }
}
