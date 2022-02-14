using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutSpawnpoint : EnemySpawnpoint
{
    public float speedModifier = 0.5f;
    [SerializeField]
    private BezierPath[] routes;
    public override void SpawnEnemy()
    {
        if (enabled)
        {
            GameObject newObject = Instantiate(enemyPrefab, gameObject.transform);
            BezierFollow bezier = newObject.GetComponent<BezierFollow>();
            bezier.speedModifier = speedModifier;
            bezier.SetRoutes(routes);
            enabled = false;
        }
    }
}
