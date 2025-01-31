using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TimerValues
{
    public float MaxTimerValue;
}

[CreateAssetMenu(fileName = "LevelTimerData", menuName = "MaxTimer/LevelTimerData", order = 0)]
public class TimerData : ScriptableObject
{
    public List<TimerValues> TimerValues;
}
