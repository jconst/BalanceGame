using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CalibrationChecker : MonoBehaviour
{
    SerialParser serial;
    Vector3 groundRotation;

    void Start () {
        serial = GameObject.FindObjectOfType(typeof(SerialParser)) as SerialParser;
        groundRotation = serial.groundRotation;
    }
    
    void Update () {
        Vector3 newRotation = serial.groundRotation;
        bool calibrated = 
        newRotation != Vector3.zero &&
        Enumerable.Range(0,3)
        .Select(i => newRotation[i] - groundRotation[i])
        .Aggregate(true, (acc, cur) => acc && cur < 1f);
        if (calibrated) {
            Application.LoadLevel(1);
        }
    }
}
