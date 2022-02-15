using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMovement : MonoBehaviour
{
    public float baseSpeed = 1f;
    public float levelLength;
    public float speedMultiplier = 1f;
    public float spawnY = 0f;
    private ActivationZone stupidZone;
    private Vector3 startPos;
    private EnemySpawnpoint[] enemySpawns = { };
    private int currentSpawnToCheck = 0;
    private void Awake()
    {
        gameObject.SetActive(true);
        startPos = transform.position;
        enemySpawns = GetComponentsInChildren<EnemySpawnpoint>();
        stupidZone = FindObjectOfType<ActivationZone>();
    }

    void Update()
    {
        //moves level down if it has not moved the listed level length
        if (transform.position.y - startPos.y> -levelLength) {
            Vector2 newPosition = transform.position;
            newPosition += baseSpeed * speedMultiplier * Vector2.down * Time.deltaTime;
            transform.position = newPosition;
        }
        //spawns an enemy if its spawnpoint is below the spawn Y level. Steps within the array once per frame
        if (enemySpawns.Length > 0) {
            if (enemySpawns[currentSpawnToCheck].transform.position.y < spawnY) {
                enemySpawns[currentSpawnToCheck].SpawnEnemy();
            }
            currentSpawnToCheck = (currentSpawnToCheck + 1) % enemySpawns.Length;
        }
    }
    public void Reset()
    {
        EnemyScript[] activeEnemies = FindObjectsOfType<EnemyScript>();
        foreach (EnemyScript e in activeEnemies)
        {
            Destroy(e.gameObject);
        }
        speedMultiplier = 1;
        transform.position = startPos;
        stupidZone.GetComponentInParent<Collider2D>().enabled = true;
        foreach (EnemySpawnpoint s in enemySpawns) {
            s.enabled = true;
        }
    }
    public void DisableEnemies() {
        stupidZone.GetComponentInParent<Collider2D>().enabled = false;
        EnemyScript[] enemies = FindObjectsOfType<EnemyScript>();
        foreach (EnemyScript e in enemies) {
            e.Disable();
        }
    }
    private void OnDrawGizmos()
    {
        Vector2 cubeCenter = transform.position;
        cubeCenter.y += levelLength / 2;
        Gizmos.DrawWireCube(cubeCenter, new Vector3(18, levelLength, 1));

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(-9, spawnY, 0), new Vector3(9, spawnY, 0));
    }
}
