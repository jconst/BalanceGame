using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Ball : MonoBehaviour
{
    public int number;
    public bool frozen;

    void Start() {
    
    }
    
    void FixedUpdate() {
        if (!frozen)
            rigidbody.AddForce(InputControl.S.RollVelocity(number));
    }

    void OnBecameInvisible() {
        Destroy(gameObject);
    }
}
