using System.Collections;
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

    [Header("Bools")]
    public bool gameStarted = false; //Used to determine when game can be paused
    public bool isPaused = false; //Shows if the game is paused or not

    [Header("Hearts")]
    public List<GameObject> hearts;

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
        Time.timeScale = 1;
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

    public void ShowGateText()
    {
        gateText.gameObject.SetActive(true);
    }

    public void GameOver() //Brings up the endPanel when a game ending condition is met
    {
        endPanel.SetActive(true);
    }

    public void VictoryMessage() //Affects endText and causeText to tell the player they won
    {
        endText.text = "Victory!";
        causeText.text = "You Made It Home!";
    }

    public void DefeatMessage() //Affects endText and causeText to tell the player they lost
    {
        endText.text = "Defeat";
        causeText.text = "You Were Killed";
    }

    public void LoseHeart(int health) //Affects the order in which the heart sprites are disabled
    {
        foreach (GameObject x in hearts)
        {
            if (health == hearts.IndexOf(x))
            {
                x.SetActive(false);
            }
        }
    }
}
