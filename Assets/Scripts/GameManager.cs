using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;



public class GameManager : MonoBehaviour
{

    [field: SerializeField] public TimeKeeper TimeKeeperManager { get; private set; }
    [field: SerializeField] public Willpower WillpowerManager { get; private set; }
    [field: SerializeField] public Weight WeightManager { get; private set; }
    [field: SerializeField] public ToDoList ToDoListManager { get; private set; }
    [field: SerializeField] public ActionButtons ActionButtonsManager { get; private set; }
    // [SerializeField] 
    // [SerializeField] Mood mood;


    [Header("Scenarios")]
    [field: SerializeField] public ScenarioSO CurrentScenario { get; private set; }
    [SerializeField] ScenarioSO noonScenario;
    [SerializeField] ScenarioSO eveningScenario;
    [SerializeField] ScenarioSO duskScenario;
    [SerializeField] ScenarioSO nightScenario;
    [SerializeField] ScenarioSO lateNightScenario;

    [Header("Results Panel")]
    [SerializeField] GameObject resultsPanel;
    [SerializeField] TextMeshProUGUI resultsMessage;
    [SerializeField] Button confirmResultsButton;


    // TODO results script
    // TODO action script?


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        ToDoListManager.PopulateToDoList();
        ActionButtonsManager.PopulateActionButtons(WillpowerManager.GetVal());
    }

    // Update is called once per frame
    void Update()
    {
    }

    void StartChoiceSession()
    {
        bool isNewPhase = UpdateCurrentSceneIfChanged();
        if (isNewPhase)
        {
            ToDoListManager.PopulateToDoList();
            ActionButtonsManager.PopulateActionButtons(WillpowerManager.GetVal());
        }
        TimeKeeperManager.RestartChoiceTime();
        TimeKeeperManager.clockState = ClockState.Active;
    }

    bool UpdateCurrentSceneIfChanged()
    {

        ScenarioSO newCurrentScenario = TimeKeeperManager.GetCurrentPhaseOfDay() switch
        {
            PhaseOfDay.Noon => noonScenario,
            PhaseOfDay.Evening => eveningScenario,
            PhaseOfDay.Dusk => duskScenario,
            PhaseOfDay.Night => nightScenario,
            _ => lateNightScenario,
        };

        // Update current scene if changed and return true
        if (CurrentScenario != newCurrentScenario)
        {
            CurrentScenario = newCurrentScenario;
            return true;
        }

        // Change nothing and return false
        return false;
    }

    public void DisplayResultsPanel(AttemptResult result, int minPassed, string actionLabel)
    {
        resultsPanel.SetActive(true);
        confirmResultsButton.interactable = false;
        Debug.Log($"is the button usable {confirmResultsButton.interactable}");

        resultsMessage.text = result switch
        {
            AttemptResult.Success => $"Success! You spent {minPassed} minutes to {actionLabel}.",
            AttemptResult.PartialSuccess => $"You tried! You spent {minPassed} minutes attempting to {actionLabel}.",
            _ => $"You failed. You gave up and spent {minPassed} minutes to {actionLabel}.",
        };

        confirmResultsButton.interactable = true;

        // TODO Pause confirmButton until fast forward is finished.
        // coroutines, event listeners, async await??
        // UnityEvent clockPausedEvent = new UnityEvent();
        // clockPausedEvent.AddListener(() => confirmResultsButton.interactable = true);
        // timeKeeper.InvokeGivenEvent(clockPausedEvent);

    }

    public void OnClickConfirmButton()
    {
        resultsPanel.SetActive(false);
        StartChoiceSession();
    }
}