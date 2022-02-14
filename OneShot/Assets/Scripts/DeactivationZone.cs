using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivationZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyScript script;
        if (collision.gameObject.TryGetComponent(out script))
        {
            script.Disable();
        }
    }
}
