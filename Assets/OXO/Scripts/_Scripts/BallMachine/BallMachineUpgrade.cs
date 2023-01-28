using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class BallMachineUpgrade : MonoBehaviour
{
    private bool _canRestart;
        
    public Slider slider;
    public int startPrice;
    public int priceLeft;
    public TMP_Text priceText;

    public GameObject moneyImage;
    public TextMeshProUGUI text;

    private BallMachine BallMachine => transform.parent.GetComponent<BallMachine>();
    private void Awake()
    {
        if(PlayerPrefs.GetInt("priceLeft" + gameObject.GetInstanceID()) == 0)
            PlayerPrefs.SetInt("priceLeft" + gameObject.GetInstanceID(), startPrice);


        CheckCanvas();
        
        priceLeft = PlayerPrefs.GetInt("priceLeft" + gameObject.GetInstanceID());
        priceText.text = priceLeft.ToString();
        slider.minValue = -startPrice;
        slider.maxValue = 0;
        slider.value = -priceLeft;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GiveMoney(other);
        }
    }

    private void GiveMoney(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && MoneyManager.Instance.currentMoney > 0 && priceLeft > 0 &&
            (other.gameObject.GetComponent<Rigidbody>().velocity.magnitude < 0.1f))
        {
            priceLeft -= startPrice / 100;
            priceText.text = "" + priceLeft;
            PlayerPrefs.SetInt("priceLeft" + gameObject.GetInstanceID(),priceLeft);
            MoneyManager.Instance.currentMoney =
                Mathf.Clamp(MoneyManager.Instance.currentMoney - (startPrice / 100), 0, 99999);
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
                priceLeft = 0;
                PlayerPrefs.SetInt("priceLeft" + gameObject.GetInstanceID(), 0);
                _canRestart = true;
                BallMachine.LevelUp();
                CheckCanvas();
                StartCoroutine(UpgradeAgain());
            }
        }
    }

    private IEnumerator UpgradeAgain()
    {
        yield return new WaitForSeconds(1f);
        if (!_canRestart) yield break;
        
        priceLeft = startPrice;
        priceText.text = priceLeft.ToString();
        slider.minValue = -startPrice;
        slider.maxValue = 0;
        slider.value = -priceLeft;
    }

    private void CheckCanvas()
    {
        if (PlayerPrefs.GetFloat("GenerateDuration") <= 0.1f && PlayerPrefs.GetFloat("GenerateDuration") > 0)
        {
            priceText.gameObject.SetActive(false);
            moneyImage.SetActive(false);
            text.text = "Max Level Reached";
            GetComponent<Collider>().enabled = false;
        }
    }
}