using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannoneer : EnemyScript
{
    public GameObject cannon;
    public GameObject iris;
    public ParticleSystem emitter;
    public Vector2 eyeSize = Vector2.zero;
    private Vector2 irisZone = Vector2.zero;
 
    private void Start()
    {
        //Will have undefined behavior if iris is larger than eye
        irisZone = 0.4f * (eyeSize - (Vector2)iris.GetComponent<SpriteRenderer>().bounds.extents);
    }
    private void Update()
    {
        if (target != null)
        {
            AimEye();
        }
        else {
            iris.transform.localPosition = Vector2.zero;
        }
    }
    private void AimEye() {
        Vector2 targetAngle = (target.position - cannon.transform.position).normalized;
        //aim cannon
        cannon.transform.up = targetAngle;
        //calculate position within eye (linear interpolate from width to height based on angle, in a similar manner to an ellipse)
        iris.transform.localPosition = Vector2.up * Mathf.Lerp(irisZone.x, irisZone.y, Mathf.Sin(Mathf.Deg2Rad * Vector2.Angle(Vector2.right, targetAngle)));
        iris.transform.up = Vector2.up;
        try
        {
            emitter.gameObject.transform.localPosition = iris.transform.localPosition;
        }
        catch (System.Exception) { }
    }
    public override void Disable()
    { 
        disabled = true;
        if (emitter != null) {
            emitter.Stop();
        }
    }
    public override void Enable()
    {
        disabled = false;
        if (emitter != null)
        {
            emitter.Play();
        }
    }
    public override void Hit()
    {
        emitter.Stop();
        ParticleSystem.EmissionModule em = emitter.emission;
        em.enabled = false;
        emitter.transform.SetParent(gameObject.transform.parent);
        emitter.transform.localScale = Vector3.one;
        base.Hit();
    }
}
