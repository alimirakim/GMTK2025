using System;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
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
    [SerializeField] VerticalLayoutGroup toDoCheckboxGroup;
    [SerializeField] GameObject toDoCheckboxPrefab;
    [SerializeField] int rectTransformHeight = 50;

    [Header("Action Choices")]
    [SerializeField] VerticalLayoutGroup actionButtonGroup;
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
        List<Transform> children = GetListOfGroupChildren(toDoCheckboxGroup);

        foreach (Transform child in children)
        {
            Destroy(child.GameObject());
        }

        List<ToDoItemSO> currentToDoItems = currentScenario.GetToDoItems();

        foreach (ToDoItemSO toDoItem in currentToDoItems)
        {
            string toDoLabel = toDoItem.GetToDoAction().GetLabel();
            // Debug.Log($"toDoLabel {toDoLabel}");
            GameObject toDoCheckbox = Instantiate(toDoCheckboxPrefab);
            toDoCheckbox.transform.SetParent(toDoCheckboxGroup.transform);

            // Add some height to let the label render properly
            RectTransform rectTransform = toDoCheckbox.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransformHeight);

            Toggle toggle = toDoCheckbox.GetComponent<Toggle>();
            toggle.isOn = false;
            toggle.interactable = false;

            Text textComponent = toDoCheckbox.GetComponentInChildren<Text>();
            textComponent.text = toDoLabel;
        }
    }

    List<Transform> GetListOfGroupChildren(VerticalLayoutGroup layoutGroup)
    {
        List<Transform> children = new List<Transform>();

        for (int i = 0; i < layoutGroup.transform.childCount; ++i)
        {
            children.Add(layoutGroup.transform.GetChild(i));
        }

        return children;
    }

    void PopulateActionButtons()
    {
        List<Transform> children = GetListOfGroupChildren(actionButtonGroup);

        foreach (Transform child in children)
        {
            Destroy(child.GameObject());
        }

        List<ActionSO> currentActions = currentScenario.GetActionsAvailable();
        foreach (ActionSO action in currentActions)
        {
            string actionLabel = action.GetLabel();
            GameObject newActionButton = Instantiate(actionButtonPrefab);
            newActionButton.transform.SetParent(actionButtonGroup.transform);

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


    void UpdateToDoList(AttemptResult result, ActionSO action)
    {
        List<Transform> toDoCheckboxes = GetListOfGroupChildren(toDoCheckboxGroup);
        foreach (Transform toDoCheckbox in toDoCheckboxes)
        {
            if (toDoCheckbox.GetComponent<Toggle>().isOn == true)
                break;

            Text textComponent = toDoCheckbox.GetComponentInChildren<Text>();
            if (textComponent.text == action.GetLabel())
            {
                toDoCheckbox.GetComponent<Toggle>().isOn = true;

                if (result == AttemptResult.PartialSuccess)
                    textComponent.text += " (kind of)";
                break;
            }
        }
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

            // Fast forward time based on result's action
            if (result != AttemptResult.Failure)
            {
                UpdateToDoList(result, action);
                PassTimeForAction(action);
            }
            else
            {
                action = currentScenario.GetDefaultAction();
                PassTimeForAction(action);
            }
        });
    }

    void PassTimeForAction(ActionSO action)
    {
        int timeForAction = action.GetTimeDuration();
        Debug.Log($"time for action: {timeForAction}");

        // pause realtime clock
        // speed through clock x3whatever speed
        // pause clock
        // show message "Success! You spent x hours/minutes to [action]"
        // "You failed to [x]. You spent x hours/minutes to [action] instead"
        // highlight successful actions with green
        // highlight failed actions with red?
        timeKeeper.FastForwardClockByMinutes(timeForAction);
    }
}