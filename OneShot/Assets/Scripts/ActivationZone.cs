using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationZone : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        EnemyScript script;
        if (collision.gameObject.TryGetComponent(out script)) {
            script.Enable();
        }
    }
}
