using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtons : MonoBehaviour
{
    [SerializeField] VerticalLayoutGroup actionButtonGroup;
    [SerializeField] UnityEngine.GameObject actionButtonPrefab;

    [SerializeField] Sprite successButtonSprite;
    [SerializeField] Sprite failedButtonSprite;
    [SerializeField] Sprite disabledButtonSprite;
    [SerializeField] GameManager gameManager;
    [SerializeField] TextMeshProUGUI actionInfoMessage;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // util method
    List<Transform> GetListOfGroupChildren(VerticalLayoutGroup layoutGroup)
    {
        List<Transform> children = new List<Transform>();

        for (int i = 0; i < layoutGroup.transform.childCount; ++i)
        {
            children.Add(layoutGroup.transform.GetChild(i));
        }

        return children;
    }

    public void PopulateActionButtons(int willpowerVal)
    {
        List<Transform> children = GetListOfGroupChildren(actionButtonGroup);

        foreach (Transform child in children)
        {
            Destroy(child.GameObject());
        }

        List<ActionSO> currentActions = gameManager.CurrentScenario.GetActionsAvailable();
        foreach (ActionSO action in currentActions)
        {
            string actionLabel = action.GetLabel();
            UnityEngine.GameObject newActionButton = Instantiate(actionButtonPrefab);
            newActionButton.transform.SetParent(actionButtonGroup.transform);

            newActionButton.AddComponent<Action>();
            Action actionComponent = newActionButton.GetComponent<Action>();
            actionComponent.action = action;
            actionComponent.infoMessage = actionInfoMessage;

            TextMeshProUGUI textComponent = newActionButton.GetComponentInChildren<TextMeshProUGUI>();
            textComponent.text = actionLabel;

            Button buttonComponent = newActionButton.GetComponent<Button>();
            buttonComponent.onClick.AddListener(() => OnClickAction(action, buttonComponent));

            // Disable button if lacking required WP
            if (willpowerVal < action.GetRequiredWillpower())
            {
                buttonComponent.interactable = false;
            }

        }
    }

    public void OnClickAction(ActionSO action, Button buttonComponent)
    {
        ScenarioSO currentScenario = gameManager.CurrentScenario;
        Willpower willpower = gameManager.WillpowerManager;
        TimeKeeper timeKeeper = gameManager.TimeKeeperManager;
        ToDoList toDoList = gameManager.ToDoListManager;

        // Deplete willpower
        willpower.UpdateWillpower(-action.GetRequiredWillpower());

        // Get attempt result
        AttemptResult result = action.GetAttemptResult();

        Debug.Log($"Attempt result: {result}");

        ActionSO executedAction = action;
        int timeForAction = 0;

        // Fast forward time based on result's action
        if (result == AttemptResult.Success)
        {
            toDoList.UpdateToDoList(result, executedAction);
            buttonComponent.GetComponent<UnityEngine.UI.GameObject>().sprite = successButtonSprite;
            timeForAction = executedAction.GetTimeDuration();
            if (action.GetLabel() == "Sleep")
            {
                willpower.UpdateWillpower(timeForAction / 5);
            }

            // add weight of type
        }
        else if (result == AttemptResult.PartialSuccess)
        {
            toDoList.UpdateToDoList(result, executedAction);
            buttonComponent.GetComponent<UnityEngine.UI.GameObject>().sprite = successButtonSprite;
            timeForAction = executedAction.GetTimeDuration();
        }
        else
        {
            buttonComponent.GetComponent<UnityEngine.UI.GameObject>().sprite = failedButtonSprite;
            executedAction = currentScenario.GetDefaultAction();

            // Adds willpower based on sleep
            timeForAction = executedAction.GetTimeDuration();
            Debug.Log($"Add points to wp {timeForAction / 5}, time for sleep {timeForAction}");
            willpower.UpdateWillpower(timeForAction / 5);
        }

        // pass time for action
        timeKeeper.FastForwardClockByMinutes(timeForAction);
        gameManager.DisplayResultsPanel(result, timeForAction, executedAction.GetLabel());
        // Apply action effects - willpower meter, shame size, message bubbles, mood meter and expression
    }


}
