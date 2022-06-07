using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUIManager : MonoBehaviour
{
    public TextMeshProUGUI blueText;
    public TextMeshProUGUI redText;

    public void UpdateScores(float player1Score, float player2Score)
    {
        blueText.text = Mathf.RoundToInt(player1Score / 100).ToString();
        redText.text = Mathf.RoundToInt(player2Score / 100).ToString();
    }
}
