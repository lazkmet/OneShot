using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyScript : MonoBehaviour
{
    protected Transform target;
    public int pointValue;
    public bool disabled;
    
    private void Awake()
    {
        target = FindObjectOfType<PlayerMovement>().gameObject.transform;
        disabled = false;
    }
    public virtual void Hit() {
        FindObjectOfType<GameManager>().AddPoints(pointValue);
        Destroy(this.gameObject);
    }
    public virtual void Disable()
    {
        disabled = true;
    }
}
