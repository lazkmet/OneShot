using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : EnemyScript
{
    public float hitstun = 0f;
    private enum behavior { IDLE, WAVE, LASER, HOMING, SPAWN }
    private behavior currentState = behavior.IDLE;
    private behavior previousAttack;
    private float idleCountdown = 0;
    private bool stunned = false;
    private void Start()
    {
       //set behavior to idle, freeze screen motion
    }
    public override void Hit()
    {
        
    }
    private void Update()
    {
        switch (currentState)
        {
            case behavior.IDLE:
                if (idleCountdown > 0)
                {
                    idleCountdown -= Time.deltaTime;
                }
                else
                {
                    SelectNextAttack();
                }
                break;
            case behavior.WAVE:
                break;
            case behavior.LASER:
                break;
            case behavior.HOMING:
                break;
            case behavior.SPAWN:
                break;
            default:
                print("broken behavior from idiot programmer");
                break;
        }
    }
    public void IdleForSeconds(float seconds)
    {
        if (seconds >= 0)
        {
            idleCountdown = seconds;
            currentState = behavior.IDLE;
        }
        else
        {
            Debug.LogWarning("Failed to idle: " + seconds + " is less than 0 seconds");
        }
    }
    private void SelectNextAttack() {
        int ignore = (int)previousAttack;
        int nextAttack;
        do {
            nextAttack = Random.Range((int)behavior.WAVE, (int)behavior.SPAWN + 1);
        }
        while (nextAttack != ignore);
        currentState = (behavior)nextAttack;
    }
    public void TryStun(float aHitstun = 0) {
        stunned = true;
        //Disable collider
        StartCoroutine("Stun", aHitstun);
    }
    private IEnumerator Stun(float aHitstun) {
        yield return new WaitForSeconds(aHitstun);
        //Enable collider
        stunned = false;
    }
}
