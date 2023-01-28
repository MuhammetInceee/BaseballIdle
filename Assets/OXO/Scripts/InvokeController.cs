using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using System;
using System.Linq;

public class InvokeController : MonoBehaviour
{
   

    public List<GameObject> list;
    public UnityEvent invokeEvent;
    
    [Header("Settings")]
    public float delay;
    public float delayForTasks;
    private void Start()
    {
        StartCoroutine(Do());
    }


    void SomeListener(float f)
    {
        Debug.Log("Listened to change on value " + f); //prints "Listened to change on value 3.14"
    }
    private IEnumerator Do()
    {
        for (int i = 0; i < list.Count; i++)
        {
            StartCoroutine(WorkAnotherTask(i));
            yield return new WaitForSeconds(delay);

        }
    }

    private IEnumerator WorkAnotherTask(int i)
    {
        for (int j = 0; j < invokeEvent.GetPersistentEventCount(); j++)
        {
            var go = invokeEvent.GetPersistentTarget(j) as MonoBehaviour;
            if (go)
            {
                var ga = list[i].GetComponents<MonoBehaviour>().Where(x => x.GetType() == go.GetType()).FirstOrDefault();
                ga.Invoke(invokeEvent.GetPersistentMethodName(j), j);
            }
            else
            {

            }
            yield return new WaitForSeconds(delayForTasks);
        }
    }
}
