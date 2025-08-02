using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;


public class TimeKeeper : MonoBehaviour
{
    float timerValue = 3;
    [SerializeField] int dayCount = 1;
    [SerializeField] TextMeshProUGUI dayCountText;
    [SerializeField] TextMeshProUGUI clockText;
    [SerializeField] DateTime currentTime;
    [SerializeField] Image phaseOfDayImage;
    [SerializeField] Sprite phaseNoonSprite;
    [SerializeField] Sprite phaseEveningSprite;
    [SerializeField] Sprite phaseDuskSprite;
    [SerializeField] Sprite phaseNightSprite;
    [SerializeField] Sprite phaseLateNightSprite;

    PhaseOfDay currentPhase = PhaseOfDay.Noon;

    private float minuteTimer = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTime = GetResetDayTime();
        dayCountText.text = $"Day {dayCount}";
    }

    // Update is called once per frame
    void Update()
    {
        UpdateClock();

    }

    private DateTime GetResetDayTime()
    { return new DateTime(1, 1, 1, 12, 0, 0); }

    void UpdateClock()
    {
        minuteTimer += Time.deltaTime;
        if (minuteTimer > 1)
        {
            AddToClock(10);
            minuteTimer -= 1;
        }

        clockText.text = currentTime.ToShortTimeString();

    }

    void IncrementDayCount()
    {
        dayCount++;
        dayCountText.text = $"Day {dayCount}";
    }

    public void AddToClock(int minutes)
    {
        currentTime = currentTime.AddMinutes(minutes);

        if (currentPhase != GetCurrentPhaseOfDay())
        {
            UpdatePhaseOfDay();
        }

        if (currentTime.Day > 1 && currentTime.Hour > 3)
            {
                IncrementDayCount();
                currentTime = GetResetDayTime();
            }
    }

    public PhaseOfDay GetCurrentPhaseOfDay()
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

    void UpdatePhaseOfDay()
    {
        currentPhase = GetCurrentPhaseOfDay();

        switch (currentPhase)
        {
            case PhaseOfDay.Noon:
                phaseOfDayImage.sprite = phaseNoonSprite;
                break;

            case PhaseOfDay.Evening:
                phaseOfDayImage.sprite = phaseEveningSprite;
                break;

            case PhaseOfDay.Dusk:
                phaseOfDayImage.sprite = phaseDuskSprite;
                break;

            case PhaseOfDay.Night:
                phaseOfDayImage.sprite = phaseNightSprite;
                break;

            case PhaseOfDay.LateNight:
                phaseOfDayImage.sprite = phaseLateNightSprite;
                break;
        }
    }

}
