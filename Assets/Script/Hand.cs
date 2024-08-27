using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public bool isLeft;
    public SpriteRenderer spriter;

    SpriteRenderer player;

    Vector3 rightPos = new Vector3(0.35f, -0.3f, 0);
    Vector3 rightPosRev = new Vector3(-0.15f, -0.3f, 0);
    
    Quaternion leftRot = Quaternion.Euler(0, 0, -30.0f);
    Quaternion leftRotRev = Quaternion.Euler(0, 0, -130.0f);
    private void Awake()
    {
        player = GetComponentsInParent<SpriteRenderer>()[1];
    }

    private void LateUpdate()
    {
        bool isReverse = player.flipX;
        if (isLeft) {  //±Ÿ¡¢
            transform.localRotation = isReverse ? leftRotRev : leftRot;
            spriter.flipY = isReverse;
            spriter.sortingOrder = isReverse ? 4 : 6;
        }
        else {
            transform.localPosition = isReverse ? rightPosRev : rightPos;
            spriter.flipX = isReverse;
            spriter.sortingOrder = isReverse ? 6 : 4;
        }
    }
}
