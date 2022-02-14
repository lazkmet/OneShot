using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDetector : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        if (enabled == true) {
            EnemyScript script;
            if (other.gameObject.TryGetComponent(out script))
            {
                script.Hit();
            }
        }
    }
}
