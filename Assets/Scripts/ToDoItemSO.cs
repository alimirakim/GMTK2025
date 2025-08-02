using UnityEngine;

[CreateAssetMenu(fileName = "ToDoItemSO", menuName = "Scriptable Objects/ToDoItemSO")]
public class ToDoItemSO : ScriptableObject
{
    [SerializeField] bool isChecked;
    [SerializeField] ActionSO toDoAction;
    
}
