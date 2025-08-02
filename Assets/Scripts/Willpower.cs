using UnityEngine;
using UnityEngine.UI;

public class Willpower : MonoBehaviour
{
    [SerializeField] Slider willpowerMeter;
    [SerializeField] int willpowerVal;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void UpdateWillpower(int wpToAdd)
    {
        willpowerVal += wpToAdd;
        willpowerMeter.value = willpowerVal;
    }
}
