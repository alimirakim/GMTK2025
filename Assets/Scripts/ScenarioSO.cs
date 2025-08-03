using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



[CreateAssetMenu(fileName = "ScenarioSO", menuName = "Scriptable Objects/ScenarioSO")]
public class ScenarioSO : ScriptableObject
{
    public int timeOfDay;
    [SerializeField] ActionSO defaultAction;
    [SerializeField] List<ActionSO> actionsAvailable;
    [SerializeField] List<ToDoItemSO> toDoItems;

    public ActionSO GetDefaultAction() => defaultAction;

    public List<ActionSO> GetActionsAvailable() => actionsAvailable;

    public List<ToDoItemSO> GetToDoItems() => toDoItems;

}
