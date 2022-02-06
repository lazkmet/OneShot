using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Code slightly adapted from https://www.youtube.com/watch?v=3UO-1suMbNc 
//to use manually moving objects instead of a moving camera
//and to loop vertically instead of horizontally
public class LoopingBackground : MonoBehaviour
{
    public Camera mainCamera;
    public float loopSpeed = 1f;
    private Vector2 screenBounds;
    private Transform[] children;
    public GameObject image;
    public float choke = 0f;
    private Vector2 newPosition;
    private void Awake()
    {
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        LoadChildObjects(image);
    }
    private void LoadChildObjects(GameObject obj) {
        float objectHeight = obj.GetComponent<SpriteRenderer>().bounds.size.y - choke;
        int childrenNeeded = (int)Mathf.Ceil(screenBounds.y * 2 / objectHeight);
        GameObject clone = Instantiate(obj) as GameObject;
        for (int i = 0; i <= childrenNeeded; i++) {
            GameObject c = Instantiate(clone) as GameObject;
            c.transform.SetParent(obj.transform);
            c.transform.position = new Vector2(obj.transform.position.x, objectHeight * i);
            c.name = obj.name + i;
        }
        Destroy(clone);
        Destroy(obj.GetComponent<SpriteRenderer>());
        children = obj.GetComponentsInChildren<Transform>();
    }
    private void Update()
    {
        for (int i = 1; i < children.Length; i++) {
            Transform current = children[i];
            newPosition = current.position;
            newPosition.y -= loopSpeed * Time.deltaTime;
            current.position = newPosition;
        }
    }
    private void LateUpdate()
    {
        RepositionChildObjects(image);
    }
    private void RepositionChildObjects(GameObject obj) {
        children = obj.GetComponentsInChildren<Transform>();
        if (children.Length > 1) {
            GameObject firstChild = children[1].gameObject;
            GameObject lastChild = children[children.Length - 1].gameObject;
            float objHeight = lastChild.GetComponent<SpriteRenderer>().bounds.size.y - choke;
            if (mainCamera.transform.position.y + screenBounds.y > lastChild.transform.position.y + objHeight)
            {
                print("loopUp");
                firstChild.transform.SetAsLastSibling();
                firstChild.transform.position = new Vector3(lastChild.transform.position.x, lastChild.transform.position.y + objHeight, lastChild.transform.position.z);
            }
            else if (mainCamera.transform.position.y - screenBounds.y < firstChild.transform.position.y - choke)
            {
                print("loopDown");
                lastChild.transform.SetAsFirstSibling();
                lastChild.transform.position = new Vector3(firstChild.transform.position.x, firstChild.transform.position.y - objHeight, firstChild.transform.position.z);
            }
        }
    }
}
