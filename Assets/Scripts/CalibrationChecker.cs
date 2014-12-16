using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CalibrationChecker : MonoBehaviour
{
    SerialParser serial;
    Vector3 groundRotation;
    const float calibrationTolerance = 0.3f;

    void Start () {
        serial = GameObject.FindObjectOfType(typeof(SerialParser)) as SerialParser;
        groundRotation = serial.groundRotation;
        StartCoroutine(Calibration());
    }
    
    IEnumerator Calibration() {
        bool calibrated = false;
        while(!calibrated) {
            Vector3 newRotation = serial.groundRotation;
            Debug.Log("calibration read");
            Debug.Log(newRotation);
            calibrated = 
            newRotation != Vector3.zero &&
            Enumerable.Range(0,3)
            .Select(i => newRotation[i] - groundRotation[i])
            .Aggregate(true, (acc, cur) => acc && cur < calibrationTolerance);

            groundRotation = newRotation;
            yield return new WaitForSeconds(1f);
        }
        Application.LoadLevel(1);
    }
}
