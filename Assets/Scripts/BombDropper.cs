using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BombDropper : MonoBehaviour
{
    const float dropHeight = 10f;

    void Start() {
    
    }
    
    void Update() {
        if (InputControl.S.PendingBomb()) {
            Vector3 bombPos = InputControl.S.BombDropPosition();
            bombPos -= (Vector3.one * 0.5f);                    //center
            bombPos *= Manager.S.ground.transform.localScale.x; //scale
            bombPos.y = dropHeight;
            GameObject go = Instantiate(Resources.Load("Bomb")) as GameObject;
            go.transform.position = bombPos;
        }
    }
}
