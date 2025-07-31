using UnityEngine;
using TMPro;

public class TimeKeeper : MonoBehaviour
{
    [SerializeField] int dayCount = 1;
    [SerializeField] TextMeshProUGUI dayCountText;
    float timerValue = 3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dayCountText.text = $"Day {dayCount}";
    }

    // Update is called once per frame
    void Update()
    {

        timerValue -= Time.deltaTime;
        if (timerValue <= 0)
        {
            IncrementDayCount();
            timerValue = 3;
        }
        // Debug.Log($"timerValue = {timerValue}, dayCount {dayCount}");

    }

    public void IncrementDayCount()
    {
        dayCount++;
        dayCountText.text = $"Day {dayCount}";
    }
}
