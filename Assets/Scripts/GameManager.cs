using System;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

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


    [SerializeField] ScenarioSO currentScenario;
    [SerializeField] ScenarioSO noonScenario;
    [SerializeField] ScenarioSO eveningScenario;
    [SerializeField] ScenarioSO duskScenario;
    [SerializeField] ScenarioSO nightScenario;
    [SerializeField] ScenarioSO lateNightScenario;

    [SerializeField] VerticalLayoutGroup toDoCheckboxes;
    [SerializeField] GameObject toDoCheckboxPrefab;
    [SerializeField] VerticalLayoutGroup actionButtons;
    [SerializeField] GameObject actionButtonPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        // populate to-do list
        Debug.Log("starting game manager...");
        PopulateToDoList();
        PopulateActionButtons();
        // populate action buttons
        // PopulateActionButtons();


        // start decision time limit - 30 minute aka 30 second timer
        // grab toDo list and actions list based on phase of day
        // select action - check for success, pass time based on action
        // check item if completed
        // change weight size based on action
        // change willpower dial

    }

    // Update is called once per frame
    void Update()
    {
    }

    void PopulateToDoList()
    {
        List<Transform> children = new List<Transform>();

        for (int i = 0; i < toDoCheckboxes.transform.childCount; ++i)
        {
            children.Add(toDoCheckboxes.transform.GetChild(i));
        }
        Debug.Log($"length {toDoCheckboxes.transform.childCount} children length {children.Count}");

        foreach (Transform child in children)
        {
            Debug.Log($"{child.GetComponentInChildren<Text>().text}");
            Destroy(child.GameObject());
        }

        List<ToDoItemSO> currentToDoItems = currentScenario.GetToDoItems();
        foreach (ToDoItemSO toDoItem in currentToDoItems)
        {
            string toDoLabel = toDoItem.GetToDoAction().GetLabel();
            Debug.Log($"toDoLabel {toDoLabel}");
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

            // Attach onClick event
            // reset choiceTimer clock, show action results, fast forward clock based on action
            // Apply action effects - willpower meter, shame size, message bubbles, mood meter and expression
            // wait for confirmation before continuing to next decision point
        }

    }
}