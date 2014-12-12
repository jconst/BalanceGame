using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Ball : MonoBehaviour
{
    const float jumpSpeed = 8;

    public int number;
    public bool frozen;
    public bool airborne = true;
    public bool dead {
        get {
            return transform.position.y < -0.5;
        }
    }

    void Start() {
    
    }
    
    void FixedUpdate() {
        if (!frozen) {
            Vector3 force = InputControl.S.RollVelocity(number);
            if (airborne) {
                force.y = 0;
            } else if (force.y > 0) {
                Jump();
            }
            rigidbody.AddForce(force);
        }
    }

    void Jump() {
        if (airborne)
            return;
        Vector3 newVelocity = rigidbody.velocity;
        newVelocity = Vector3.up * jumpSpeed;
        rigidbody.velocity = newVelocity;

        airborne = true;
    }

    void OnCollisionStay(Collision coll) {
        if (coll.gameObject.tag == "Ground")
            airborne = false;
    }

    void OnCollisionExit(Collision coll) {
        if (coll.gameObject.tag == "Ground")
            airborne = true;
    }
}
