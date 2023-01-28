using System.Collections.Generic;
using System.Collections;
using Dreamteck.Splines;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;
using UnityEngine;
using TMPro;

public class BaseballPlayerMain : MonoBehaviour
{
    private bool _canGetBall = true;
    private static readonly int CanAnim = Animator.StringToHash("canAnim");
    private Vector3 _currentRot;

    public ParticleSystem levelUpAnim;
    public GameObject animationBall;
    public bool isPushUp;
    [SerializeField] private float animDur;
    
    [Header("About Ball Side: ")]
    public List<GameObject> stackBallList = new List<GameObject>();
    public List<GameObject> placeHolderList = new List<GameObject>();

    [Header("About Money Side: ")] 
    public List<GameObject> stackMoneyList = new List<GameObject>();
    public List<GameObject> moneyPlaceHolderList = new List<GameObject>();

    [Header("About Level System: "), Space]
    public float currentXp;
    public int currentLevel;
    public float nextLevelXpAmount;
    public float expPerSecond;
    public float expPerAnimation;
    
    [Header("About Needed Things: "), Space]
    public GameObject moneyWithColliderPrefab;
    public Animator baseballPlayerAnimator;
    public GameObject ballGetPosition;

    [Header("About Baseball Player Level Canvas: "), Space]
    public TextMeshProUGUI levelText;
    public Slider slider;

    private void Start() => StartInit();
    private void Update() => UpdateInit();
    
    private void StartInit()
    {
        currentLevel = PlayerPrefs.GetInt("baseballLevel" + gameObject.GetInstanceID(), 1);

        slider.minValue = 0;
        slider.maxValue = nextLevelXpAmount;
        
        
        
        _currentRot = transform.eulerAngles;
        
        baseballPlayerAnimator = transform.GetChild(2).GetComponent<Animator>();

        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            placeHolderList.Add(transform.GetChild(0).GetChild(i).gameObject);
        }

        for (int i = 0; i < transform.GetChild(3).childCount; i++)
        {
            moneyPlaceHolderList.Add(transform.GetChild(3).GetChild(i).gameObject);
        }

        ballGetPosition = transform.GetChild(1).gameObject;
        
    }

    private void UpdateInit()
    {
        slider.value = currentXp;
        levelText.text = currentLevel.ToString();
        
        if(!GameManager.instance.isMatchScreen)
            currentXp += expPerSecond * Time.deltaTime;

        LevelUpWithWait();
        GetBallForAnim();
    }

    [Header("About Runner Player Animation Duration: "), Space] 
    [SerializeField] private bool isRunnerPlayer;
    [SerializeField] private float runDur;

    [Header("About Begin Twins: "), Space] 
    public bool isTwin;
    [SerializeField] private float canMoveDuration;

    private static readonly int LetsStrike = Animator.StringToHash("letsStrike");
    private static readonly int LetsPitch = Animator.StringToHash("letsPitch");

    private GameObject HandHolder => GameObject.FindWithTag("HandHolder");
    public GameObject BallHitHolder => GameObject.FindWithTag("BallHitArea");
    public Animator BlueAnimator => transform.GetChild(4).GetComponent<Animator>();
    private SplineFollower Follower => transform.GetChild(2)!.GetComponent<SplineFollower>();
    private GameObject RedPlayer => transform.GetChild(2).gameObject;
    private GameObject BluePlayer => transform.GetChild(4).gameObject;
    
    private void GetBallForAnim()
    {
        if(!_canGetBall) return;
        if(stackBallList.Count == 0) return;
        GameObject lastBall = stackBallList[^1];
        
        _canGetBall = false;
        lastBall.transform.parent = ballGetPosition.transform;
        
        lastBall.transform.DOLocalJump(Vector3.zero, 1f, 1, 1f).OnComplete(() =>
        {
            stackBallList.Remove(lastBall);
            Destroy(lastBall);

            if (isRunnerPlayer)
            {
                Follower.Restart();
                Follower.enabled = true;
                baseballPlayerAnimator.SetBool(CanAnim, true);
                StartCoroutine(TylerDurden());
            }

            else if (isTwin)
            {
                animationBall =  Instantiate(lastBall, HandHolder.transform.position, Quaternion.identity, HandHolder.transform);
                animationBall.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                baseballPlayerAnimator.SetBool(LetsStrike, true);
                StartCoroutine(JasonStatham());
                StartCoroutine(BatPlayerCanMove());
            }
            
            else
            {
                baseballPlayerAnimator.SetBool(CanAnim, true);
                StartCoroutine(PlayerAnimWithBall());
            }
            
        });
    }
    
    private IEnumerator PlayerAnimWithBall()
    {
        yield return new WaitForSeconds(animDur);
        
        //SmoothXpUpgrade(currentXp, expPerAnimation);
        currentXp += expPerAnimation;
        _canGetBall = true;
        baseballPlayerAnimator.SetBool(CanAnim, false);
        transform.DORotate(_currentRot, 0.2f);
        if(isPushUp)
            baseballPlayerAnimator.gameObject.transform.DORotate(new Vector3(0,-90,0), 0.2f);
        else if(!isPushUp)
            baseballPlayerAnimator.gameObject.transform.DORotate(Vector3.zero, 0.2f);
        StartCoroutine(GenerateMoneyCoroutine());
    }

    private IEnumerator TylerDurden()
    {
        yield return new WaitForSeconds(runDur);
        SmoothXpUpgrade(currentXp, expPerAnimation);
        _canGetBall = true;
        baseballPlayerAnimator.SetBool(CanAnim, false);
        Follower.enabled = false;
        StartCoroutine(GenerateMoneyCoroutine());
    }

    private IEnumerator JasonStatham()
    {
        yield return new WaitForSeconds(8);
        SmoothXpUpgrade(currentXp, expPerAnimation);
        _canGetBall = true;
        baseballPlayerAnimator.SetBool(LetsStrike, false);
        RedPlayer.transform.eulerAngles = new Vector3(0,180,0);
        BlueAnimator.SetBool(LetsPitch, false);
        BluePlayer.transform.eulerAngles = new Vector3(0,0,0);
        StartCoroutine(GenerateMoneyCoroutine());
    }

    private IEnumerator BatPlayerCanMove()
    {
        yield return new WaitForSeconds(canMoveDuration);
        BlueAnimator.SetBool(LetsPitch, true);
    }

    private IEnumerator GenerateMoneyCoroutine()
    {
        yield return null;
        
        GameObject createdMoney = Instantiate(moneyWithColliderPrefab, ballGetPosition.transform.position, Quaternion.identity);
        Collider createdMoneyCollider = createdMoney.GetComponent<Collider>();
        GameObject targetHolder = moneyPlaceHolderList.FirstOrDefault(m => m.transform.childCount == 0);

        createdMoneyCollider.enabled = false;
        createdMoney.transform.DOMove(targetHolder!.transform.position, 1f).OnComplete(() =>
        {
            stackMoneyList.Add(createdMoney);
            createdMoney.transform.SetParent(targetHolder.transform);
            createdMoneyCollider.enabled = true;
        });

    }
    private void LevelUpWithWait()
    {
        if (currentXp >= nextLevelXpAmount)
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        currentLevel++;
        currentXp = 0;
        levelUpAnim.Play();
        StartCoroutine(AnimCloser());

        PlayerPrefs.SetInt("baseballLevel" + gameObject.GetInstanceID(), currentLevel);
    }

    private void SmoothXpUpgrade(float xp, float increasedXp)
    {
        DOTween.To(() => xp, (b) =>  xp = b, xp+increasedXp, 0.2f);
    }

    private IEnumerator AnimCloser()
    {
        yield return new WaitForSeconds(2f);
        levelUpAnim.Stop();
    }
}
