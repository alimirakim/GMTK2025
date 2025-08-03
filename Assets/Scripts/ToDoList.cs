using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ToDoList : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] VerticalLayoutGroup toDoCheckboxGroup;
    [SerializeField] GameObject toDoCheckboxPrefab;
    [SerializeField] int rectTransformHeight = 50;

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

    public void PopulateToDoList()
    {
        List<Transform> children = GetListOfGroupChildren(toDoCheckboxGroup);

        foreach (Transform child in children)
        {
            Destroy(child.GameObject());
        }

        Debug.Log($"populating now... currentScenario {gameManager.CurrentScenario}");
        Debug.Log($"testing {gameManager.CurrentScenario.timeOfDay}");


        List<ToDoItemSO> currentToDoItems = gameManager.CurrentScenario.GetToDoItems();

        foreach (ToDoItemSO toDoItem in currentToDoItems)
        {
            string toDoLabel = toDoItem.GetToDoAction().GetLabel();
            // Debug.Log($"toDoLabel {toDoLabel}");
            GameObject toDoCheckbox = Instantiate(toDoCheckboxPrefab, toDoCheckboxGroup.GetComponent<Transform>(), false);
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

    public void UpdateToDoList(AttemptResult result, ActionSO action)
    {
        List<Transform> toDoCheckboxes = GetListOfGroupChildren(toDoCheckboxGroup);
        foreach (Transform toDoCheckbox in toDoCheckboxes)
        {
            Text textComponent = toDoCheckbox.GetComponentInChildren<Text>();
            Debug.Log($"updating to-do list result {result} action {action.GetLabel()} textComp {textComponent.text}");
            if (textComponent.text == action.GetLabel())
            {
                toDoCheckbox.GetComponent<Toggle>().isOn = true;

                if (result == AttemptResult.PartialSuccess)
                    // TODO change checkbox mark to a slash
                    textComponent.text += " (kind of)";
                break;
            }
        }
    }
}
