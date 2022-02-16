using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : EnemyScript
{
    public int maxLives = 3;
    private int currentLives;
    public float iFrames = 0f;
    public GameObject[] doors;
    public BossAttack[] attacks = { };
    public GameObject explosion;
    public GameObject bigExplosion;
    public SpriteRenderer shield;
    public GameObject sprite;
    public SpriteRenderer hurtSprite;
    private GameManager manager;
    private enum behavior { IDLE, ATTACKING}
    private behavior currentState;
    private int previousAttack;
    private float idleCountdown = 0;
    [HideInInspector]
    public bool stunned;
 
    private void Start()
    {
        stunned = false;
        shield.enabled = true;
        manager = FindObjectOfType<GameManager>();
        foreach (BossAttack attack in attacks)
        {
            attack.enabled = false;
        }
        previousAttack = -1;
        currentLives = maxLives;
    }
    public override void Hit()
    {
        if (!shield.enabled && !stunned) {
            if (--currentLives > 0) {
                TryStun(iFrames);
            }
            else {
                TryStun(iFrames);
                StopAllCoroutines();
                StartCoroutine("Die");
            }
        }  
    }
    private void Update()
    {
        switch (currentState)
        {
            case behavior.IDLE:
                if (!stunned) {
                    shield.enabled = true;
                }
                if (idleCountdown > 0)
                {
                    idleCountdown -= Time.deltaTime;
                }
                else
                {
                    SelectNextAttack();
                }
                break;
            case behavior.ATTACKING:
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
        int nextAttack;
        if (attacks.Length > 1)
        {
            do
            {
                nextAttack = Random.Range(0, attacks.Length);
            }
            while (nextAttack == previousAttack);
        }
        else {
            nextAttack = 0;
        }
        previousAttack = nextAttack;
        try {
            shield.enabled = false;
            attacks[nextAttack].enabled = true;
        }
        catch (System.Exception) {
            print("Error: Tried to reference nonexistent behavior");
            Disable();
        }
        currentState = behavior.ATTACKING;
    }
    public void TryStun(float aInvincible = 0) { 
        StartCoroutine("Stun", aInvincible);
    }
    private IEnumerator Stun(float aHitstun) {
        stunned = true;
        hurtSprite.enabled = true;
        foreach (BossAttack b in attacks) {
            b.Stun();
        }
        yield return new WaitForSeconds(aHitstun / 3);
        hurtSprite.enabled = false;
        yield return new WaitForSeconds(aHitstun * 2 / 3);
        foreach (BossAttack b in attacks)
        {
            b.Unstun();
        }
        stunned = false;
    }
    public override void Disable() {
        disabled = true;
        IdleForSeconds(float.MaxValue);
    }
    public override void Enable()
    {
        StartCoroutine("DelayedStart");
    }
    public IEnumerator DelayedStart() {
        yield return new WaitForSeconds(2);
        disabled = false;
        IdleForSeconds(0);
        print("startFirstAttack");
        //FindObjectOfType<LevelMovement>().speedMultiplier = 0; Unnecessary if boss is at end of level
    }
    public IEnumerator OpenDoors(bool opening = true) {
        float currentTime = 0f;
        while (currentTime < 0.75f) {
            foreach (GameObject door in doors) {
                Vector3 currentScale = door.transform.localScale;
                currentScale.y = opening ? Mathf.Lerp(0, 1, currentTime / 0.75f) : Mathf.Lerp(1, 0, currentTime / 0.75f);
                door.transform.localScale = currentScale;
            }
            currentTime += Time.deltaTime;
            yield return new WaitWhile(() => stunned == true);
        }
        foreach (GameObject door in doors)
        {
            Vector3 currentScale = door.transform.localScale;
            currentScale.y = opening ? 1 : 0;
            door.transform.localScale = currentScale;
        }
    }
    public IEnumerator Die() {
        AudioManager soundManager = FindObjectOfType<AudioManager>();

        //Destroy all spawned scouts
        Scout[] bossEnemies = FindObjectsOfType<Scout>();
        foreach (Scout s in bossEnemies) {
            Destroy(s.emitter);
            Instantiate(explosion, s.gameObject.transform.position, Quaternion.identity);
            s.Hit();
        }

        //Spawn a large number of little explosions
        int numExplosions = Random.Range(25, 40);
        float[] delays = new float[Random.Range(6,10)];
        for (int i = 0; i < delays.Length; i++) {
            delays[i] = Random.Range(0, 2.5f);
        }
        manager.delayedWin = true;
        for (int i = 0; i < numExplosions; i++) {
            Vector2 directionToAdd = Random.insideUnitCircle * 4;
            if (directionToAdd.y > 0) {
                directionToAdd.y = directionToAdd.y * -1;
            }
            GameObject newExp = Instantiate(explosion, ((Vector2)gameObject.transform.position + directionToAdd), Quaternion.identity);
            ParticleSystem[] systems = newExp.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem p in systems) {
                ParticleSystem.MainModule expStats = p.main;
                expStats.startDelay = Random.Range(0, 2.5f);
            }
            print("Created Explosion");
        }
        for (float time = 0; time < 2.6; time += Time.deltaTime) {
            for (int i = 0; i < delays.Length; i++) {
                if (delays[i] < time) {
                    print("boom");
                    //Play explosion sound
                    delays[i] = 999f;
                }
            }
            yield return null;
        }

        Instantiate(bigExplosion, gameObject.transform.position, Quaternion.identity);
        manager.AddPoints(pointValue);
        sprite.SetActive(false);
        gameObject.GetComponent<Collider2D>().enabled = false;
        //play big explosion sound
        yield return new WaitForSeconds(2);
        manager.Victory();
        Destroy(gameObject);
    }
}
