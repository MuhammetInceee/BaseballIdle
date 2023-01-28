using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MuhammetInce.DesignPattern.Singleton;
using UnityEngine.Rendering.Universal;

public class TaskManager : LazySingleton<TaskManager>
{
    public List<string> enumerators;
    public short workingTaskIndex;

    public string tutorialEndText;
    
    public TutorialManager tutorialManager;
    [Range(0f, 0.5f)]
    public float interval = 0.05f;

    public bool isComplete => PlayerPrefs.GetInt("IsComplete", 0) == 0 ? false : true;
    private void Awake()
    {
        byte s = 16;
    }
    private IEnumerator Start()
    {
        if (isComplete)
        {
            Destroy(tutorialManager.cursor.gameObject);
            yield break;
        }
         
        enumerators.Add("GoBallMachine");
        enumerators.Add("GoTwin");
        enumerators.Add("DoneTask");

        for (int i = 0; i < enumerators.Count; i++)
        {
            workingTaskIndex = (short)i;
            yield return StartCoroutine(enumerators[i]);
        }

    }

    public IEnumerator GoBallMachine()
    {
        print(PlayerCollide.Instance.playerHandObjects.Count);
        while (PlayerCollide.Instance.playerHandObjects.Count == 0)
        {
            if(Vector3.Distance(tutorialManager.ballMachine.transform.position,tutorialManager.PlayerTransform)>2f)
                tutorialManager.cursor.DODynamicLookAt(tutorialManager.ballMachine.transform.position, 0f ,AxisConstraint.Y);
            tutorialManager.SetCursorRadius();
            yield return new WaitForSeconds(interval);
        }
    }
    
    public IEnumerator GoTwin()
    {
        
        int count = PlayerCollide.Instance.playerHandObjects.Count;

        while (PlayerCollide.Instance.playerHandObjects.Count >= count) 
        {
            tutorialManager.cursor.DODynamicLookAt(tutorialManager.twinPlayer.transform.position, 0f ,AxisConstraint.Y);
            tutorialManager.SetCursorRadius();
            yield return new WaitForSeconds(interval);
        }
    }

    public IEnumerator DoneTask()
    {
        Destroy(tutorialManager.cursor.gameObject);
        MessageManager.instance.Show(tutorialEndText);
        PlayerPrefs.SetInt("IsComplete",1);
        PlayerPrefs.Save();
        
        yield return null;
    }
}
