using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using MuhammetInce.DesignPattern.Singleton;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MatchManager : LazySingleton<MatchManager>
{
    private bool _canBoing;
    private bool _isCalculated;
    private bool _canPlay;
    private bool _weWin;
    private bool _rivalWin;
    
    private int _myLevelForText;
    private int _rivalLevelForText;
    private string _myLevelString;
    private string _rivalLevelString;

    private int _myScoreForText;
    private int _rivalScoreForText;

    [SerializeField] private List<GameObject> baseballPlayers;
    public int activePlayerCount;
    public bool timeToMatch;
    public int myPlayerLevels;
    public int rivalPlayerLevels;
    public int myScore;
    public int rivalScore;

    [Header("About UI Elements : "), Space]
    public Slider slider;
    public GameObject warningText;

    [Header("About Match Screen Elements: "), Space]
    public string matchReadyText;
    public TextMeshProUGUI myLevelText;
    public TextMeshProUGUI rivalLevelText;
    public TextMeshProUGUI myScoreText;
    public TextMeshProUGUI rivalScoreText;

    [Header("Cards : "), Space] 
    public GameObject myTeamCard;
    public GameObject rivalTeamCard;
    public TextMeshProUGUI myCardLevelText;
    public TextMeshProUGUI rivalCardLevelText;

    private int _myCardLevel;
    private int _rivalCardLevel;


    private void Start()
    {
        CalculateReadyMatch();
        slider = transform.GetChild(2).GetComponent<Slider>();
        warningText.SetActive(false);
        slider.minValue = 0;
        slider.maxValue = 1.5f;

        _myLevelString = myLevelText.text;
        _rivalLevelString = rivalLevelText.text;
    }

    private void Update()
    {
        myCardLevelText.text = "Level " + _myCardLevel;
        rivalCardLevelText.text = "Level " + _rivalCardLevel;
    }

    private void CalculateTeamLevels()
    {
        foreach (var player in baseballPlayers)
        {
            myPlayerLevels += player.GetComponent<BaseballPlayerMain>().currentLevel;
        }

        for (int i = 0; i < baseballPlayers.Count; i++)
        {
            _myCardLevel = baseballPlayers[0].GetComponent<BaseballPlayerMain>().currentLevel;
            if (baseballPlayers[i].GetComponent<BaseballPlayerMain>().currentLevel > _myCardLevel)
                _myCardLevel = baseballPlayers[i].GetComponent<BaseballPlayerMain>().currentLevel;
        }

        _rivalCardLevel = _myCardLevel + Random.Range(-2, 2);
        if (_rivalCardLevel <= 1)
            _rivalCardLevel = 1;

        rivalPlayerLevels = Random.Range(myPlayerLevels - 10, myPlayerLevels + 10);
        if (rivalPlayerLevels < 1)
            rivalPlayerLevels = 1;
    }

    public void CalculateReadyMatch()
    {
        activePlayerCount = 0;
        foreach (var t in baseballPlayers.Where(t => t.activeInHierarchy))
        {
            activePlayerCount++;

            if (activePlayerCount == baseballPlayers.Count)
            {
                timeToMatch = true;
                MessageManager.instance.Show(matchReadyText);
            }
                
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            CalculateTeamLevels();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
            MatchScreenReady(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Exit();
        }
    }

    private void MatchScreenReady(Collider other)
    {
        if (other.CompareTag("Player") && other.gameObject.GetComponent<Rigidbody>().velocity.magnitude < 0.1f)
        {
            slider.value += Time.deltaTime;
            switch (timeToMatch)
            {
                case true when slider.value == slider.maxValue:
                    Enter();
                    Stay();
                    break;
                case false when slider.value == slider.maxValue:
                    warningText.SetActive(true);
                    break;
            }
        }
    }


    
    private void Enter()
    {
        if(_isCalculated) return;
        myScoreText.text = "0";
        rivalScoreText.text = "0";
        GameManager.instance.isMatchScreen = true;
        UIManager.Instance.matchScreenCanvas.SetActive(true);
        DOTween.To(() => _myLevelForText, (b) =>  _myLevelForText = b, myPlayerLevels, 2f).SetEase(Ease.Linear).OnUpdate(
            () =>
            {
                myLevelText.text = _myLevelString + _myLevelForText;
            });

        DOTween.To(() => _rivalLevelForText, (b) => _rivalLevelForText = b, rivalPlayerLevels, 2f).SetEase(Ease.Linear)
            .OnUpdate(
                () =>
                {
                    rivalLevelText.text = _rivalLevelString + _rivalLevelForText;
                }).OnComplete(() =>
            {
                StartCoroutine(CardAnimator());
            });
        
        _isCalculated = true;
    }

    private void Stay()
    {
        if (_canPlay)
        {
            UIManager.Instance.matchScreenPlayButton.SetActive(true);
            _canPlay = false;
        }
            
    }



    public void PlayButton()
    {
        int winnerSelector = Random.Range(0, 3);
        
        switch (winnerSelector)
        {
            case 0 or 1:
                //Player  Win
                myScore = Random.Range(5, 12);
                rivalScore = myScore - Random.Range(1, 4);
                
                if (rivalScore < 0)
                    rivalScore = 0;
                
                ScoreCounter();
                break;
            case 2:
                // AI Win
                rivalScore = Random.Range(5, 12);
                myScore = rivalScore - Random.Range(1, 3);
                
                if (myScore < 0)
                    myScore = 0;
                
                ScoreCounter();
                break;
        }
        
        UIManager.Instance.matchScreenPlayButton.SetActive(false);
    }

    private void ScoreCounter()
    {
        DOTween.To(() => _myScoreForText, (b) =>  _myScoreForText = b, myScore, 2f).SetEase(Ease.Linear).OnUpdate(
            () =>
            {
                if(_myScoreForText <= 9)
                    myScoreText.text = "0" + _myScoreForText.ToString();
                else if(_myScoreForText > 9)
                    myScoreText.text =_myScoreForText.ToString();
            });

        DOTween.To(() => _rivalScoreForText, (b) => _rivalScoreForText = b, rivalScore, 2f).SetEase(Ease.Linear)
            .OnUpdate(
                () =>
                {
                    if(_myScoreForText <= 9)
                        rivalScoreText.text = "0" + _rivalScoreForText.ToString();
                    else if(_myScoreForText > 9)
                        rivalScoreText.text = _rivalScoreForText.ToString();
                    
                    
                }).OnComplete(() =>
            {
                if (rivalScore > myScore)
                {
                    _rivalWin = true;
                    _weWin = false;
                    UIManager.Instance.goButton.SetActive(true);
                    UIManager.Instance.looseImage.SetActive(true);
                }
                    
                else if (myScore > rivalScore)
                {
                    _weWin = true;
                    _rivalWin = false;
                    UIManager.Instance.goButton.SetActive(true);
                    UIManager.Instance.plus100Image.SetActive(true);
                }
                    
            });
    }

    public IEnumerator CardAnimator()
    {
        myTeamCard.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        rivalTeamCard.SetActive(true);
        _canPlay = true;
    }

    public void GoButton()
    {
        switch (_weWin)
        {
            case true when !_rivalWin:
                MoneyManager.Instance.ChangeMoney(200);
                UIManager.Instance.plus100Image.SetActive(false);
                UIManager.Instance.goButton.SetActive(false);
                break;
            case false when _rivalWin:
                UIManager.Instance.looseImage.SetActive(false);
                UIManager.Instance.goButton.SetActive(false);
                break;
        }
        
        UIManager.Instance.MatchScreenExitButton();
    }
    
    public void Exit()
    {
        slider.value = 0;
        _canBoing = false;
        _isCalculated = false;
        _myLevelForText = 0;
        _rivalLevelForText = 0;
        _myScoreForText = 0;
        _rivalScoreForText = 0;
        myPlayerLevels = 0;
        _canPlay = false;
        myLevelText.text = _myLevelString;
        rivalLevelText.text = _rivalLevelString;
        UIManager.Instance.matchScreenPlayButton.SetActive(false);
        warningText.SetActive(false);
        
        myTeamCard.SetActive(false);
        rivalTeamCard.SetActive(false);
    }
    

}
