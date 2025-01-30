using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private float screePercentageToExclude=20f;
    
    void Start()
    {
        gameData.isStartTimer=true;
    }

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnPlayerStartMove, OnPlayerStartMove);
        EventManager.AddHandler(GameEvent.OnPlayerStopMove, OnPlayerStopMove);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnPlayerStartMove, OnPlayerStartMove);
        EventManager.RemoveHandler(GameEvent.OnPlayerStopMove, OnPlayerStopMove);
    }

    void Update()
    {
        if(!gameData.isGameEnd && gameData.canTouch)
            CheckStartStop();
    }

    private void OnPlayerStartMove()
    {
        gameData.canTouch=true;
    }

    private void OnPlayerStopMove()
    {
        gameData.canTouch=false;
    }

    

    private void CheckStartStop()
    {
        if(Input.touchCount>0)
        {
            Touch touch=Input.GetTouch(0);
            if(touch.phase==TouchPhase.Began && touch.position.y<(Screen.height*(1-screePercentageToExclude/100)))
            {
                gameData.isStartTimer=!gameData.isStartTimer;
                SendStartStop();
            }
            
        }
    }

    private void SendStartStop()
    {
        if(gameData.isStartTimer)
        {
            EventManager.Broadcast(GameEvent.OnStartTimer);
        }
            
        else
        {
            EventManager.Broadcast(GameEvent.OnStopTimer);

        }
            
    }

}
