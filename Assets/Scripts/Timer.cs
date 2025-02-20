using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("Component")]
    public TextMeshProUGUI timerText;

    [Header("Timer Settings")]
    public float currentTime;

    // Update is called once per frame
    void Update()
    {
        currentTime = currentTime + Time.deltaTime;
        timerText.text = "Time: " + currentTime.ToString("0.00");
    }
}
