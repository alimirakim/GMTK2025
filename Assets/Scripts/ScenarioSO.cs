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
    [SerializeField] List<ActionSO> actionsAvailable;
    [SerializeField] List<ToDoItemSO> toDoItems;

    public List<ActionSO> GetActionsAvailable()
    {
        return actionsAvailable;
    }

    public List<ToDoItemSO> GetToDoItems()
    {
        return toDoItems;
    }
}
