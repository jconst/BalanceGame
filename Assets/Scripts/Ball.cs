using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Ball : MonoBehaviour
{
    public int number;

    void Start() {
    
    }
    
    void FixedUpdate() {
        rigidbody.AddForce(InputControl.S.RollVelocity(number));
    }
}
