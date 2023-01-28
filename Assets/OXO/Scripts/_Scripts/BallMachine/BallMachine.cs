using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using System.Linq;
using MuhammetInce.DesignPattern.Singleton;

public class BallMachine : MonoBehaviour
{
    private bool _canGenerateBall;
    private float _generateDurationHelper;
    public float umut;

    public SetBallToPlayer setBallToPlayer;

    [SerializeField] private List<GameObject> placeHolder = new List<GameObject>();

    [Header("About Game Mechanic Values:")]
    public int createdBallCount;
    [SerializeField] private int maxStackedBall;
    
    [Header("About Ball and Object Related to Them: "), Space]
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Transform ballSpawnPos;
    [SerializeField] private GameObject spawnHolderPrefab;
    [SerializeField] private GameObject ballHolder;
    [SerializeField] private GameObject spawnLocations;
    
    [Header("About Organizing Floats: "), Space]
    [SerializeField] private float ballMoveDelay;
    [SerializeField] private float stackDistance;
    public float generateDuration;


    
    private WaitForSeconds Wait => new WaitForSeconds(generateDuration);
    private void Awake() => AwakeInit();
    private void Update() => UpdateInit();
    
    private void AwakeInit()
    {
        _generateDurationHelper = generateDuration + ballMoveDelay;
        
    }

    private void UpdateInit()
    {
        _generateDurationHelper -= Time.deltaTime;
        
        
        if (_generateDurationHelper <= 0)
        {
            StartCoroutine(GenerateBallRoutine());
            _generateDurationHelper = generateDuration + ballMoveDelay;
        }
        
        GeneratePlaceHolder();
    }
    
    private IEnumerator GenerateBallRoutine()
    {
        yield return Wait;
        if (createdBallCount >= maxStackedBall) yield break;
        GameObject targetHolder = placeHolder.FirstOrDefault(b => b.transform.childCount == 0);
        GameObject createdBall = Instantiate(ballPrefab, ballSpawnPos.position, Quaternion.identity, targetHolder!.transform);
        createdBallCount++;
        
        createdBall.transform.DOMove(targetHolder!.transform.position, ballMoveDelay).SetEase(Ease.Linear).OnComplete(() =>
        {
            createdBall.GetComponent<SphereCollider>().enabled = true;
            createdBall.transform.DOKill(true);
            setBallToPlayer.stackedBall.Add(createdBall);
        });
    }
    private void GeneratePlaceHolder()
    {
        if (createdBallCount % placeHolder.Count == 0 && createdBallCount != 0)
        {
            GameObject lastChild = spawnLocations.transform.GetChild(spawnLocations.transform.childCount - 1).gameObject;
            Vector3 position = lastChild.transform.position;
            Vector3 spawnPos = new Vector3(position.x, position.y + stackDistance, position.z);
            
            GameObject go = Instantiate(spawnHolderPrefab, spawnPos, Quaternion.identity, spawnLocations.transform);
            go.transform.localEulerAngles = Vector3.zero;

            for (int i = 0; i < go.transform.childCount; i++)
            {
                go.transform.GetChild(i).gameObject.SetActive(true);
                placeHolder.Add(go.transform.GetChild(i).gameObject);
            }
        }
    }

    public void LevelUp()
    {
        generateDuration -= umut;
        
        if (generateDuration <= 0.1f)
        {
            generateDuration = 0.1f;
            PlayerPrefs.SetFloat("GenerateDuration", 0.1f);
        }
        else
        {
            PlayerPrefs.SetFloat("GenerateDuration", generateDuration);
        }
        
        
    }
}
