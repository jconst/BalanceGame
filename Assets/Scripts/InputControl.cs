﻿using UnityEngine;
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

    public virtual Vector3 RollVelocity(int playerNum)
    {
        Vector3 velocity = Vector3.zero;

        //allow a player to be controlled by keyboard for testing:
        if (playerNum == keyboardPlayer && velocity.magnitude < 0.2f) {
            velocity = new Vector3(Input.GetAxisRaw("Horizontal"),
                                   0,
                                   Input.GetAxisRaw("Vertical"));
        }
        return velocity * 10f;
    }
}
