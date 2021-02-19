using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreFork : Fork
{
    private new void OnTriggerEnter2D(Collider2D collision)
    {
        if (State == ForkState.Idle)
        {
            var hitTarget = collision.gameObject.GetComponent<Target>();
            if (hitTarget != null)
            {
                _body.bodyType = RigidbodyType2D.Static;
                transform.parent = hitTarget.transform;
                State = ForkState.HitTarget;
            }

            Debug.Log($"State: {State}");
        }
    }
}
