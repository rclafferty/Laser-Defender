using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    int score;
    int sessionHiscore;
    TextMeshProUGUI scoreText;
    TextMeshProUGUI hiscoreText;

    private void Awake()
    {
        int numberGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numberGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            scoreText = GameObject.Find("Score Text").GetComponent<TextMeshProUGUI>();
            hiscoreText = GameObject.Find("Hiscore Text").GetComponent<TextMeshProUGUI>();

            SceneManager.sceneLoaded += UpdateGameObjects;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        sessionHiscore = 0;
        UpdateGUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateGameObjects(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "title")
            return;

        scoreText = GameObject.Find("Score Text").GetComponent<TextMeshProUGUI>();
        hiscoreText = GameObject.Find("Hiscore Text").GetComponent<TextMeshProUGUI>();

        if (scene.name == "game")
            score = 0;

        UpdateGUI();
    }

    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            if (value >= 0)
            {
                score = value;
                UpdateGUI();
            }
        }
    }

    public void AddToScore(int value)
    {
        if (value > 0)
        {
            score += value;

            UpdateGUI();
        }
    }

    public void ResetGame()
    {

    }

    void UpdateGUI()
    {
        if (score > sessionHiscore)
        {
            sessionHiscore = score;
        }

        scoreText.text = "Score:   " + score.ToString("00000");
        hiscoreText.text = "Hiscore: " + sessionHiscore.ToString("00000");
    }
}
