using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BluePlayerAnimatorScript : MonoBehaviour
{
    private BaseballPlayerMain BaseballPlayerMain => transform.parent.GetComponent<BaseballPlayerMain>();

    public void Animations()
    {
        GameObject ball = BaseballPlayerMain.animationBall;

        ball.transform.parent = null;
        //Debug.Break();
        ball.transform.DOKill();
        ball.transform.DOScale(ball.transform.localScale * 1.25f, 0.1f).SetEase(Ease.OutElastic);
        ball.transform.DOMove(ball.transform.position + Vector3.left * 100 + Vector3.forward * Random.Range(-10f, 10f) + Vector3.up * Random.Range(25,40),
            30).SetSpeedBased().SetEase(Ease.Linear);
        Destroy(ball, 3);
    }
}
