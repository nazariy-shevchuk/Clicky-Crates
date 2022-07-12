using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<GameObject> targets;
    public GameObject[] lifeIndicators;
    public GameObject titleScreen;

    public bool isGameActive;

    public Button restartButton;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;

    [SerializeField] private int score;
    [SerializeField] private int lives;
    [SerializeField] private int maxLives;
    [SerializeField] private float spawnRate = 1.0f;

    // Start the game, remove title screen, reset score, and adjust spawnRate based on difficulty button clicked
    public void StartGame(int difficulty)
    {
        spawnRate /= difficulty;
        isGameActive = true;
        score = 0;
        maxLives = lifeIndicators.Length;
        lives = 0;

        StartCoroutine(SpawnTarget());
        UpdateScore(score);
        UpdateLives(maxLives);

        scoreText.gameObject.SetActive(true);
        titleScreen.gameObject.SetActive(false);
    }


    // While game is active spawn a random target
    IEnumerator SpawnTarget()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(spawnRate);
            
            int index = Random.Range(0, targets.Count);
            Instantiate(targets[index]);
        }
    }

    // Update score with value from target clicked
    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }

    // Update lives
    public void UpdateLives(int livesToChange)
    {
        lives += livesToChange;
        Debug.Log("Lives: " + lives);

        if (livesToChange > 0)
        {
            for (int i = 0; i < lives; i++)
            {
                lifeIndicators[i].gameObject.SetActive(true);
            }
        }
        else
        {
            lifeIndicators[lives].gameObject.SetActive(false);
        }

        if (lives <= 0)
        {
            GameOver();
        }
    }

    // Stop game, bring up game over text and restart button
    public void GameOver()
    {
        isGameActive = false;
        gameOverText.gameObject.SetActive(true);
        
        restartButton.gameObject.SetActive(true);
    }

    // Restart game by reloading the scene
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
