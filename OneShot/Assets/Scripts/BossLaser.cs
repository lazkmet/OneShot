using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(LineRenderer), typeof(Collider2D))]
public class BossLaser : BossAttack
{
    public float duration = 2.0f;
    public float chargeTime = 1.5f;
    public Vector3 newIrisScale;
    public SpriteRenderer iris;
    private Vector3 defaultIrisScale;
    //public SpriteRenderer eye;
    public Color laserEye;
    private Color normalEye;
    private float laserMaxWidth;
    private LineRenderer laserRenderer;
    private Collider2D laserTrigger;
    private AudioManager aManager;
    private bool firing = false;
    protected override void Awake() {
        laserRenderer = GetComponent<LineRenderer>();
        laserTrigger = GetComponent<Collider2D>();
        aManager = FindObjectOfType<AudioManager>();
        normalEye = iris.color;
        defaultIrisScale = iris.gameObject.transform.localScale;
        laserMaxWidth = laserRenderer.widthMultiplier;
        base.Awake();
    }
    public override IEnumerator Attack()
    {
        EyeFollow e = iris.gameObject.GetComponentInParent<EyeFollow>();
        e.enabled = false;
        e.transform.up = Vector2.up;
        iris.gameObject.transform.localPosition = Vector2.down * 0.15f;
        iris.gameObject.transform.localRotation = Quaternion.identity;
        float currentTime;
        for (currentTime = 0; currentTime < chargeTime; currentTime += Time.deltaTime) {
            iris.color = Color.Lerp(normalEye, laserEye, currentTime / chargeTime);
            yield return null;
        }
        iris.color = laserEye;

        currentTime = 0.05f * duration;
        float width;
        if (!boss.stunned) {
            laserRenderer.enabled = true;
            aManager.Play("Deep Laser Sustain");
        }
        for (float timer = 0; timer < currentTime; timer += Time.deltaTime)
        {
            width = Mathf.Lerp(0, laserMaxWidth, timer / currentTime);
            laserRenderer.widthMultiplier = width;
            iris.gameObject.transform.localScale = Vector3.Lerp(defaultIrisScale, newIrisScale, timer / currentTime);
            yield return null;
        }
        laserRenderer.widthMultiplier = laserMaxWidth;

        currentTime = 0.8f * duration;
        if (!boss.stunned) { 
            laserTrigger.enabled = true; 
        }
        
        firing = true;
        yield return new WaitForSeconds(currentTime);

        currentTime = 0.18f * duration;
        laserTrigger.enabled = false;
        firing = false;
        aManager.Stop("Deep Laser Sustain");
        aManager.Play("Deep Laser Decay");
        for (float timer = 0; timer < currentTime; timer += Time.deltaTime)
        {
            width = Mathf.Lerp(laserMaxWidth, 0, timer / currentTime);
            laserRenderer.widthMultiplier = width;
            iris.gameObject.transform.localScale = Vector3.Lerp(newIrisScale, defaultIrisScale, timer / currentTime);
            iris.color = Color.Lerp(laserEye, normalEye, currentTime / chargeTime);
            yield return null;
        }
        iris.color = normalEye;
        laserRenderer.widthMultiplier = 0;
        iris.gameObject.transform.localScale = defaultIrisScale;
        e.enabled = true;
        laserRenderer.enabled = false;
        boss.IdleForSeconds(idleSeconds);
        this.enabled = false;
    }
    public override void Stun(){
        laserRenderer.enabled = false;
        laserTrigger.enabled = false;
        aManager.Stop("Deep Laser Sustain");
    }
    public override void Unstun() {
        if (firing) {
            laserRenderer.enabled = true;
            laserTrigger.enabled = true;
            aManager.Play("Deep Laser Sustain");
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        PlayerMovement script;
        if (other.gameObject.TryGetComponent(out script))
        {
            script.PlayerDeath();
        }
    }
}
