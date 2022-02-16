using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollow : MonoBehaviour
{
    public Camera mainCamera;
    private SpriteRenderer image;
    private Vector3 originalScale;

    private void Awake()
    {
        mainCamera = FindObjectOfType<Camera>();
        image = GetComponent<SpriteRenderer>();
        originalScale = gameObject.transform.localScale;
    }
        
    void Update()
    {
        gameObject.transform.position = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0)) {
            gameObject.transform.localScale *= 1.1f;
        }
        else if (Input.GetMouseButtonUp(0)) {
            gameObject.transform.localScale = originalScale;
        }
    }
    public void Hide() {
        Cursor.visible = true;
        image.enabled = false;
    }
    public void Show() {
        Cursor.visible = false;
        image.enabled = true;
    }
}
