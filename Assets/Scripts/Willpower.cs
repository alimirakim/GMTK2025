using UnityEngine;
using UnityEngine.UI;

public class Willpower : MonoBehaviour
{
    [SerializeField] Slider meter;
    [SerializeField] int startVal = 100;
    [SerializeField] int val;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        meter.value = val;
    }

    public void UpdateWillpower(int wpToAdd)
    {
        val += wpToAdd;
        meter.value = val;
    }

    public void ResetWillpower()
    {
        val = startVal;
    }

    public int GetVal() => val;
}
