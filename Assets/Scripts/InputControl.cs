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

    const int keyboardPlayer = 0;
    SerialParser serial;
    Vector3 tilt = Vector3.zero;
    Vector3 tiltOffset = Vector3.zero;

    public InputControl() {
        serial = GameObject.FindObjectOfType(typeof(SerialParser)) as SerialParser;
        tiltOffset = serial.groundRotation;
        Debug.Log("tiltOffset");
        Debug.Log(tiltOffset);        
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
        return velocity * 5f;
    }

    public Quaternion Tilt()
    {
        if (serial.groundRotation != Vector3.zero) {
            Vector3 euler = serial.groundRotation;
            for (int i=0; i<3; ++i) {
                euler[i] = Mathf.Clamp(euler[i], -15, 15);
            }
            return Quaternion.Euler(euler);
        }
        Vector3 newTilt = new Vector3(Input.GetAxisRaw("TiltForward"),
                                      0,
                                      -Input.GetAxisRaw("TiltRight"));
        newTilt -= tiltOffset;
        tilt = Vector3.Lerp(tilt, newTilt, 0.2f);
        return Quaternion.Euler(tilt * 15f);;
    }

    public bool PendingBomb() {
        return serial.touchpadPosition.magnitude > 10f ||
               Input.GetKeyDown(KeyCode.B);
    }

    public Vector3 BombDropPosition() {
        if (Input.GetKeyDown(KeyCode.B)) {
            return new Vector2(0.5f, 0.5f);
        }
        return serial.touchpadPosition;
    }
}
