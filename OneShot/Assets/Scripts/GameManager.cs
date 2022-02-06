using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerMovement player;
    public LevelMovement currentLevel;
    private void Awake()
    {
        FindObjects();
        Reset();
    }
    private void FindObjects() {
        player = FindObjectOfType<PlayerMovement>();
        currentLevel = FindObjectOfType<LevelMovement>();
    }
    private void Reset()
    {
        player.Reset();
        currentLevel.Reset();
    }
}
