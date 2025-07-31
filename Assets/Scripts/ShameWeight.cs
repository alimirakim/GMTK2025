using UnityEngine;
using UnityEngine.UI;

public class ShameWeight : MonoBehaviour
{
    // Grab shame button image
    // method to adjust image size bigger, smaller
    // onclick method to show causes of shame in popups

    [SerializeField] Button shameWeight;
    [SerializeField] int shameQuantity = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickShameWeight()
    {
        Debug.Log("Clicked shame weight! :( ");
        Debug.Log("No job or money ");
        Debug.Log("Can't do anything right ");
        Debug.Log("Burden on my family ");
    }

    void ShowMessages()
    {
        // Grab list of player's currently held shames
        // Create and display text bubbles with shames, varying size of bubble and text based on 'shame value'
        // Perhaps affect avatar mood the longer they contemplate shames
    }

    void UpdateShameQuantity(int additionalShameQuantity)
    {
        shameQuantity += additionalShameQuantity;
        UpdateShameSize();
    }

    void UpdateShameSize()
    {
        // Grab Rect Transform of object
        // Adjust Scale X/Y based on shameQuantity
    }
}
