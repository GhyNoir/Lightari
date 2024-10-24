using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllor_1 : EnemyControllor
{
    public Rigidbody2D rig;
    public GameObject line1Obj, line2Obj, laserObj, spriteObj;
    private LineRenderer line1, line2;
    public GameObject deadExplod;

    public float aimAngle = 30;

    public float deadTimer = 1f;
    public bool isAim, isShoot, isDead;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        line1 = line1Obj.GetComponent<LineRenderer>();
        line2 = line2Obj.GetComponent<LineRenderer>();
        isAim = true;
        isShoot = false;
    }


    void Update()
    {
        if (isDead)
        {
            deadTimer -= Time.deltaTime;
            if(deadTimer <= 0)
            {
                EnemyManager.instance.enemyList.Remove(gameObject);
                Destroy(gameObject);
            }
        }
    }
    private void FixedUpdate()
    {
        if (!LevelManager.instance.levelPause)
        {
            Move();
            Rotate();

            if (isAim)
            {
                Aim();
            }
            if (isShoot)
            {
                Shoot();
            }
        }
        else
        {
            rig.velocity = Vector2.zero;
        }
    }


    public void LaserWeapon()
    {
        
    }


    public override void Aim()
    {
        line1.SetPosition(0, transform.position);
        line2.SetPosition(0, transform.position);

        Vector3 direction1 = Quaternion.Euler(0, 0, transform.eulerAngles.z + aimAngle) * Vector3.right * 5;
        Vector3 direction2 = Quaternion.Euler(0, 0, transform.eulerAngles.z - aimAngle) * Vector3.right * 5;
        aimAngle = Mathf.Lerp(aimAngle, 0, 0.02f);

        line1.SetPosition(1,transform.position + direction1);
        line2.SetPosition(1, transform.position + direction2);
        //line1.SetPosition(1, (Quaternion.Euler(transform.right).z + aimAngle))
        laserObj.GetComponent<LaserControl>().allowHurtPlayer = false;
        if(Mathf.Abs(aimAngle - 5) <= 0.1f)
        {
            isAim = false;
            isShoot = true;
            line1Obj.SetActive(false); line2Obj.SetActive(false);
        }
    }
    public override void Shoot()
    {
        laserObj.GetComponent<SpriteRenderer>().material.SetColor("_Color",Color.white);
        laserObj.GetComponent<LaserControl>().allowHurtPlayer = true;
    }
    public override void Move()
    {
        float x = Mathf.PerlinNoise(Time.time/2,0) * 2 - 1;
        float y = Mathf.PerlinNoise(0,Time.time/2) * 2 - 1;

        rig.velocity = new Vector2(x,y);
    }

    public void Rotate()
    {
        float angle = (Mathf.PerlinNoise(Time.time/2, 0) * 2 - 1) * 5;
        transform.rotation = Quaternion.Euler(0,0,transform.eulerAngles.z + angle);
    }
    public override void Dead()
    {
        isDead = true;
        PlayerControllor.instance.UpdateEnergy(0.1f);
        LevelManager.instance.UpdateExp(1);
        spriteObj.SetActive(false);
        line1Obj.SetActive(false); 
        line2Obj.SetActive(false); 
        laserObj.SetActive(false);
        deadExplod.SetActive(true);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(PlayerControllor.instance.moveState == MoveState.dash)
            {
                Dead();
                CameraControllor.instance.CameraShake();
            }
        }
    }
}
