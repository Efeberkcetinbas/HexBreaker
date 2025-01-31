using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> successParticles=new List<ParticleSystem>();
    [SerializeField] private ParticleSystem[] feedbackParticles; // Assign in Inspector
    [SerializeField] private GameData gameData;
    private static readonly int[] thresholds = { 20, 60, 75, 90, 100 }; // Thresholds for each level


    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnSuccess,OnSuccess);
        EventManager.AddHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.AddHandler(GameEvent.OnSetFeedback,OnSetFeedback);

    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnSuccess,OnSuccess);
        EventManager.RemoveHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.AddHandler(GameEvent.OnSetFeedback,OnSetFeedback);

    }

    private void OnSuccess()
    {
        OpenClose(true);

        for (int i = 0; i < successParticles.Count; i++)
        {
            successParticles[i].Play();
        }
    }

    private void OpenClose(bool val)
    {
        for (int i = 0; i < successParticles.Count; i++)
        {
            successParticles[i].gameObject.SetActive(val);
        }
    }

    private void OnSetFeedback()
    {
        PlayFeedbackParticle();
    }

    private void PlayFeedbackParticle()
    {
        int index = Array.FindIndex(thresholds, t => gameData.RoundedTime < t);

        if (index == -1) index = feedbackParticles.Length - 1; // If RoundedTime >= 100, use last particle

        feedbackParticles[index].Play(); // Play selected particle
    }
    private void OnNextLevel()
    {
        OpenClose(false);
    }

}
