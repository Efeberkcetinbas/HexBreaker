using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GameData", menuName = "Data/GameData", order = 0)]
public class GameData : ScriptableObject 
{   
    //Game Management
    public int score;
    public int increaseScore;
    public int levelIndex;
    public int levelNumber;

    //Probability
    public int ChanceOfCriticalHit;
    public int CriticalHitDamage;


    //Level
    public int LevelHexParentNumber;
    public int Credit;

    //Weapon
    public int WeaponIndex;
    public int WeaponBoosterAmount;

    //Bullet
    public int BulletDamageAmount;


    //Player
    public int RoundedTime;
    public float maxTimerRange;


    //Chronometer
    public bool isGameEnd=false;
    public bool canTouch=false;
    public bool isStartTimer=false;

}
