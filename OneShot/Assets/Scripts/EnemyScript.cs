using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyScript : MonoBehaviour
{
    [HideInInspector]
    public Transform target;
    public int pointValue;
    public bool disabled;
    
    private void Awake()
    {
        target = FindObjectOfType<PlayerMovement>().gameObject.transform;
        Disable();
    }
    public virtual void Hit() {
        FindObjectOfType<GameManager>().AddPoints(pointValue);
        Destroy(gameObject);
    }
    public abstract void Disable();
    public abstract void Enable();
}
