using UnityEngine;
using UnityEngine.SceneManagement;
public class SplashMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickStart()
    {
        SceneManager.LoadScene(1);
        Debug.Log("Start new game! :D");
    }

    public void OnClickOptions()
    {
        Debug.Log("Clicked Options! :)");
    }

    public void OnClickCredits()
    {
        Debug.Log("Clicked Credits! :)");
    }
}
