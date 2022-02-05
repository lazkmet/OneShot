using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 1.0f;
    public LaserFire laser;
    private float speedMultiplier = 1.0f;
    private Vector2 startPos;

    private void Awake()
    {
        startPos = this.transform.position;
    }
    void Update()
    {
            //Move in particular direction
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");

            Vector2 movement = new Vector2(x, y);
            movement = movement.normalized * speed * speedMultiplier * Time.deltaTime;

            Vector2 newPosition = transform.position;
            newPosition = newPosition + movement;

            transform.position = newPosition;

        if (Input.GetMouseButtonDown(0)) {
            laser.TryFire();
        }
    }
    public void Reset()
    {
        speedMultiplier = 1.0f;
        this.transform.position = startPos;
    }
}
