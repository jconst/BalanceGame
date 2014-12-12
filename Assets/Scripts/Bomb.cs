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
    }
    
    void Update() {
        float progress = 1f-(transform.position.y/initialAltitude);
        float newSize = initialSize * progress;
        transform.localScale = Vector3.one * newSize;
    }
}
