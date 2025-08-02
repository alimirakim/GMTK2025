using System.Collections.Generic;
using UnityEngine;

enum MoodType // deterioration rate
{
    Boredom,
    Loneliness,
    Breath,
    Hygiene,
    Atmosphere,
    Hunger,
}

[CreateAssetMenu(fileName = "ActionSO", menuName = "Scriptable Objects/ActionSO")]
public class ActionSO : ScriptableObject
{
    [SerializeField] string label;
    [SerializeField] float baseSuccessChance;
    [SerializeField] int requiredWillpower; 
    [SerializeField] int minMinutesDuration;
    [SerializeField] int maxMinutesDuration;
    // [SerializeField] int consecutiveSuccesses;
    [SerializeField] int consecutiveFailures; // can success chance change with consecutive failures? 'urgency' meter? or separate secret meters like hygiene

    [Header("Mood")]
    // [SerializeField] MoodType moodType;
    [SerializeField] int successMoodEffect;
    [SerializeField] int partialSuccessMoodEffect;
    [SerializeField] int failureMoodEffect;

    [Header("Willpower")]
    [SerializeField] int successWillpowerEffect;
    [SerializeField] int partialSuccessWillpowerEffect;
    // [SerializeField] int failureWillpowerEffect; 

    [Header("Shame")]
    [SerializeField] ShameType shameType;
    [SerializeField] int successShameEffect;
    [SerializeField] int partialSuccessShameEffect;
    [SerializeField] int failureShameEffect;

    [Header("Result Messages")]
    [SerializeField] List<string> considerActionMessages;
    [SerializeField] string successMessage;
    [SerializeField] string partialSuccessMessage;
    [SerializeField] List<string> failureMessages;
}
