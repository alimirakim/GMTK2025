using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Threading;
using Unity.VisualScripting;

public enum ClockState
{
    Paused,
    Active,
}

public class TimeKeeper : MonoBehaviour
{
    [Header("Day")]
    [SerializeField] int dayCount = 1;
    [SerializeField] TextMeshProUGUI dayCountText;

    [Header("Time")]
    [SerializeField] TextMeshProUGUI clockText;
    [SerializeField] DateTime currentTime;
    public ClockState clockState = ClockState.Active;
    [SerializeField] int minPerSecond = 1;
    [SerializeField] int fastForwardMinPerSecond = 10;


    [Header("Phase of Day")]
    [SerializeField] Image phaseOfDayImage;
    [SerializeField] Sprite phaseNoonSprite;
    [SerializeField] Sprite phaseEveningSprite;
    [SerializeField] Sprite phaseDuskSprite;
    [SerializeField] Sprite phaseNightSprite;
    [SerializeField] Sprite phaseLateNightSprite;

    [Header("Choice Timer")]
    [SerializeField] Image choiceTimerImage;
    bool isChoiceTimeActive = true;
    [SerializeField] float choiceTimeMax = 10f;
    float choiceTimeLeft;
    float fillFraction;

    PhaseOfDay currentPhase = PhaseOfDay.Noon;
    float fastForwardMinLeft;

    private float minuteTimer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTime = GetResetDayTime();
        dayCountText.text = $"Day {dayCount}";
        choiceTimeLeft = choiceTimeMax;
    }


    // Update is called once per frame
    void Update()
    {
        UpdateClock();
        UpdateChoiceTime();
    }

    public void EnableChoiceTime()
    { isChoiceTimeActive = true; }

    void UpdateChoiceTime()
    {
        if (isChoiceTimeActive)
        {
            choiceTimeLeft -= Time.deltaTime;

            if (choiceTimeLeft > 0)
            {
                fillFraction = choiceTimeLeft / choiceTimeMax;
            }
            else
            {
                isChoiceTimeActive = false;
                choiceTimeLeft = choiceTimeMax;
                fillFraction = 1f;
            }
            choiceTimerImage.fillAmount = fillFraction;
        }
    }

    private DateTime GetResetDayTime()
    { return new DateTime(1, 1, 1, 12, 0, 0); }

    void UpdateClock()
    {
        if (clockState == ClockState.Paused)
            return;

        minuteTimer += Time.deltaTime;

        // Speed time up x10 if there is time on the fast-forward tracker
        if (fastForwardMinLeft > 0)
        {
            if (minuteTimer > 0.1)
            {
                AddToClock(minPerSecond);
                minuteTimer -= 0.1f;
                fastForwardMinLeft -= 1;
                if (fastForwardMinLeft <= 0)
                {
                    clockState = ClockState.Paused;
                }
            }
        }
        else if (clockState == ClockState.Active)
        {
            if (minuteTimer > 1)
            {
                AddToClock(minPerSecond);
                minuteTimer -= 1;
            }
        }

        clockText.text = currentTime.ToShortTimeString();
    }

    public void FastForwardClockByMinutes(int minutes)
    {
        fastForwardMinLeft = minutes;
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
