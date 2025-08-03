using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Weight : MonoBehaviour
{
    // Grab shame button image
    // method to adjust image size bigger, smaller
    // onclick method to show causes of shame in popups


    [field: SerializeField] public List<ShameSO> ShameList { get; private set; }
    [SerializeField] Image weightImage;
    [SerializeField] GameObject messageTextPrefab;
    List<GameObject> messageTextObjects = new List<GameObject>();
    [SerializeField] int messageMinX = -400;
    [SerializeField] int messageMaxX = 400;
    [SerializeField] int messageMinY = -300;
    [SerializeField] int messageMaxY = 500;
    [SerializeField] float speedOfMessageAppearance = 0.3f;
    [SerializeField] float speedOfMessageRemoval = 0.2f;

    IEnumerator ShowMessagesCoroutine;
    IEnumerator RemoveMessagesCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ShowMessagesCoroutine = ShowMessages();
        RemoveMessagesCoroutine = RemoveMessages();
        UpdateWeightSize();
        // attach onhover to shame weight
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnEnterWeightArea()
    {
        Debug.Log("Hovered shame weight! :( ");
        StopCoroutine(RemoveMessagesCoroutine);
        StartCoroutine(ShowMessagesCoroutine);


        
        // create list and group to store bubbles
        // create prafab object with message
        // spawn in random location near weight in circlish
        // TODO have them fade in one by one
        // TODO have their size differ by associated shame weight

    }

    public void OnExitWeightArea()
    {
        StopCoroutine(ShowMessagesCoroutine);
        StartCoroutine(RemoveMessagesCoroutine);
    }

    // Breaks when attempting to remove items that don't exist anymore, due to start/stop routines
    IEnumerator RemoveMessages()
    {

        foreach (GameObject messageTextObject in messageTextObjects)
        {
            Destroy(messageTextObject);
            yield return new WaitForSeconds(speedOfMessageRemoval);
        }
    }

    // TODO Once foreach is done, messages run out and don't reappear
    IEnumerator ShowMessages()
    {
        // Create and display text bubbles with shames, varying size of bubble and text based on 'shame value'
        // Perhaps affect avatar mood the longer they contemplate shames

        foreach (ShameSO shame in ShameList)
        {
            List<string> messages = shame.GetMessages();
            foreach (string message in messages)
            {
                GameObject messageTextObject = Instantiate(messageTextPrefab, weightImage.transform);
                messageTextObjects.Add(messageTextObject);
                TextMeshProUGUI textMeshPro = messageTextObject.GetComponent<TextMeshProUGUI>();
                textMeshPro.text = message;
                RectTransform rectTransform = messageTextObject.GetComponent<RectTransform>();
                Vector2 msgPos = GetRandomMessagePosition();
                Debug.Log($"choosing pos{msgPos}");
                rectTransform.anchoredPosition = msgPos;
                // await 0.1s per message
                yield return new WaitForSeconds(speedOfMessageAppearance);
                
            }
        }
    }

    public void UpdateWeightSize()
    {


        RectTransform rectTransform = weightImage.GetComponent<RectTransform>();
        float shameSize = CalculateShameSize();

        Vector3 localScale = rectTransform.localScale;
        localScale.x = shameSize;
        localScale.y = shameSize;
        rectTransform.localScale = localScale;
        // can also try with .sizeDelta instead

        // Grab Rect Transform of object
        // Adjust Scale X/Y based on shameQuantity
    }

    Vector2 GetRandomMessagePosition()
    {
        int x = Random.Range(messageMinX, messageMaxX);
        int y = Random.Range(messageMinY, messageMaxY);
        return new Vector2(x, y);
    }

    float CalculateShameSize()
    {
        int shameQuantity = 0;
        foreach (ShameSO shame in ShameList)
        {
            shameQuantity += shame.GetWeightValue();
        }
        float shameScaling = shameQuantity / 100;
        Debug.Log($"shame quantity {shameQuantity}, shame Scaling {shameScaling}");


        return shameScaling;
    }
}
