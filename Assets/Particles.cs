using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class Particles : MonoBehaviour
{
    private void Start()
    {
        Fork.ForkCollision += Fire;
    }

    public void Fire(Fork fork)
    {
        GetComponent<ParticleSystem>().Play();
    }

    private void OnDestroy()
    {
        Fork.ForkCollision -= Fire;
    }
}
