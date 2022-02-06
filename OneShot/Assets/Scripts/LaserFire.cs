using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserFire : MonoBehaviour
{
    public Camera mainCamera;
    public float baseDuration = 2.0f;
    public float baseCooldown = 6.0f;
    //public PowerMeter meter;
    private Vector3 targetDirection;
    private float currentMaxCooldown = 0;
    private float cooldownTimer = 0;
    private float currentDuration = 0;
    private float laserMaxWidth;
    private PlayerMovement parentScript;
    private LineRenderer laserRenderer;
    private LaserDetector laserCollisionHandler;
    private void Awake()
    {
        parentScript = GetComponentInParent<PlayerMovement>();
        laserRenderer = GetComponent<LineRenderer>();
        laserCollisionHandler = GetComponent<LaserDetector>();
        laserMaxWidth = laserRenderer.widthMultiplier;
        Reset();
    }
    private void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
        else { 
            cooldownTimer = 0; 
        }
        //meter.setCooldown(currentMaxCooldown, cooldownTimer);
    }
    public void Reset()
    {
        StopAllCoroutines();
        currentMaxCooldown = baseCooldown;
        currentDuration = baseDuration;
        cooldownTimer = 0;
        laserRenderer.enabled = false;
        laserCollisionHandler.enabled = false;
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }
    public void TryFire() {
        if (cooldownTimer <= 0) {
            //stop movement
            parentScript.enabled = false;
            Vector2 targetPoint = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            transform.up = targetPoint - (Vector2)transform.position;
            StartCoroutine("Fire");
        }
    }
    private IEnumerator Fire() {
        //Firing duration (currentDuration) is split into 4 parts: Delay, Expand, Hold, Retract.
        //Delay = 20% of duration, Expand = 2% of duration, Hold = 80% of duration, Retract = 18% of duration. (120% total)
        //e.g. 10s duration, delay for 0.2s, expand for 1s, hold for 7s, retract for 2s
        
        //Delay
        float currentTime = 0.2f * currentDuration;
        yield return new WaitForSeconds(currentTime);

        //Expand (trigger screenshake/kickback?)
        currentTime = 0.02f * currentDuration;
        float width = 0;
        laserRenderer.enabled = true;
        for (float timer = 0; timer < currentTime; timer += Time.deltaTime) {
            width = Mathf.Lerp(0, laserMaxWidth, timer / currentTime);
            laserRenderer.widthMultiplier = width;
            yield return null;
        }
        laserRenderer.widthMultiplier = laserMaxWidth;

        //Hold (Activate Detection)
        currentTime = 0.8f * currentDuration;
        laserCollisionHandler.enabled = true;
        yield return new WaitForSeconds(currentTime);

        //Retract(Deactivate Detection)
        currentTime = 0.18f * currentDuration;
        laserCollisionHandler.enabled = false;
        width = laserMaxWidth;
        for (float timer = 0; timer < currentTime; timer += Time.deltaTime)
        {
            width = Mathf.Lerp(laserMaxWidth, 0, timer / currentTime);
            laserRenderer.widthMultiplier = width;
            yield return null;
        }
        laserRenderer.widthMultiplier = 0;
        laserRenderer.enabled = false;
        cooldownTimer = currentMaxCooldown;
        parentScript.enabled = true;
    }
}
