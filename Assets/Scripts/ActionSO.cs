using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "ActionSO", menuName = "Scriptable Objects/ActionSO")]
public class ActionSO : ScriptableObject
{
    [SerializeField] string label;
    [SerializeField] public float baseSuccessChance;
    [SerializeField] public int requiredWillpower;
    [SerializeField] public int minMinutesDuration;
    [SerializeField] public int maxMinutesDuration;
    // [SerializeField] int consecutiveSuccesses;
    // [SerializeField] int consecutiveFailures; // can success chance change with consecutive failures? 'urgency' meter? or separate secret meters like hygiene

    [SerializeField] List<string> considerActionMessages;
    // [SerializeField] MoodType moodType;
    [SerializeField] ShameType shameType;

    [Header("Success Effects")]
    [SerializeField] int successWillpowerEffect;
    [SerializeField] int successMoodEffect;
    [SerializeField] int successWeightEffect;
    [SerializeField] string successMessage;

    [Header("Partial Success Effects")]
    [SerializeField] int partialSuccessWillpowerEffect;
    // [SerializeField] int failureWillpowerEffect; 
    [SerializeField] int partialSuccessMoodEffect;
    [SerializeField] int partialSuccessWeightEffect;
    [SerializeField] string partialSuccessMessage;

    [Header("Failure Effects")]
    [SerializeField] int failureMoodEffect;
    [SerializeField] int failureWeightEffect;
    [SerializeField] List<string> failureMessages;

    public string GetLabel() => label;

    public int GetRequiredWillpower() => requiredWillpower;

    public AttemptResult GetAttemptResult()
    {
        float attemptVal = Random.Range(0f, 1f);
        if (attemptVal <= baseSuccessChance)
        {
            return attemptVal <= baseSuccessChance / 2 ? AttemptResult.Success : AttemptResult.PartialSuccess;
        }
        return AttemptResult.Failure;
    }

    public int GetTimeDuration() => Random.Range(minMinutesDuration, maxMinutesDuration + 1);

    public void OnClick()
    {
        // deplete willpower
        // calculate success or fail
        // if fail, 
        //      change to fallback action
        //      add to fail tally for action
        //      calculate shame / failure stats
        // calculate time passed
        // fast-forward time
        // display result message


    }
}
