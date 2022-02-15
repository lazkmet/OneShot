using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesDisplay : MonoBehaviour
{
    public Sprite life;
    private int currentChildren = 0;
    private Image[] lifeImages = { };

    private void Awake()
    {
        currentChildren = this.gameObject.transform.childCount;
        lifeImages = new Image[currentChildren];
        for (int i = 0; i < lifeImages.Length; i++) {
            lifeImages[i] = transform.GetChild(i).gameObject.GetComponent<Image>();
        }
    }
    public void UpdateLives(int currentLives = 0)
    {
        for (int i = 0; i < lifeImages.Length; i++){
            if (i < currentLives) {
                lifeImages[i].enabled = true;
                lifeImages[i].sprite = life;
            }
            else {
                lifeImages[i].enabled = false;
            }
        }
    }
}
