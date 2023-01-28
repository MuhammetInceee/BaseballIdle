using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class CanvasManager : MonoBehaviour
{
    [HideInInspector]
    public static CanvasManager instance;

    public GameObject tapToPlayButton;
    public GameObject nextLevelButton;
    public GameObject retryLevelButton;

    public GameObject tutorialRect;
    public GameObject mainMenuRect;
    public GameObject inGameRect;
    public GameObject finishRect;

    public Image levelSliderImage;

    public Text levelText;
    public Text coinText;

    public GameObject awesome;
    public GameObject unlucky;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        mainMenuRect.SetActive(true);

    }
    public void TapToPlayButtonClick()
    {
        GameManager.instance.StartGame();
    }

    public void NextLevel()
    {
        LevelManager.instance.IncreaseLevel();
        LevelManager.instance.SetLevel();

    }
    public void RestartGame()
    {
        LevelManager.instance.SetLevel();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OpenFinishRect(bool isSuccess)
    {

        if (isSuccess)
        {
            awesome.SetActive(true);
            retryLevelButton.SetActive(false);
            nextLevelButton.SetActive(true);
        }
        else
        {
            unlucky.SetActive(true);
            retryLevelButton.SetActive(true);
            nextLevelButton.SetActive(false);
        }

        inGameRect.SetActive(false);
        finishRect.GetComponent<CanvasGroup>().DOFade(1, 2f);

        finishRect.SetActive(true);
    }
}