using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using DG.Tweening;
using UnityEngine.UI;



public class Chronometer : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    private bool isHolding;
    private float holdStartTime;
    private float elapsedTime;

    public Image progressBarFill;
    [SerializeField] private TextMeshProUGUI timerText;
    


    private void Start()
    {
        OnNextLevel();
    }

    private void Update()
    {
        if (!gameData.isGameEnd && isHolding)
        {
            UpdateTimer();
        }
    }

    private void UpdateTimer()
    {
        elapsedTime = Time.time - holdStartTime;

        if (elapsedTime >= gameData.maxTimerRange)
        {
            elapsedTime = gameData.maxTimerRange; // Cap it
            Debug.LogWarning("Elapsed time exceeded maxTimerRange! Clamping to max value.");
        }

        float percentage = elapsedTime / gameData.maxTimerRange;
        gameData.RoundedTime = Mathf.Clamp(Mathf.RoundToInt(percentage * 100), 0, 100);

        // Format seconds and milliseconds (e.g., "15,67")
        string formattedTime = elapsedTime.ToString("F2").Replace('.', ',');
        timerText.SetText(formattedTime);

        // Update progress bar
        progressBarFill.fillAmount = percentage;
        progressBarFill.color = GetColorForProgress(percentage);
    }

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.AddHandler(GameEvent.OnStartTimer,OnStartTimer);
        EventManager.AddHandler(GameEvent.OnStopTimer,OnStopTimer);
        EventManager.AddHandler(GameEvent.OnRestartLevel,OnRestartLevel);
        EventManager.AddHandler(GameEvent.OnUpdateRounded,OnUpdateRounded);

    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.RemoveHandler(GameEvent.OnStartTimer,OnStartTimer);
        EventManager.RemoveHandler(GameEvent.OnStopTimer,OnStopTimer);
        EventManager.RemoveHandler(GameEvent.OnRestartLevel,OnRestartLevel);
        EventManager.RemoveHandler(GameEvent.OnUpdateRounded,OnUpdateRounded);

    }

    private void OnNextLevel()
    {
        gameData.RoundedTime=0;
        timerText.SetText(gameData.RoundedTime.ToString());
        //progressBarFill.fillAmount=0;
        /*isStop=true;
        gameData.RoundedTime=0;*/

    }

    private void OnRestartLevel()
    {
        gameData.RoundedTime=0;
        timerText.SetText(gameData.RoundedTime.ToString());
    }

   
   
    private void OnStartTimer()
    {   
        isHolding = true;
        holdStartTime = Time.time;
        elapsedTime = 0;
        gameData.RoundedTime = 0;
        timerText.SetText("0");
        progressBarFill.fillAmount = 0;
    }

    

    private void OnStopTimer()
    {
        isHolding = false;
        ScaleUP();
        gameData.Credit--;
        //EventManager.Broadcast(GameEvent.OnSetFeedback);
    }

    private void OnUpdateRounded()
    {
        ScaleUP();
        timerText.SetText(gameData.RoundedTime.ToString());

    }

    private void ScaleUP()
    {
        timerText.transform.DOScale(Vector3.one*1.5f,.25f).OnComplete(()=>timerText.transform.DOScale(Vector3.one,.25f));
    }

    private Color GetColorForProgress(float progress)
    {
        Color green = Color.green;
        Color yellow = Color.yellow;
        Color orange = new Color(1f, 0.5f, 0f);
        Color red = Color.red;

        if (progress < 0.33f) return Color.Lerp(green, yellow, progress / 0.33f);
        else if (progress < 0.66f) return Color.Lerp(yellow, orange, (progress - 0.33f) / 0.33f);
        else return Color.Lerp(orange, red, (progress - 0.66f) / 0.34f);
    }

}
