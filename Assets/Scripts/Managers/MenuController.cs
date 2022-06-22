using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class MenuController : MonoBehaviour
{
    public GameObject startMenu;
    public GameObject gameOver;
    public GameObject loadingScreen;
    public IntVariable playerScore;
    public FloatVariable loadingProgress;
    public FloatVariable bossPercentHealth;
    public TextMeshProUGUI endScoreText;
    public Slider loadingBar;
    public GameObject bossHealthBar;
    public bool firstStage = false;
    public bool bossPresent = false;
    private bool isGameOver = false;
    private bool audioDone = false;

    void Awake()
    {
        if (firstStage || startMenu.activeInHierarchy)
        {
            Time.timeScale = 0.0f;
        }
        if (bossPresent)
        {
            bossHealthBar.SetActive(true);
            bossPercentHealth.SetValue(1);
            
        }
    }

    public void StartButtonClicked()
    {
        foreach (Transform eachChild in startMenu.transform)
        {
            if (eachChild.name != "Score" && eachChild.name != "Powerups")
            {
                Debug.Log("Child Found. Name: " + eachChild.name);
                // Disable Each Child
                eachChild.gameObject.SetActive(false);
                Time.timeScale = 1f;
            }
        }
    }

    public void GameOver()
    {
        if (!isGameOver)
        {
            StartCoroutine(waitAWhile());
            // Debug.Log("Game Over Triggered");
            // Time.timeScale = 0f;
            // gameOver.SetActive(true);
            // endScoreText.text = "Score\n"+playerScore.value;
            // endCoinText.text = "Coins\n";
            isGameOver = true;
        }
    }

    IEnumerator waitAWhile()
    {
        yield return new WaitUntil(() => audioDone);
        Debug.Log("Game Over Triggered");
        endScoreText.text = "Score\n"+playerScore.value;
        gameOver.SetActive(true);
        // isGameOver = true;
    }

    public void RestartGame()
    {
        gameOver.SetActive(false);
    }

    public void changeScreen()
    {
        audioDone = true;
        if(!loadingScreen.activeInHierarchy)
        {
            loadingScreen.SetActive(true);
        }

    }

    public void Update()
    {
        if(loadingScreen.activeInHierarchy)
        {
            loadingBar.value = loadingProgress.value;
        }
        if(bossHealthBar.activeInHierarchy)
        {
            bossHealthBar.GetComponent<Slider>().value = bossPercentHealth.value;
        }
    }
}
