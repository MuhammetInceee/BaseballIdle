using System;
using MoreMountains.NiceVibrations;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UnlockNewPlayerMain : MonoBehaviour
{
    public Slider slider;
    public int startPrice;
    public int priceLeft;
    public bool isBuyed;
    public TMP_Text priceText;
    public ParticleSystem openParticle;
    public GameObject area;

    void Awake()
    {
        priceLeft = PlayerPrefs.GetInt("priceLeft" + gameObject.GetInstanceID(), startPrice);
        priceText.text = priceLeft.ToString();
        slider.minValue = -startPrice;
        slider.maxValue = 0;
        slider.value = -priceLeft;
        if (isBuyed || PlayerPrefs.GetInt("isBuyed" + gameObject.GetInstanceID()) == 1)
        {
            isBuyed = true;
            transform.GetChild(0).gameObject.SetActive(false);
            if (area != null)
            {
                area.SetActive(true);
            }
        }
        if (!isBuyed)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            if (area != null)
            {
                area.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isBuyed && priceLeft > 0 && MoneyManager.Instance.currentMoney > 0 && (other.gameObject.GetComponent<Rigidbody>().velocity.magnitude < 0.1f))
        {
            MMVibrationManager.Haptic(HapticTypes.Success);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        GiveMoney(other);
    }

    private void GiveMoney(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isBuyed && priceLeft > 0 && MoneyManager.Instance.currentMoney > 0 && (other.gameObject.GetComponent<Rigidbody>().velocity.magnitude < 0.1f))
        {
            priceLeft -= startPrice / 60;
            priceText.text = "" + priceLeft;
            PlayerPrefs.SetInt("priceLeft" + gameObject.GetInstanceID(),priceLeft);
            MoneyManager.Instance.currentMoney = Mathf.Clamp(MoneyManager.Instance.currentMoney - (startPrice / 60), 0, 99999);
            if (MoneyManager.Instance.currentMoney < 1000)
            {
                UIManager.Instance.playerMoneyText.text = "" + MoneyManager.Instance.currentMoney;
            }
            else
            {
                float sayi = (MoneyManager.Instance.currentMoney / 1000f);
                string last = sayi.ToString("f2");
                UIManager.Instance.playerMoneyText.text = last + "K";
            }
            MoneyManager.Instance.currentMoney = Mathf.Clamp(MoneyManager.Instance.currentMoney, 0, 99999);
            slider.value = -priceLeft;
            PlayerPrefs.SetFloat("Money", MoneyManager.Instance.currentMoney);
            if (priceLeft < 1)
            {
                MMVibrationManager.Haptic(HapticTypes.Success);
                isBuyed = true;
                PlayerPrefs.SetInt("isBuyed" + gameObject.GetInstanceID(), 1);
                priceLeft = 0;
                GetComponent<Collider>().enabled = false;
                transform.GetChild(0).gameObject.SetActive(false);
                if (openParticle != null)
                {
                    openParticle.Play();
                }
                if (area != null)
                {
                    area.SetActive(true);
                }
                
                MatchManager.Instance.CalculateReadyMatch();
            }
        }
    }

}
