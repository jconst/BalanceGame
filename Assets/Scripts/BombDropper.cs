using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BombDropper : MonoBehaviour
{
    const float dropHeight = 10f;
    float bombCooldown = 5f;
    
    void Update() {
        bombCooldown -= Time.deltaTime;
        if (InputControl.S.PendingBomb() &&
            bombCooldown < 0) {
            Vector3 bombPos = InputControl.S.BombDropPosition();
            Debug.Log("dropping bomb");
            Debug.Log(bombPos);
            bombPos *= 1/800f;                                  //normalize
            bombPos -= (Vector3.one * 0.5f);                    //center
            bombPos.Scale(new Vector3(-1, 1, 1));               //invert x
            bombPos *= Manager.S.ground.transform.localScale.x; //scale
            bombPos.y = dropHeight;
            GameObject go = Instantiate(Resources.Load("Bomb")) as GameObject;
            go.transform.position = bombPos;
            bombCooldown = 2f;
        }
    }
}
