using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyScript : MonoBehaviour
{
    private Transform target;
    public int pointValue;
    
    private void Awake()
    {
        target = FindObjectOfType<PlayerMovement>().gameObject.transform;
    }
    private void Update()
    {
        
    }
    public virtual void Hit() {
        FindObjectOfType<GameManager>().AddPoints(pointValue);
        Destroy(this.gameObject);
    }
    public void Disable()
    {
        
    }
}
