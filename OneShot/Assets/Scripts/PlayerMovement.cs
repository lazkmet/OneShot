using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 1.0f;
    public LaserFire laser;
    public GameObject spriteParent;
    public Collider2D playerCollider { get; private set; }
    public Rigidbody2D body { get; private set; }
    public GameObject deathEffect;
    public float speedMultiplier = 1.0f;
    private Vector2 startPos;
    private float previousX = 0;
    private float previousY = 0;
    private SpriteRenderer[] sprites;
    private void Awake()
    {
        playerCollider = GetComponent<Collider2D>();
        body = GetComponent<Rigidbody2D>();
        sprites = spriteParent.GetComponentsInChildren<SpriteRenderer>();
        startPos = this.transform.position;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            laser.TryFire();
        }
        //self destruct button muahahahah
        else if (Input.GetKeyDown(KeyCode.X)) {
            PlayerDeath();
        }
    }
    private void FixedUpdate()
    {
        //Move in particular direction
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        float Xlimiter;
        float Ylimiter;

        //If input is being lifted (magnitude less than previous frame), stop early
        Xlimiter = Mathf.Abs(previousX) > Mathf.Abs(x) ? 0.7f : 0;
        Ylimiter = Mathf.Abs(previousY) > Mathf.Abs(y) ? 0.7f : 0;

        previousX = x;
        previousY = y;

        //Multiply by 1.5 for faster "turn" speed
        x = (Mathf.Abs(x) > Xlimiter) ? Mathf.Clamp(2f * x, -1, 1) : 0;
        y = (Mathf.Abs(y) > Ylimiter) ? Mathf.Clamp(2f * y, -1, 1) : 0;

        Vector2 movement = new Vector2(x, y);
        movement = movement.normalized * speed * speedMultiplier * Time.deltaTime;

        Vector2 newPosition = transform.position;
        newPosition = newPosition + movement;

        body.MovePosition(newPosition);
    }
    public void Reset()
    {
        enabled = true;
        speedMultiplier = 1.0f;
        this.transform.position = startPos;
        playerCollider.enabled = true;
        SetSpriteEnabled(true);
        laser.Reset();
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) {
            collision.gameObject.GetComponent<EnemyScript>().Hit();
            Instantiate(deathEffect, collision.gameObject.transform.position, Quaternion.identity);
            PlayerDeath();
        }
    }
    public void OnParticleCollision(GameObject other)
    {
        //Forced to use a deprecated feature here because of a peculiar interaction. OnParticleCollision triggers when the particles hit
        //the laser (which is designed to destroy them) because it is a child of the player. Unfortunately this caused the particles to
        //destroy the player whenever the laser hit a particle. NOT intended behavior, to be certain.
        ParticleSystem part = other.GetComponent<ParticleSystem>();
        List<ParticleCollisionEvent> events = new List<ParticleCollisionEvent>(part.GetSafeCollisionEventSize());
        part.GetCollisionEvents(gameObject, events);
        bool playerCollision = false;
        foreach (ParticleCollisionEvent coll in events){
            if (coll.colliderComponent != null && coll.colliderComponent.gameObject.layer == LayerMask.NameToLayer("Player")) {
                playerCollision = true;
            }
        }
        //If the collision was actually with the player...
        if (playerCollision) {
            PlayerDeath();
        }
        
    }
    private void SetSpriteEnabled(bool isEnabled = true)
    {
        foreach (SpriteRenderer s in sprites)
        {
            s.enabled = isEnabled;
        }
    }
    public void PlayerDeath() {
        enabled = false;
        playerCollider.enabled = false;
        laser.Reset();
        laser.HideCharge();
        SetSpriteEnabled(false);
        StartCoroutine("Explode");
    }
    private IEnumerator Explode() {
        GameManager manager = FindObjectOfType<GameManager>();
        manager.currentLevel.DisableEnemies();
        manager.currentLevel.speedMultiplier = 0.25f;
        GameObject splode = Instantiate(deathEffect, gameObject.transform);
        yield return new WaitUntil(() => splode == null);
        manager.playerHit();
    }
}
