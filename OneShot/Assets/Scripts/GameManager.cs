using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MenuManager))]
public class GameManager : MonoBehaviour
{
    public int points;
    private int maxPoints = 0; 
    public PlayerMovement player;
    public LevelMovement currentLevel;
    private MenuManager menus;
    public int gameOverIndex;
    [HideInInspector]
    public int currentLives;
    public int maxLives;
    private void Awake()
    {
        FindObjects();
    }
    private void Start()
    {
        Reset();
    }
    private void FindObjects() {
        player = FindObjectOfType<PlayerMovement>();
        currentLevel = FindObjectOfType<LevelMovement>();
        menus = GetComponent<MenuManager>();
    }
    private void Reset()
    {
        currentLives = maxLives;
        maxPoints = 0;
        RestartLevel();
    }
    public void playerHit() {
        if (--currentLives > 0)
        {
            RestartLevel();
        }
        else {
            GameOver();
        }
    }
    public void RestartLevel() {
        points = 0;
        player.Reset();
        currentLevel.Reset();
        menus.Reset();
        menus.UpdateDisplay(points, currentLives);
    }
    public void AddPoints(int amount) {
        points += amount;
        if (points > maxPoints) {
            maxPoints = points;
        }
        menus.UpdateDisplay(points);
    }
    private void GameOver() {
        menus.UpdateDisplay(maxPoints);
        menus.SetActiveScreen(gameOverIndex);
        currentLevel.speedMultiplier = 0;
        currentLevel.DisableEnemies();
    }
}
