using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Action : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ActionSO action;
    public TextMeshProUGUI infoMessage;

    public void OnPointerEnter(PointerEventData eventData)
    {

        string successChance = $"Success Chance: {action.baseSuccessChance * 100}%";
        string willpowerNeeded = $"Required Willpower: {action.requiredWillpower}";
        string timeEstimate = $"Duration: {action.minMinutesDuration}-{action.maxMinutesDuration} minutes.";

        infoMessage.text = $"{action.GetLabel()}: \n{successChance} \n{willpowerNeeded} \n{timeEstimate}";
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        infoMessage.text = "";
    }
    
}
