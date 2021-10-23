using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<GameObject> targerts;
    public TextMeshProUGUI scoreText, highScoreText, gameOverText, levelText;
    public Button restartButton;
    public Button continueButton;

    private int score, highScore;
    private float spawnRate = 5f;

    public GameObject titleScreen, pauseMenu, pickerWhell, shop;

    public bool isGameActive;

    public int exp;
    private int expToNextLevel;
    private int level = 1;

    public List<GameObject> prefabLoad;
    private int prefabCount;



    void Awake()
    {
        instance = this;
        if (PlayerPrefs.HasKey("SaveHighScore"))
        {
            highScore = PlayerPrefs.GetInt("SaveHighScore");
        }
        if (PlayerPrefs.HasKey("Count"))
        {
            prefabCount = PlayerPrefs.GetInt("Count");
        }
    }


    public void BuyPrefab(int prefabIndex)
    {
        prefabLoad = new List<GameObject>(Resources.LoadAll<GameObject>("Targets"));
        targerts.Add(prefabLoad[prefabIndex]);

        for (int i = 0; i < targerts.Count; i++)
        {
            PlayerPrefs.SetInt("Count", targerts.Count);
        }
    }


    void Update()
    {
        scoreText.text = "Score: " + score;
        highScoreText.text = "HighScore: " + highScore;
    }

    IEnumerator SpawnTarget()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(spawnRate);
            int index = Random.Range(0, targerts.Count);
            Instantiate(targerts[index]);
        }
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        instance.Score();
        HighScore();
    }


    public void Score()
    {
        HighScore();
    }


    public void HighScore()
    {
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("SaveHighScore", highScore);
        }
    }


    public void LevelUp(int lvl)
    {
        exp += lvl;
        if (exp >= expToNextLevel)
        {
            level++;
            spawnRate -= 0.3f;
            levelText.text = "Level " + level;
            exp -= expToNextLevel;
        }
    }

    public void GameOver()
    {
        pauseMenu.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(true);
        isGameActive = false;
        restartButton.gameObject.SetActive(true);
        continueButton.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void PauseOff()
    {
        pauseMenu.gameObject.SetActive(false);
    }

    public void Continue()
    {
        AdsCore.ShowAdsVideo("Rewarded_Android");
        isGameActive = true;
        StartCoroutine(SpawnTarget());
        pauseMenu.gameObject.SetActive(false);
    }

    public void InterstitialReward()
    {
        AdsCore.ShowAdsVideo("Interstitial_Android");
    }

    public void StartGame()
    {
        levelText.text = "Level " + level;
        exp = 0;
        expToNextLevel = 10;

        isGameActive = true;
        StartCoroutine(SpawnTarget());

        titleScreen.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(false);
    }

    public void OpenShop()
    {
        shop.gameObject.SetActive(true);
    }

    public void CloseShop()
    {
        shop.gameObject.SetActive(false);
    }
}
