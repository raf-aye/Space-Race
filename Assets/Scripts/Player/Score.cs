using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{

    private TMP_Text txt; // Reference to the score text UI component
    public float increaseSpeed = 10f; // The speed at which the score increases
    public float targetScore = 0; // Scripts will add to this score, then current score will try to update to that

    public float currentScore = 0;
    void Start()
    {
        txt = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentScore < targetScore)
        {
            float increment = increaseSpeed * Time.deltaTime;
            currentScore = Mathf.Clamp(currentScore + increment, 0f, targetScore);
            UpdateScoreText();
        }
    }

    private void UpdateScoreText()
    {
        int roundedScore = Mathf.RoundToInt(currentScore);
        string formattedScore = roundedScore.ToString("D7");
        //txt.fontStyle = FontStyles.Bold | FontStyles.SmallCaps;
        txt.SetText("Score: " + formattedScore);
    }
}
