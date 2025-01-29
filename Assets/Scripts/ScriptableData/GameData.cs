using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GameData", menuName = "Data/GameData", order = 0)]
public class GameData : ScriptableObject 
{

    public int score;
    public int increaseScore;
    public int levelIndex;
    public int levelNumber;

    //Probability
    public int ChanceOfCriticalHit;
    public int CriticalHitDamage;

    public TimerTypes timerTypes;


    //Player
    public int RoundedTime;
    public float maxTimerRange;


    public bool isGameEnd=false;
    public bool isStartTimer=false;

}
