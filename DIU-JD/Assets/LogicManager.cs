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
    public GameObject gameOverPanel;
    public void addScore()
    {
        playerScore++;
        scoreText.text = playerScore.ToString();
    }
    public void gameOver()
    {
        gameOverPanel.SetActive(true);
        Invoke("restartGame", 3f);
    }
    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
