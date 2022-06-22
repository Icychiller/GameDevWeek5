using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreMonitor : MonoBehaviour
{
    public IntVariable gameScore;
    public TextMeshProUGUI text;
    public void Start()
    {
        UpdateScore();
    }
    public void UpdateScore()
    {
        Debug.Log("Update called");
        text.text = "Score\n" + gameScore.value.ToString();
    }
}
