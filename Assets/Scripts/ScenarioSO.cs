using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum ShameType
{
    Burden,
    Failure,
    Disgusting,
    Lazy,
    Weak,
    Ungrateful,
    Broken
}

[CreateAssetMenu(fileName = "ScenarioSO", menuName = "Scriptable Objects/ScenarioSO")]
public class ScenarioSO : ScriptableObject
{
    [SerializeField] int timeOfDay;
    [SerializeField] ActionSO defaultAction;
    [SerializeField] List<ActionSO> actionsAvailable;
    [SerializeField] List<ToDoItemSO> toDoItems;

    public ActionSO GetDefaultAction() => defaultAction;

    public List<ActionSO> GetActionsAvailable() => actionsAvailable;

    public List<ToDoItemSO> GetToDoItems() => toDoItems;
    
}
