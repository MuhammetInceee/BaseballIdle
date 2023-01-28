using System;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeDeskCollide : MonoBehaviour
{
    public Slider slider;

    private void Start()
    {
        slider = transform.GetChild(1).GetComponent<Slider>();
        slider.minValue = 0;
        slider.maxValue = 1.5f;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && other.gameObject.GetComponent<Rigidbody>().velocity.magnitude < 0.1f)
        {
            slider.value += Time.deltaTime;
            if (slider.value == slider.maxValue)
            {
                UIManager.Instance.ButtonChanger();
                UIManager.Instance.upgradeCanvas.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        slider.value = 0;
        UIManager.Instance.upgradeCanvas.SetActive(false);
    }
}
