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

    [SerializeField] TimeKeeper timeKeeper;
    [SerializeField] Willpower willpower;
    [SerializeField] Weight weight;
    [SerializeField] ToDoList toDoList;
    [SerializeField] ActionButtons actionButtons;
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
    public bool isResultsPanelActive = false;
    [SerializeField] GameObject resultsPanel;
    [SerializeField] TextMeshProUGUI resultsMessage;
    [SerializeField] Button confirmResultsButton;


    // TODO results script
    // TODO action script?


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        toDoList.PopulateToDoList();
        actionButtons.PopulateActionButtons(willpower.GetVal());
    }

    // Update is called once per frame
    void Update()
    {
    }

    // public ScenarioSO GetCurrentScenario() => currentScenario;
    // public Willpower GetWillPower() => willpower;
    // public TimeKeeper GetTimeKeeper() => timeKeeper;
    // public ToDoList GetToDoList() => toDoList;

    // public ScenarioSO CurrentScenario { get; set; }
    // public ScenarioSO GetCurrentScensario() => currentScenario
    public Willpower WillpowerManager { get; set; }
    public TimeKeeper TimeKeeperManager { get; set; }
    public ToDoList ToDoListManager { get; set; }
    public Weight WeightManager { get; set; }

    void StartChoiceSession()
    {
        bool isNewPhase = UpdateCurrentSceneIfChanged();
        if (isNewPhase)
        {
            toDoList.PopulateToDoList();
            actionButtons.PopulateActionButtons(willpower.GetVal());
        }
        timeKeeper.RestartChoiceTime();
        timeKeeper.clockState = ClockState.Active;
    }

    bool UpdateCurrentSceneIfChanged()
    {

        ScenarioSO newCurrentScenario = timeKeeper.GetCurrentPhaseOfDay() switch
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