using System.Collections.Generic;
using System.Collections;
using System.Linq;
using DG.Tweening;
using MoreMountains.NiceVibrations;
using MuhammetInce.DesignPattern.Singleton;
using UnityEngine;

public class PlayerCollide : LazySingleton<PlayerCollide>
{
    private int _otherNeededMoney;
    
    private float _income;
    public float capacity;
    private float _timerHelper;
    
    private bool _isProgressing;
    private bool _isOnTheBaseballStackArea;

    [SerializeField] private UpgradableAttributeSettings playerSettings;
    [SerializeField] private BallMachine ballMachine;

    [Header("About Lists: "), Space]
    public List<GameObject> playerHandObjects;
    public List<GameObject> handStackHolderList;

    [Header("About DoTween Duration: "), Space]
    [SerializeField] private float ballDropDuration;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BaseballStackArea"))
        {
            _isOnTheBaseballStackArea = true;
            StartCoroutine(StackBallBaseBallPlayer(other));
        }

        if (other.CompareTag("Money"))
        {
            CheckIncome();
            BaseballPlayerMain playerComponent = other.GetComponentInParent<BaseballPlayerMain>();
            
            other.transform.SetParent(transform.GetChild(0).transform);
            other.transform.DOLocalJump(Vector3.zero, 1f, 1, 0.35f).OnComplete(() =>
            {
                MMVibrationManager.Haptic(HapticTypes.LightImpact);
                
                if(playerComponent.isTwin) 
                    MoneyManager.Instance.currentMoney += 2 * _income;
                else
                    MoneyManager.Instance.currentMoney += _income;
            
                MoneyManager.Instance.SetPlayerPrefs();
                
                playerComponent.stackMoneyList.Remove(other.gameObject);
                Destroy(other.gameObject);
            });


        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BaseballStackArea"))
        {
            _isOnTheBaseballStackArea = false;
            _isProgressing = false;
        }
    }
    private IEnumerator StackBallBaseBallPlayer(Collider other)
    {
        if (_isProgressing) yield break;
        _isProgressing = true;

        while (TargetHolder(other) != null && _isOnTheBaseballStackArea)
        {
            GameObject targetHolder = TargetHolder(other);
            Transform root = other.transform.parent.transform.parent;
            if (playerHandObjects.Count == 0) yield break;
            
            GameObject lastBall = playerHandObjects[^1]!.gameObject;
            lastBall.transform.parent = targetHolder!.transform;
            playerHandObjects.Remove(lastBall);
            root.GetComponent<BaseballPlayerMain>().stackBallList.Add(lastBall);
            lastBall.transform.DOLocalJump(Vector3.zero, 3, 1, ballDropDuration).OnComplete((() =>
            {
                MMVibrationManager.Haptic(HapticTypes.LightImpact);
            }));
            yield return new WaitForSeconds(ballDropDuration);
        }
    }
    private GameObject TargetHolder(Collider other)
    {
        Transform root = other.transform.parent.transform.parent;
        var manager = root.GetComponent<BaseballPlayerMain>();
        GameObject targetHolder = manager.placeHolderList.FirstOrDefault(b => b.transform.childCount == 0);
        return targetHolder;
    }
    public void CheckCapacity()
    {
        if (!PlayerPrefs.HasKey("PlayerCapacity"))
        {
            capacity = playerSettings.playerCarryCapacityList[0];
        }
        else
        {
            capacity = PlayerPrefs.GetFloat("PlayerCapacity") switch
            {
                0 => playerSettings.playerCarryCapacityList[0],
                1 => playerSettings.playerCarryCapacityList[1],
                2 => playerSettings.playerCarryCapacityList[2],
                3 => playerSettings.playerCarryCapacityList[3],
                4 => playerSettings.playerCarryCapacityList[4],
                _ => capacity
            };
        }
    }
    private void CheckIncome()
    {
        if (!PlayerPrefs.HasKey("PlayerIncome"))
        {
            _income = playerSettings.incomeList[0];
        }
        else
        {
            _income = PlayerPrefs.GetFloat("PlayerIncome") switch
            {
                0 => playerSettings.incomeList[0],
                1 => playerSettings.incomeList[1],
                2 => playerSettings.incomeList[2],
                3 => playerSettings.incomeList[3],
                4 => playerSettings.incomeList[4],
                _ => _income
            };
        }
    }
}
