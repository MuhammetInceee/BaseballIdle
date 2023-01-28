using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using MoreMountains.NiceVibrations;

public class SetBallToPlayer : MonoBehaviour
{
    public BallMachine ballMachine;
    public PlayerCollide playerCollide;
    
    public List<GameObject> stackedBall;
    public float ballReachTime;

    private bool ase;


    private void Update()
    {
        print(ase);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerCollide.CheckCapacity();
            ase = true;
            StartCoroutine(SetBall());

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ase = false;
        }
    }

    private IEnumerator SetBall()
    {
        while (ase)
        {
            if (playerCollide.playerHandObjects.Count < playerCollide.capacity)
            {
                yield return new WaitForSeconds(ballReachTime);
                if (stackedBall.Count != 0)
                {
                    GameObject targetObj = stackedBall[^1];
                    ballMachine.createdBallCount--;
                    GameObject targetPos = playerCollide.handStackHolderList.FirstOrDefault(b => b.transform.childCount == 0);
                    if (targetPos != null) targetObj.transform.SetParent(targetPos.transform);
                    targetObj.transform.DOLocalJump(Vector3.zero, 1f, 1, ballReachTime).OnComplete(() =>
                    {
                        playerCollide.playerHandObjects.Add(targetObj);
                        targetObj.GetComponent<SphereCollider>().enabled = false;
                        stackedBall.Remove(targetObj);
                        MMVibrationManager.Haptic(HapticTypes.LightImpact);
                    });
                }
            }

            yield return null;
        }
        
    }
}
