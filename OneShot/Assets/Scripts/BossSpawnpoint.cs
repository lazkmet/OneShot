using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawnpoint : EnemySpawnpoint
{
    public BezierPath[] initialPath = { };
    public Transform permanentCenter;
    public Transform pathEnd;
    public GameObject pathPrefab;
    public override void SpawnEnemy()
    {
        GameObject newScout = Instantiate(enemyPrefab, gameObject.transform);
        GameObject pathParent = Instantiate(pathPrefab, pathEnd.position, Quaternion.identity, gameObject.transform);
        newScout.GetComponent<Scout>().Disable();
        OneTimeFollow otf = newScout.GetComponent<OneTimeFollow>();
        otf.SetRoutes(initialPath);
        BezierPath[] permanentPaths = pathParent.GetComponentsInChildren<BezierPath>();
        otf.permanentPath.SetRoutes(permanentPaths);
        newScout.GetComponent<Scout>().Invoke(nameof(Scout.Enable), 1);
    }
    public void RandomizeTarget() {
        if (initialPath.Length > 0) {
            Vector2 newCenter = (Vector2)permanentCenter.position + Random.insideUnitCircle;
            pathEnd.position = newCenter;
            initialPath[initialPath.Length - 1].controlPoints[3].position = pathEnd.position;
        }
    }
}
