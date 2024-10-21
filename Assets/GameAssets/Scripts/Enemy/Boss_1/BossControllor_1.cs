using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossControllor_1 : EnemyControllor
{

    Rigidbody2D rig;
    public GameObject spriteObj;
    public List<GameObject> laserObjList;
    public GameObject deadExplod;

    public float rotateSpeed_aim = 20;
    public float aimTimer, aimInterval;
    public float shootTimer, shootInterval;

    public int hitNum = 0, maxHitNum = 10;
    public bool isAim, isShoot, isDead;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        isAim = true;
        aimTimer = aimInterval;
        shootTimer = shootInterval;
    }

    void Update()
    {
        if(hitNum >= maxHitNum && !isDead)
        {
            Dead();
            isDead = true;
        }
    }
    private void FixedUpdate()
    {
        if (!LevelManager.instance.levelPause)
        {
            Move();

            if (isAim)
            {
                Aim();
            }
            if (isShoot)
            {
                Shoot();
            }
        }
    }
    public override void Move()
    {
        float x = Mathf.PerlinNoise(Time.time / 2, 0) * 2 - 1;
        float y = Mathf.PerlinNoise(0, Time.time / 2) * 2 - 1;

        rig.velocity = new Vector2(x, y);
    }
    public void Rotate()
    {
        float angle = (Mathf.PerlinNoise(Time.time / 2, 0) * 2 - 1) * 5;
        transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z + angle);
    }
    public override void Aim()
    {
        //transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z + rotateSpeed_aim);
        transform.Rotate(Vector3.forward, -rotateSpeed_aim * Time.deltaTime);
        rotateSpeed_aim = Mathf.Lerp(rotateSpeed_aim, 0, 0.01f);
        for (int i = 0; i < laserObjList.Count; i++)
        {
            laserObjList[i].GetComponent<LaserControl>().allowHurtPlayer = false;
            laserObjList[i].GetComponent<SpriteRenderer>().material.SetColor("_Color", new Color(155/255f, 155/255f, 155/255f, 1f));
        }
        if (rotateSpeed_aim <= 5f)
        {
            isAim = false;
            isShoot = true;
            shootTimer = shootInterval;
        }
    }
    public override void Shoot()
    {

        transform.Rotate(Vector3.forward, 50 * Time.deltaTime);
        for (int i = 0; i < laserObjList.Count; i++)
        {
            laserObjList[i].GetComponent<LaserControl>().allowHurtPlayer = true;
            laserObjList[i].GetComponent<SpriteRenderer>().material.SetColor("_Color", Color.white);
        }
        shootTimer -= Time.deltaTime;
        if(shootTimer <= 0)
        {
            isShoot = false;
            isAim = true;
            aimTimer = aimInterval;
            rotateSpeed_aim = 100;
        }
    }
    public void Hit()
    {
        StartCoroutine(HitAnim());
    }
    IEnumerator HitAnim()
    {
        Color temp = spriteObj.GetComponent<SpriteRenderer>().color;
        while (true)
        {
            spriteObj.GetComponent<SpriteRenderer>().color = Color.Lerp(spriteObj.GetComponent<SpriteRenderer>().color, Color.red, 0.1f);
            if(Vector4.Distance(spriteObj.GetComponent<SpriteRenderer>().color, Color.red) < 0.05f)
            {
                break;
            }
            else
            {
                yield return null;
            }
        }

        while (true)
        {
            spriteObj.GetComponent<SpriteRenderer>().color = Color.Lerp(spriteObj.GetComponent<SpriteRenderer>().color, temp, 0.1f);
            if (Vector4.Distance(spriteObj.GetComponent<SpriteRenderer>().color, temp) < 0.05f)
            {
                break;
            }
            else
            {
                yield return null;
            }
        }
    }
    public override void Dead()
    {
        isDead = true;
        LevelManager.instance.UpdateExp(LevelManager.instance.requiredExp - LevelManager.instance.startExp);
        spriteObj.SetActive(false);
        for(int i = 0; i < laserObjList.Count; i++)
        {
            laserObjList[i].SetActive(false);
        }
        deadExplod.SetActive(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (PlayerControllor.instance.moveState == MoveState.dash)
            {
                hitNum += 1;
                Hit();
            }
        }
    }
}
