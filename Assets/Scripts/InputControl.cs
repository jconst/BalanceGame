using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class InputControl
{
    static public InputControl _s;
    static public InputControl S {
        get {
            if (_s == null) {
                _s = new InputControl();
            }
            return _s;
        }
    }

    public bool pendingBomb = false;
    public Vector3 bombDropPosition = Vector3.zero;

    const int keyboardPlayer = 0;
    SerialParser serial;
    Vector3 tilt = Vector3.zero;

    public InputControl() {
        serial = GameObject.FindObjectOfType(typeof(SerialParser)) as SerialParser;
    }

    public Vector3 RollVelocity(int playerNum)
    {
        Vector3 velocity = serial.ballVelocity[playerNum];

        //allow a player to be controlled by keyboard for testing:
        if (playerNum == keyboardPlayer && velocity.magnitude < 0.2f) {
            velocity = new Vector3(Input.GetAxisRaw("Horizontal"),
                                   0,
                                   Input.GetAxisRaw("Vertical"));
        }
        return velocity * 12f;
    }

    public Quaternion Tilt()
    {
        if (serial.groundRotation != Quaternion.identity) {
            return serial.groundRotation;// * Quaternion.Euler(0,0,0);
        }
        Vector3 newTilt = new Vector3(Input.GetAxisRaw("TiltForward"),
                                      0,
                                      -Input.GetAxisRaw("TiltRight"));
        tilt = Vector3.Lerp(tilt, newTilt, 0.2f);
        return Quaternion.Euler(tilt * 20f);;
    } 
}
