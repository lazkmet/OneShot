using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 1.0f;
    public LaserFire laser;
    public Rigidbody2D body { get; private set; }
    public float speedMultiplier = 1.0f;
    private Vector2 startPos;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        startPos = this.transform.position;
    }
    void Update()
    { 
        if (Input.GetMouseButtonDown(0)) {
            laser.TryFire();
        }
    }
    private void FixedUpdate()
    {
        //Move in particular direction
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

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
        laser.Reset();
    }
}
