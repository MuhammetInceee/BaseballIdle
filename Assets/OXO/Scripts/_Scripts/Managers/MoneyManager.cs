using System;
using MuhammetInce.DesignPattern.Singleton;
using UnityEngine;

public class MoneyManager : LazySingleton<MoneyManager>
{
    public float currentMoney;
    public KeyCode keyCode;

    private void Start()
    {
        currentMoney = PlayerPrefs.GetFloat("Money", 0);
    }

    public void ChangeMoney(float money)
    {
        currentMoney += money;
        SetPlayerPrefs();
    }
    public void SetPlayerPrefs() => PlayerPrefs.SetFloat("Money", currentMoney);

    private void Update()
    {
        if (Input.GetKeyDown(keyCode))
        {
            ChangeMoney(100);
        }
    }
}
