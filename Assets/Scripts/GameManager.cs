using System;
using System.Collections.Generic;
using System.Threading;
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
    [SerializeField] DateTime currentTime = new DateTime(0, 1, 1, 12, 0, 0);
    [SerializeField] float decisionMaxTime = 30;
    private float minuteTimer = 0;
    private float decisionTimer = 30;

    [SerializeField] ScenarioSO noonScenario;
    [SerializeField] ScenarioSO eveningScenario;
    [SerializeField] ScenarioSO duskScenario;
    [SerializeField] ScenarioSO nightScenario;
    [SerializeField] ScenarioSO lateNightScenario;
    [SerializeField] VerticalLayoutGroup actionButtons;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // set current time, phaseofday
        // start running clock and update
        // populate to-do list
        // populate action buttons
        // 


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
        // UpdateClock();
    }

    void PopulateActionButtons()
    {
        GameObject[] currentActionButtons = actionButtons.GetComponentsInChildren<GameObject>();

        for (int i = 0; i < currentActionButtons.Length; i++)
        {
            Destroy(gameObject);
        }

        PhaseOfDay currentPhase = GetCurrentPhaseOfDay();
        ScenarioSO currentScenario = currentPhase switch
        {
            PhaseOfDay.Noon => noonScenario,
            PhaseOfDay.Evening => eveningScenario,
            PhaseOfDay.Dusk => duskScenario,
            PhaseOfDay.Night => nightScenario,
            _ => lateNightScenario,
        };

        foreach (ActionSO action in currentScenario.GetActionsAvailable())
        {
            // create prefab button and add to currentActionButtons
        }

        foreach (ToDoItemSO toDoItem in currentScenario.GetToDoItems())
        {
            // create todo prefab toggle and add to currentToDoItems
        }

    }

    PhaseOfDay GetCurrentPhaseOfDay()
    {
        if (currentTime.Hour >= 12 && currentTime.Hour < 15)
        { return PhaseOfDay.Noon; }
        else if (currentTime.Hour >= 15 && currentTime.Hour < 18)
        { return PhaseOfDay.Evening; }
        else if (currentTime.Hour >= 18 && currentTime.Hour < 21)
        { return PhaseOfDay.Dusk; }
        else if (currentTime.Hour >= 21 && currentTime.Hour < 24)
        { return PhaseOfDay.Night; }
        else
        { return PhaseOfDay.LateNight; }
    }


    void UpdateClock()
    {
        minuteTimer += Time.deltaTime;
        if (minuteTimer > 1)
        {
            currentTime.AddMinutes(1);
        }
        minuteTimer -= 1;

        Debug.Log($"Time: {currentTime.ToShortTimeString()}");
    }

    public void AddToClock(int minutes)
    {
        currentTime.AddMinutes(minutes);
    }

    bool DecisionTimer()
    {
        decisionTimer -= Time.deltaTime;
        if (decisionTimer <= 0)
        {
            decisionTimer = decisionMaxTime;
            return true;
        }
        return false;
    }

    void StartDecisionTimer()
    {
        decisionTimer = decisionMaxTime;

    }
}
