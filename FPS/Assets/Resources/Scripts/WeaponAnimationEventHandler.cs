using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimationEventHandler : MonoBehaviour
{
    Weapon weapon;

    void Awake()
    {
        weapon = GetComponentInParent<Weapon>();
    }

    public void PlayFootstep()
    {
        weapon.player.PlayFootstep();
    }

    public void ReturnToIdle()
    {
        weapon.ChangeState(WeaponState.Idle);
    }
}
