using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMovement : MonoBehaviour
{
    public float baseSpeed = 1f;
    public float levelLength;
    public float speedMultiplier = 1f;
    private Vector3 startPos;
    private void Awake()
    {
        gameObject.SetActive(true);
        startPos = transform.position;
        Reset();
    }

    void Update()
    {
        if (transform.position.y > -levelLength) {
            Vector2 newPosition = transform.position;
            newPosition += baseSpeed * speedMultiplier * Vector2.down * Time.deltaTime;
            transform.position = newPosition;
        }
    }
    public void Reset()
    {
        speedMultiplier = 1;
        transform.position = startPos;
    }
}
