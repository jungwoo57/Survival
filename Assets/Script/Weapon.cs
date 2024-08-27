using System.Collections;
using System.Collections.Generic;
//using UnityEditor.U2D;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float dmg;
    public int count;
    public float speed;

    float timer;
    Player player;

    private void Awake()
    {
        player = GameManager.instance.player;
    }

    private void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        switch (id) {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                    break;
            default:
                timer += Time.deltaTime;

                if(timer > speed){
                    timer = 0.0f;
                    Fire();

                }
                break;
        }
    }

    public void Init(ItemData data)
    {
        name = "Weapon" + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        id = data.itemId;
        dmg = data.baseDamage;
        count = data.baseCount; 
        
        for(int index = 0; index < GameManager.instance.pool.Prefabs.Length; index++) {     
            if(data.projectile == GameManager.instance.pool.Prefabs[index]){
                prefabId = index;
                break;
            }
        }
        switch (id) {
            case 0:
                speed = 150 * Charactor.WeaponSpeed;
                Batch();
                break;
            case 1:
                speed = 0.5f* Charactor.WeaponRate ;
                break;
            default:
                break;
                
        }
        Hand hand = player.hands[(int)data.itemType];
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    void Batch()
    {
        for(int index = 0; index < count; index++){
            Transform bullet;

            if (index < transform.childCount)
            {
                bullet = transform.GetChild(index);

            }
            else
            {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform;
            }
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.2f, Space.World);
            bullet.GetComponent<Bullet>().Init(dmg, -100 ,Vector3.zero); // -100 is Infinity Per;
        }
    }

    public void LevelUp(float dmg, int count) {
        this.dmg = dmg;
        this.count += count;

        if(id == 0) {
            Batch();
        }
        player.BroadcastMessage("ApplyGear",SendMessageOptions.DontRequireReceiver);
    }

    void Fire() 
    {
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(dmg, count, dir);

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
    }
}
