using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }
}
