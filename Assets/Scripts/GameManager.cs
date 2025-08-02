using System;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum PhaseOfDay
{
    Noon,
    Evening,
    Dusk,
    Night,
    LateNight
}

public class GameManager : MonoBehaviour
{

    [SerializeField] TimeKeeper timeKeeper;
    [SerializeField] Willpower willpower;
    [SerializeField] Weight weight;
    // [SerializeField] Mood mood;


    [Header("Scenarios")]
    [SerializeField] ScenarioSO currentScenario;
    [SerializeField] ScenarioSO noonScenario;
    [SerializeField] ScenarioSO eveningScenario;
    [SerializeField] ScenarioSO duskScenario;
    [SerializeField] ScenarioSO nightScenario;
    [SerializeField] ScenarioSO lateNightScenario;

    [Header("To-Do List")]
    [SerializeField] VerticalLayoutGroup toDoCheckboxes;
    [SerializeField] GameObject toDoCheckboxPrefab;

    [Header("Action Choices")]
    [SerializeField] VerticalLayoutGroup actionButtons;
    [SerializeField] GameObject actionButtonPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PopulateToDoList();
        PopulateActionButtons();
        // StartChoiceScenario();
        // select action - check for success, pass time based on action
        // check item if completed
        // change weight size based on action
        // change willpower dial

    }

    // Update is called once per frame
    void Update()
    {
    }

    void StartChoiceScenario()
    {
        bool isNewPhase = UpdateCurrentSceneIfChanged();
        if (isNewPhase)
        {
            PopulateToDoList();
            PopulateActionButtons();
        }
        timeKeeper.EnableChoiceTime();
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
        if (currentScenario != newCurrentScenario)
        {
            currentScenario = newCurrentScenario;
            return true;
        }

        // Change nothing and return false
        return false;
    }

    void PopulateToDoList()
    {
        List<Transform> children = new List<Transform>();

        for (int i = 0; i < toDoCheckboxes.transform.childCount; ++i)
        {
            children.Add(toDoCheckboxes.transform.GetChild(i));
        }

        foreach (Transform child in children)
        {
            Destroy(child.GameObject());
        }

        List<ToDoItemSO> currentToDoItems = currentScenario.GetToDoItems();

        foreach (ToDoItemSO toDoItem in currentToDoItems)
        {
            string toDoLabel = toDoItem.GetToDoAction().GetLabel();
            // Debug.Log($"toDoLabel {toDoLabel}");
            GameObject newToDoCheckbox = Instantiate(toDoCheckboxPrefab);
            newToDoCheckbox.transform.SetParent(toDoCheckboxes.transform);
            newToDoCheckbox.GetComponent<UnityEngine.UI.Toggle>().isOn = false;
            Text textComponent = newToDoCheckbox.GetComponentInChildren<Text>();
            // TODO Even though the text is changes in the editor panel, it doesn't appear in scene
            textComponent.text = toDoLabel;
        }
    }

    void PopulateActionButtons()
    {
        List<Transform> children = new List<Transform>();

        for (int i = 0; i < actionButtons.transform.childCount; ++i)
        {
            children.Add(actionButtons.transform.GetChild(i));
        }

        foreach (Transform child in children)
        {
            Destroy(child.GameObject());
        }

        List<ActionSO> currentActions = currentScenario.GetActionsAvailable();
        foreach (ActionSO action in currentActions)
        {
            string actionLabel = action.GetLabel();
            GameObject newActionButton = Instantiate(actionButtonPrefab);
            newActionButton.transform.SetParent(actionButtons.transform);
            TextMeshProUGUI textComponent = newActionButton.GetComponentInChildren<TextMeshProUGUI>();
            textComponent.text = actionLabel;

            Button buttonComponent = newActionButton.GetComponent<Button>();
            AddOnClickToActionButton(action, buttonComponent);

            // Disable button if lacking required WP
            if (willpower.GetVal() < action.GetRequiredWillpower())
            {
                buttonComponent.interactable = false;
            }


            // reset choiceTimer clock, show action results, fast forward clock based on action
            // Apply action effects - willpower meter, shame size, message bubbles, mood meter and expression
            // wait for confirmation before continuing to next decision point
        }
    }

    public void BlahTest()
    {
        Debug.Log("am I in?");
    }

    public void AddOnClickToActionButton(ActionSO action, UnityEngine.UI.Button buttonComponent)
    {
        Debug.Log($"adding on Click {buttonComponent}");
        buttonComponent.onClick.AddListener(() =>
        {
            Debug.Log("in the clicky!");
            // Deplete willpower
            willpower.UpdateWillpower(-action.GetRequiredWillpower());

            // Get attempt result
            AttemptResult result = action.GetAttemptResult();

            Debug.Log($"Attempt result: {result}");

            // Skip time based on result's action
            if (result != AttemptResult.Failure)
            {
                int timeForAction = action.GetTimeDuration();
                Debug.Log($"time for action: {timeForAction}");
                timeKeeper.AddToClock(timeForAction);
            }
            else
            {
                // TODO Mark or partial-mark to-do checkbox if action is on list

                action = currentScenario.GetDefaultAction();
                int timeForAction = action.GetTimeDuration();
                Debug.Log($"time for action: {timeForAction}");
                timeKeeper.AddToClock(timeForAction);
            }
        });
    }
}