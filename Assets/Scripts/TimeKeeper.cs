using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine.Events;
using System.Threading.Tasks;
using System.Collections;



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
    [SerializeField] Image phaseOfDayBigImage;
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

    [Header("Sleep Panel")]
    [SerializeField] GameObject darkScreen;
    [SerializeField] GameObject sleepPanel;
    [SerializeField] Button continueButton;

    PhaseOfDay currentPhase = PhaseOfDay.Noon;
    float fastForwardMinLeft;

    private float minuteTimer = 0f;
    public bool isFastForward = false;
    public bool isNewDay = false;

    public float GetFastForwardMinLeft() => fastForwardMinLeft;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTime = GetResetDayTime();
        dayCountText.text = $"Day {dayCount}";
        choiceTimeLeft = choiceTimeMax;
        // StartCoroutine(ShowNewPhaseImageCoroutine());

    }


    // Update is called once per frame
    void Update()
    {
        UpdateClock();
        UpdateChoiceTime();
    }

  
    // TODO Skips over sleep phase

    IEnumerator ShowNewPhaseImageCoroutine()
    {
        Debug.Log("Started Coroutine at timestamp : " + Time.time);
        phaseOfDayBigImage.enabled = true;
        yield return new WaitForSeconds(1);
        phaseOfDayBigImage.enabled = false;
    }

    public void RestartChoiceTime()
    {
        choiceTimeLeft = choiceTimeMax;
        isChoiceTimeActive = true;
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
                fillFraction = 0;
            }
            choiceTimerImage.fillAmount = fillFraction;
        }
    }

    private DateTime GetResetDayTime() => new DateTime(1, 1, 1, 12, 0, 0);

    void UpdateClock()
    {
        if (clockState == ClockState.Paused)
            return;

        minuteTimer += Time.deltaTime;

        // Speed time up x20 if there is time on the fast-forward tracker
        if (fastForwardMinLeft > 0)
        {
            if (minuteTimer > 0.01)
            {
                AddToClock(minPerSecond);
                minuteTimer -= 0.01f;
                fastForwardMinLeft -= 1;
                if (fastForwardMinLeft <= 0)
                {
                    isFastForward = false;
                    clockState = ClockState.Paused;
                    choiceTimeLeft = 0;
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

    public void ShowSleepPanel()
    {
        clockState = ClockState.Paused;
        darkScreen.SetActive(true);
        sleepPanel.SetActive(true);
    }

    public void OnClickSleep()
    {
        currentTime = GetResetDayTime();
        Debug.Log($"sleep {currentTime}");
        clockState = ClockState.Active;
        darkScreen.SetActive(false);
        sleepPanel.SetActive(false);
        isNewDay = true;
    }

    // wait until return val then continue?
    public void FastForwardClockByMinutes(int minutes)
    {
        isFastForward = true;
        choiceTimeLeft = 0;
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

        if (currentPhase == PhaseOfDay.LateNight)
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
                phaseOfDayBigImage.sprite = phaseNoonSprite;
                phaseOfDayImage.sprite = phaseNoonSprite;
                break;

            case PhaseOfDay.Evening:
                phaseOfDayBigImage.sprite = phaseEveningSprite;
                phaseOfDayImage.sprite = phaseEveningSprite;
                break;

            case PhaseOfDay.Dusk:
                phaseOfDayBigImage.sprite = phaseDuskSprite;
                phaseOfDayImage.sprite = phaseDuskSprite;
                break;

            case PhaseOfDay.Night:
                phaseOfDayBigImage.sprite = phaseNightSprite;
                phaseOfDayImage.sprite = phaseNightSprite;
                break;

            case PhaseOfDay.LateNight:
                phaseOfDayBigImage.sprite = phaseLateNightSprite;
                phaseOfDayImage.sprite = phaseLateNightSprite;
                break;
        }

        // StartCoroutine(ShowNewPhaseImageCoroutine());
    }

}
