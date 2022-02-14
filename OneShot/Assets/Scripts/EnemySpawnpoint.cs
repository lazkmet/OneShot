using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnpoint : MonoBehaviour
{
    public GameObject enemyPrefab;
    public virtual void SpawnEnemy()
    {
        if (enabled) {
            Instantiate(enemyPrefab, gameObject.transform);
            enabled = false;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(gameObject.transform.position, 0.5f);
    }
}
