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
    }
    
    void Update() {
        float progress = 1f-(transform.position.y/initialAltitude);
        float newSize = initialSize * progress;
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
            Debug.Log("here");
            b.rigidbody.AddExplosionForce(500f, transform.position, 50f);
        });
    }
}
