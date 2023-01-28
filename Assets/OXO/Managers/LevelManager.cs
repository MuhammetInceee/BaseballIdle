using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

using Sirenix.OdinInspector;

using UnityEngine;


public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public GameObject finishPlatform;
    [HideInInspector]
    public List<GameObject> currentLevelObjectsList;
    //[HideInInspector]
    public List<GameObject> levelPrefabList;

    [Header("Current Level")]
    public GameObject currentLevel;
    public GameObject player;
    [Space]
    public GameObject moneyPrefab;

    public GameObject playerPrefab;

    public int level;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        level = PlayerPrefs.GetInt("Level");
    }
    private void Start()
    {
        SetLevel();
    }
    public void SetLevel()
    {
        ResetLevel();
        ConfettiManager.instance.Stop();
        Spawn();

    }
    public void Spawn()
    {
        if (levelPrefabList.Count == 0)
        {
            Debug.Log($"<color=orange><b>(!) Couldn't find level in the Level List.</b> </color>"); return;
        }

        currentLevel = Instantiate(levelPrefabList[level % levelPrefabList.Count]);
        currentLevel.SetActive(true);
        currentLevelObjectsList.Add(currentLevel);

        CanvasManager.instance.levelText.text = "Level " + (level + 1).ToString();

        if (playerPrefab)
        {
            player = Instantiate(playerPrefab);
            player.transform.position = Vector3.zero;
            player.SetActive(true);
            currentLevelObjectsList.Add(player);

        }
    }
    public void ResetLevel()
    {
        ResetList(currentLevelObjectsList);
    }
    public void IncreaseLevel()
    {
        level++;
        PlayerPrefs.SetInt("Level", level);
    }
    public void ResetList(List<GameObject> list)
    {
        list.ForEach(x => Destroy(x.gameObject));
        list.Clear();
    }
    public void ResetCamera()
    {
        //Camera.main.GetComponent<CameraDisplay>().ResetCamera();
    }
}