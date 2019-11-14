using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    public static int score;
    public static int lines;

    Text scoreText;
    Text linesText;

    // Use this when game is started initialization
    void Awake()
    {
        scoreText = GameObject.Find("ScoreLabel").GetComponent<Text>(); ;
        linesText = GameObject.Find("LinesClearedLabel").GetComponent<Text>(); ;
    }

    void Start()
    {
        score = 0;
        lines = 0;
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + System.String.Format("{0:D8}", score);
        linesText.text = "Lines Cleared: " +  lines;
    }
}