using System;
using System.Collections;
using MuhammetInce.DesignPattern.Singleton;
using Redcode.Extensions;
using UnityEngine;
using TMPro;

public class UIManager : LazySingleton<UIManager>
{
    private float _speed;
    private float _capacity;
    private float _income;

    private BoxCollider _collider;

    public UpgradableAttributeSettings upgradableAttributes;
    public TextMeshProUGUI playerMoneyText;
    public GameObject upgradeCanvas;
    
    [Header("About Match Screen Canvas: "), Space]
    public GameObject matchScreenCanvas;
    public GameObject matchScreenPlayButton;
    public GameObject goButton;
    public GameObject plus100Image;
    public GameObject looseImage;
    
    [Header("Upgrade Level Text: ")]
    [SerializeField] private TextMeshProUGUI playerSpeedText;
    [SerializeField] private TextMeshProUGUI playerCapacityText;
    [SerializeField] private TextMeshProUGUI playerIncomeText;

    [Header("Upgrade Cost Texts"), Space] 
    [SerializeField] private TextMeshProUGUI speedUpgradeCostText;
    [SerializeField] private TextMeshProUGUI capacityUpgradeCostText;
    [SerializeField] private TextMeshProUGUI incomeUpgradeCostText;

    private void Start() => StartInit();
    void Update() => UpdateInit();

    private void StartInit()
    {
        _collider = GameObject.FindWithTag("MatchTable").GetComponent<BoxCollider>();
    }
    private void UpdateInit()
    {
        playerMoneyText.text = MoneyManager.Instance.currentMoney + "$";
    }

    public void ButtonChanger()
    {
        LevelTextChanger();
        CostTextChanger();
    }

    private void LevelTextChanger()
    {
        if (PlayerPrefs.GetFloat("PlayerMovementSpeed") == 4)
        {
            playerSpeedText.text = "Level Max";
        }
        else
        {
            playerSpeedText.text = "Level " + (PlayerPrefs.GetFloat("PlayerMovementSpeed") + 1);
        }

        if (PlayerPrefs.GetFloat("PlayerCapacity") == 4)
        {
            playerCapacityText.text = "Level Max";
        }
        else
        {
            playerCapacityText.text = "Level " + (PlayerPrefs.GetFloat("PlayerCapacity") + 1);
        }

        if (PlayerPrefs.GetFloat("PlayerIncome") == 4)
        {
            playerIncomeText.text = "Level Max";
        }
        else
        {
            playerIncomeText.text = "Level " + (PlayerPrefs.GetFloat("PlayerIncome") + 1);
        }
    }

    private void CostTextChanger()
    {
        float speedPp = PlayerPrefs.GetFloat("PlayerMovementSpeed");
        float capacityPp = PlayerPrefs.GetFloat("PlayerCapacity");
        float incomePp = PlayerPrefs.GetFloat("PlayerIncome");
        
        if (speedPp == 4)
        {
            speedUpgradeCostText.gameObject.SetActive(false);
        }
        else
        {
            speedUpgradeCostText.text = upgradableAttributes.speedCost[(int)speedPp] + " $";
        }
        
        if (capacityPp == 4)
        {
            capacityUpgradeCostText.gameObject.SetActive(false);
        }
        else
        {
            capacityUpgradeCostText.text = upgradableAttributes.capacityCost[(int)capacityPp] + " $";
        }
        
        if (incomePp == 4)
        {
            incomeUpgradeCostText.gameObject.SetActive(false);
        }
        else
        {
            incomeUpgradeCostText.text = upgradableAttributes.incomeCost[(int)incomePp] + " $";
        }
    }
    

    public void SpeedUpgradeButton()
    {
        float pp = PlayerPrefs.GetFloat("PlayerMovementSpeed");

        switch (pp)
        {
            case 0 when MoneyManager.Instance.currentMoney >= upgradableAttributes.speedCost[0]:
                MoneyManager.Instance.ChangeMoney(-upgradableAttributes.speedCost[0]);
                PlayerPrefs.SetFloat("PlayerMovementSpeed", 1);
                break;
            case 1 when MoneyManager.Instance.currentMoney >= upgradableAttributes.speedCost[1]:
                MoneyManager.Instance.ChangeMoney(-upgradableAttributes.speedCost[1]);
                PlayerPrefs.SetFloat("PlayerMovementSpeed", 2);
                break;
            case 2 when MoneyManager.Instance.currentMoney >= upgradableAttributes.speedCost[2]:
                MoneyManager.Instance.ChangeMoney(-upgradableAttributes.speedCost[2]);
                PlayerPrefs.SetFloat("PlayerMovementSpeed", 3);
                break;
            case 3 when MoneyManager.Instance.currentMoney >= upgradableAttributes.speedCost[3]:
                MoneyManager.Instance.ChangeMoney(-upgradableAttributes.speedCost[3]);
                PlayerPrefs.SetFloat("PlayerMovementSpeed", 4);
                break;
        }
    }
    
    public void CapacityUpgradeButton()
    {
        float pp = PlayerPrefs.GetFloat("PlayerCapacity");

        switch (pp)
        {
            case 0 when MoneyManager.Instance.currentMoney >= upgradableAttributes.capacityCost[0]:
                MoneyManager.Instance.ChangeMoney(-upgradableAttributes.capacityCost[0]);
                PlayerPrefs.SetFloat("PlayerCapacity", 1);
                break;
            case 1 when MoneyManager.Instance.currentMoney >= upgradableAttributes.capacityCost[1]:
                MoneyManager.Instance.ChangeMoney(-upgradableAttributes.capacityCost[1]);
                PlayerPrefs.SetFloat("PlayerCapacity", 2);
                break;
            case 2 when MoneyManager.Instance.currentMoney >= upgradableAttributes.capacityCost[2]:
                MoneyManager.Instance.ChangeMoney(-upgradableAttributes.capacityCost[2]);
                PlayerPrefs.SetFloat("PlayerCapacity", 3);
                break;
            case 3 when MoneyManager.Instance.currentMoney >= upgradableAttributes.capacityCost[3]:
                MoneyManager.Instance.ChangeMoney(-upgradableAttributes.capacityCost[3]);
                PlayerPrefs.SetFloat("PlayerCapacity", 4);
                break;
        }
    }
    
    public void IncomeUpgradeButton()
    {
        float pp = PlayerPrefs.GetFloat("PlayerIncome");

        switch (pp)
        {
            case 0 when MoneyManager.Instance.currentMoney >= upgradableAttributes.incomeCost[0]:
                MoneyManager.Instance.ChangeMoney(-upgradableAttributes.incomeCost[0]);
                PlayerPrefs.SetFloat("PlayerIncome", 1);
                break;
            case 1 when MoneyManager.Instance.currentMoney >= upgradableAttributes.incomeCost[1]:
                MoneyManager.Instance.ChangeMoney(-upgradableAttributes.incomeCost[1]);
                PlayerPrefs.SetFloat("PlayerIncome", 2);
                break;
            case 2 when MoneyManager.Instance.currentMoney >= upgradableAttributes.incomeCost[2]:
                MoneyManager.Instance.ChangeMoney(-upgradableAttributes.incomeCost[2]);
                PlayerPrefs.SetFloat("PlayerIncome", 3);
                break;
            case 3 when MoneyManager.Instance.currentMoney >= upgradableAttributes.incomeCost[3]:
                MoneyManager.Instance.ChangeMoney(-upgradableAttributes.incomeCost[3]);
                PlayerPrefs.SetFloat("PlayerIncome", 4);
                break;
        }
    }

    public void MatchScreenExitButton()
    {
        GameManager.instance.isMatchScreen = false;
        matchScreenCanvas.SetActive(false);
        _collider.enabled = false;
        MatchManager.Instance.Exit();
        StartCoroutine(ExitButton());
    }

    private IEnumerator ExitButton()
    {
        yield return new WaitForSeconds(1.5f);
        _collider.enabled = true;
    }

    public void PlayButton()
    {
        MatchManager.Instance.PlayButton();
    }

    public void GoButton()
    {
        MatchManager.Instance.GoButton();
    }
}
