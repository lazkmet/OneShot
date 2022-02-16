using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossAttack : MonoBehaviour
{
    protected BossEnemy boss;
    public float idleSeconds = 0f;
    protected virtual void Awake()
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
