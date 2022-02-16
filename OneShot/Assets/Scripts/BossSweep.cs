using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSweep : BossAttack
{
    public Transform[] extensionPoints = { };
    public Transform[] cannons = { };
    public ParticleSystem[] emitters = { };
    private Vector3[] defaultAngles;
    private Vector3[] defaultPositions;
    private float[] sweepDirections;
    public float sweepAngle = 0f;
    public float rotationSpeed = 0f; //degrees per second
    public float cannonExtendAmount = 0f;
    protected override void Awake()
    {
        defaultAngles = new Vector3[cannons.Length];
        defaultPositions = new Vector3[cannons.Length];
        sweepDirections = new float[cannons.Length];
        for (int i = 0; i < cannons.Length; i++) {
            defaultPositions[i] = cannons[i].localPosition;
            defaultAngles[i] = cannons[i].rotation.eulerAngles;
            sweepDirections[i] = Mathf.Sign(Mathf.Sin(Mathf.Deg2Rad * (cannons[i].rotation.eulerAngles.z)));
        }

        base.Awake();
    }
    public override void Stun() {
        foreach (ParticleSystem p in emitters) {
            ParticleSystem.EmissionModule em = p.emission;
            em.enabled = false;
        }
    }
    public override void Unstun()
    {
        foreach (ParticleSystem p in emitters)
        {
            ParticleSystem.EmissionModule em = p.emission;
            em.enabled = true;
        }
    }
    public override IEnumerator Attack() {
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < extensionPoints.Length; i++) //break from association
        {
            extensionPoints[i].parent = cannons[i].transform.parent;
        }

        for (float currentTime = 0; currentTime < 0.8f; currentTime += Time.deltaTime) { //extend cannon
            for (int i = 0; i < cannons.Length; i++) {
                cannons[i].position = extensionPoints[i].position + cannons[i].up * Mathf.Lerp(0, cannonExtendAmount, currentTime / 0.8f);
            }
            yield return null;
        }
        for (int i = 0; i < cannons.Length; i++)
        {
            cannons[i].position = extensionPoints[i].position + cannons[i].up * cannonExtendAmount;
        }

        foreach (ParticleSystem p in emitters)
        {
            p.Play();
        }
        for (int i = 0; i < extensionPoints.Length; i++) //associate with cannon pivot
        {
            extensionPoints[i].parent = cannons[i].transform;
        }

        //sweep cannon (direction times angle) while total turn < amount
        float totalAngle = 0;
        float angle;
        do {
            angle = rotationSpeed * Time.deltaTime;
            for (int i = 0; i < cannons.Length; i++) {  
                cannons[i].Rotate(sweepDirections[i] * angle * new Vector3(0,0,1));
                ParticleSystem.MainModule m = emitters[i].main;
                m.startRotation = -(cannons[i].rotation.eulerAngles.z) * Mathf.Deg2Rad;
            }
            totalAngle += angle;
            yield return null;
        } while (totalAngle < sweepAngle);

        for (int i = 0; i < extensionPoints.Length; i++) //break from association
        {
            extensionPoints[i].parent = cannons[i].transform.parent;
        }
        foreach (ParticleSystem p in emitters)
        {
            p.Stop();
        }
        for (float currentTime = 0; currentTime < 0.8f; currentTime += Time.deltaTime) {  //retract cannon
            for (int i = 0; i < cannons.Length; i++) {
                cannons[i].position = extensionPoints[i].position + cannons[i].up * Mathf.Lerp(cannonExtendAmount, 0, currentTime / 0.8f);
            }
            yield return null;
        }

        ResetPositions();
        yield return new WaitForSeconds(0.5f);
        boss.IdleForSeconds(idleSeconds);
        this.enabled = false;
    }
    private void ResetPositions() {
        for (int i = 0; i < cannons.Length; i++)
        {
            cannons[i].position = extensionPoints[i].position;
        }
        for (int i = 0; i < extensionPoints.Length; i++) //associate with cannon pivot
        {
            extensionPoints[i].parent = cannons[i].transform;
            cannons[i].transform.rotation = Quaternion.Euler(defaultAngles[i]);
            cannons[i].transform.localPosition = defaultPositions[i];
        }
    }
}
