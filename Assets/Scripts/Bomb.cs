using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Bomb : MonoBehaviour
{
    float initialSize;
    float initialAltitude;

    void Start() {
        initialSize = transform.localScale.x;
        initialAltitude = transform.position.y;
        transform.localScale = Vector3.zero;
        Destroy(gameObject, 10f);
    }
    
    void Update() {
        float progress = 1f-(transform.position.y/initialAltitude);
        float newSize = Mathf.Min(initialSize * progress, initialSize);
        transform.localScale = Vector3.one * newSize;
    }

    void OnCollisionEnter(Collision coll) {
        if (coll.gameObject.tag != "Bomb") {
            Explode();
        }
    }

    void Explode() {
        GetComponentsInChildren<ParticleSystem>()
        .ToList()
        .ForEach(p => p.Play());
        renderer.enabled = false;
        collider.enabled = false;
        Destroy(gameObject, 2f);

        Manager.S.livingBalls
        .Where(b => !b.airborne)
        .ToList()
        .ForEach(b => {
            // Debug.Log("here");
            // Vector3 dist = b.transform.position - transform.position;
            // Vector3 force = dist.normalized * (20f/dist.magnitude);
            // b.rigidbody.AddForce(force, ForceMode.Impulse);

            b.rigidbody.AddExplosionForce(1300f, transform.position, 50f);
        });
    }
}
