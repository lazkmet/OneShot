using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossAttack : MonoBehaviour
{
    BossEnemy boss;
    private bool attacking = false;
    private void Awake()
    {
        boss = GetComponentInParent<BossEnemy>();
        this.enabled = false;
    }
    private void OnEnable()
    {
        StartCoroutine("Attack");
    }
    public abstract IEnumerator Attack();
    public abstract void Stun();
    public abstract void Unstun();
}
