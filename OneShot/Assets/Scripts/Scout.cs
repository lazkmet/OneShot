using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scout : EnemyScript
{
    public ParticleSystem emitter;
    public override void Disable()
    {
        disabled = true;
        if (emitter != null)
        {
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
        try {
            emitter.Stop();
            ParticleSystem.EmissionModule em = emitter.emission;
            em.enabled = false;
            emitter.transform.SetParent(gameObject.transform.parent);
        }
        catch (System.Exception) {}
        base.Hit();
    }
}
