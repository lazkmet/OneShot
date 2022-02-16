using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(MenuManager))]
public class GameManager : MonoBehaviour
{
    public int points;
    private int maxPoints = 0; 
    public PlayerMovement player;
    public LevelMovement currentLevel;
    private MenuManager menus;
    public int gameOverIndex;
    public int winScreenIndex;
    [HideInInspector]
    public int currentLives;
    public int maxLives;
    [HideInInspector]
    public bool delayedWin = false;
    private bool suicide = false;
    public TextMeshProUGUI sacrificeMessage;
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
    public void Reset()
    {
        currentLives = maxLives;
        maxPoints = 0;
        RestartLevel();
    }
    public void playerHit() {
        if (delayedWin == false)
        {
            if (--currentLives > 0)
            {
                RestartLevel();
            }
            else
            {
                GameOver();
            }
        }
        suicide = true;
    }
    public void RestartLevel() {
        points = 0;
        delayedWin = false;
        suicide = false;
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
    public void Victory() {
        menus.UpdateDisplay(maxPoints);
        if (suicide)
        {
            print("win by suicide");
            sacrificeMessage.gameObject.SetActive(true);
        }
        else { 
            sacrificeMessage.gameObject.SetActive(false); 
        }
        menus.SetActiveScreen(winScreenIndex);
        player.enabled = false;
        player.SetSpriteEnabled(false);
    }
    private void GameOver() {
        menus.UpdateDisplay(maxPoints);
        menus.SetActiveScreen(gameOverIndex);
        currentLevel.speedMultiplier = 0;
        currentLevel.DisableEnemies();
    }
}
