using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Ground : MonoBehaviour
{
    void Update() {
        rigidbody.rotation = Quaternion.Euler(InputControl.S.Tilt() * 20f);
    }
}
