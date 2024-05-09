using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LogicManager : MonoBehaviour
{
    public int playerScore = 0;
    public TextMeshProUGUI scoreText;
    public PanelManager panelManager;
    public void Start()
    {
        Time.timeScale = 0f;
    }
    public void addScore()
    {
        playerScore++;
        scoreText.text = playerScore.ToString();
    }
    public void gameOver()
    {
        Time.timeScale = 1;
        panelManager.changePanel("Game Over");
        Invoke("restartGame", 3f);
    }
    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && (panelManager.currentPanel.name == "Pause" || panelManager.currentPanel.name == "Playing"))  // Change KeyCode.Escape to any key you want to use for pausing
        {
            TogglePause();
        }
    }
    public void StartGame()
    {
        panelManager.clearPanel();
        Time.timeScale = 1;
    }
    public void TogglePause()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0; // Pause the game
            panelManager.changePanel("Pause");
        }
        else
        {
            Time.timeScale = 1; // Resume the game
            panelManager.changePanel("Playing");
        }
    }
}
