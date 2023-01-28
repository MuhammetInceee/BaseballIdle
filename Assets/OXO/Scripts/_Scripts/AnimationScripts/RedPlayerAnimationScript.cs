using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RedPlayerAnimationScript : MonoBehaviour
{

    private BaseballPlayerMain BaseballPlayerMain => transform.parent.GetComponent<BaseballPlayerMain>(); 
    
    public void Animations()
    {

        GameObject ball = BaseballPlayerMain.animationBall;
        
        ball.transform.parent = null;

        ball.transform.SetParent(BaseballPlayerMain.BallHitHolder.transform);
        
        ball.transform.DOLocalJump(Vector3.zero, 1f,1,1f);

    }
}
