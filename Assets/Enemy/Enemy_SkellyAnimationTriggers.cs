using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_SkellyAnimationTriggers : MonoBehaviour
{
    private EnemySkelly enemy => GetComponentInParent<EnemySkelly>();

    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }
}
