﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private GameObject pausePanel;
    public GameObject playPanel;
    public GameObject endPanel;

    [Header("Text")]
    public Text endText; //Shows if you won or lost
    public Text causeText; //Shows what caused the game to end
    public Text gateText; //Appears when the gate has opened

    private bool gameStarted = false; //Used to determine when game can be paused
    private bool isPaused = false; //Shows if the game is paused or not

    void Start()
    {
        startPanel.SetActive(true);
        infoPanel.SetActive(false);
        pausePanel.SetActive(false);
        playPanel.SetActive(false);
        endPanel.SetActive(false);
        gateText.gameObject.SetActive(false);
    }


    void Update()
    {
        OnEscPress();

    }

    public void OnStartClick() //Affects the startButton
    {
        playPanel.SetActive(true);
        startPanel.SetActive(false);
        gameStarted = true;
        print("Start Clicked");
    }

    public void OnInfoClick() //Affects the infoButton
    {
        infoPanel.SetActive(true);
        startPanel.SetActive(false);
        print("Info Clicked");
    }

    public void OnQuitClick() //Affects the quitButton
    {
        Application.Quit();
        print("Quit Clicked");
    }

    public void OnBackClick() //Affects the backButton
    {
        startPanel.SetActive(true);
        infoPanel.SetActive(false);
        print("Back Clicked");
    }

    public void OnReplayClick() //Affects the replayButton and the restartButton
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        print("Game Restarted");
    }

    public void OnResumeClick() //Affects the resumeButton
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        isPaused = false;
        print("Resume Clicked");
    }

    private void OnEscPress() //Affects what happens when you press the Esc Key
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameStarted == true)
            {
                if (isPaused == false)
                {
                    Time.timeScale = 0;
                    pausePanel.SetActive(true);
                    isPaused = true;
                    print("Game Paused");
                }
                else if (isPaused == true)
                {
                    Time.timeScale = 1;
                    pausePanel.SetActive(false);
                    isPaused = false;
                    print("Game Unpaused");
                }
            }
        }
    }
}