using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
    public Scanner scanner;
    public Hand[] hands;
    public RuntimeAnimatorController[] animCon;

    Animator anim;
    Rigidbody2D rigid;
    SpriteRenderer spriter;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true);
    }

    private void OnEnable()
    {
        speed *= Charactor.Speed;
        anim.runtimeAnimatorController = animCon[GameManager.instance.playerId];
    }
    private void Update()
    {
        if (!GameManager.instance.isLive)
            return;
    }

    private void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }

    private void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        if (inputVec.x != 0) {
            anim.SetBool("isRun", true);
            spriter.flipX = inputVec.x < 0;
        }
        if(inputVec.x == 0)
            anim.SetBool("isRun", false);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.instance.isLive) return;

        GameManager.instance.hp -= Time.deltaTime* 1;
        if (GameManager.instance.hp < 0)
        {
            for (int index = 2; index < transform.childCount; index++) {
                transform.GetChild(index).gameObject.SetActive(false);
            }
            anim.SetTrigger("Dead");
            GameManager.instance.GameOver();
        }   
    }

    private void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }
}
