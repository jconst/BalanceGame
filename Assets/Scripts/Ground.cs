using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Ground : MonoBehaviour
{
    float initialSize;

    void Start() {
        initialSize = transform.localScale.x;
    }

    void Update() {
        transform.rotation = InputControl.S.Tilt();

        float newSize = initialSize * Mathf.Min((1-Manager.S.roundProgress)+0.1f, 1f);
        transform.localScale = new Vector3(newSize, transform.localScale.y, newSize);
    }
}
