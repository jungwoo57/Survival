using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public int level;

    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;

    bool isLive;

    Rigidbody2D rigid;
    Collider2D coll;
    Animator anim;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait;

    private void Start()
    {
       
    }
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        wait = new WaitForFixedUpdate();
        coll = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit")) 
            return;
        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }

    private void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;
        if (!isLive) 
            return;
        spriter.flipX = target.position.x < rigid.position.x;
    }
    private void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        anim.SetBool("Dead", false);
        spriter.sortingOrder = 2;
        coll.enabled = true;
        rigid.simulated = true;
        gameObject.SetActive(true);
        health = maxHealth;

    }

    public void Init(SpawnData data)
    {
        level = (int)GameManager.instance.gameTime / 15;
        if (level == 0) level = 1;
        if (level > 15) level = 15;
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health + (level * 2);
        health = data.health + (level * 2);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive)
            return;
        health -= collision.GetComponent<Bullet>().dmg;
        StartCoroutine("KnockBack");
        if (health > 0){
            anim.SetTrigger("Hit");
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
        }
        else{
            anim.SetBool("Dead", true);
            spriter.sortingOrder = 1;
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            GameManager.instance.kill++;
            GameManager.instance.GetExp();
            if(GameManager.instance.isLive)
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);
        }
    }
    IEnumerator KnockBack() {
        yield return wait; // 1프레임 쉬기
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }

    void Dead() {
        gameObject.SetActive(false);
    }
}
