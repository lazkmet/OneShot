using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDetector : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D collision)
    {
        EnemyScript script;
        if (TryGetComponent<EnemyScript>(out script))
        {
            script.Hit();
        }
    }
    private void OnParticleCollision(GameObject other)
    {
        
    }
}
