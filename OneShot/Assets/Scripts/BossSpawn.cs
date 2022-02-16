using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawn : BossAttack
{
    public BossSpawnpoint[] spawnpoints = { };
    public SpriteMask[] masks = { };
    public override IEnumerator Attack() {
        foreach (BossSpawnpoint s in spawnpoints) {
            s.RandomizeTarget();
        }

        yield return boss.OpenDoors();
        foreach (SpriteMask m in masks) {
            m.enabled = true;
        }
        foreach (BossSpawnpoint s in spawnpoints) {
            s.SpawnEnemy();
        }
        yield return new WaitForSeconds(1);
        foreach (SpriteMask m in masks)
        {
            m.enabled = false;
        }
        yield return boss.OpenDoors(false);
        boss.IdleForSeconds(idleSeconds);
        this.enabled = false;
        
    }
    public override void Stun() { 
    
    }
    public override void Unstun()
    {
     
    }
}
