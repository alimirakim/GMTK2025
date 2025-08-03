using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



[CreateAssetMenu(fileName = "ScenarioSO", menuName = "Scriptable Objects/ScenarioSO")]
public class ScenarioSO : ScriptableObject
{
    public int timeOfDay = 42;
    [SerializeField] ActionSO defaultAction;
    [SerializeField] List<ActionSO> actionsAvailable;
    [SerializeField] List<ToDoItemSO> toDoItems;

    public void TestyTime() => Debug.Log("yo we're in!");
    public ActionSO GetDefaultAction() => defaultAction;

    public List<ActionSO> GetActionsAvailable() => actionsAvailable;

    public List<ToDoItemSO> GetToDoItems() => toDoItems;

}
