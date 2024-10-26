using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossControllor_final : MonoBehaviour
{
    public Rigidbody2D rig;
    public GameObject lightMask;
    public GameObject spriteObj;

    //move参数
    public float moveSpeed = 3;
    //dash参数
    public float oriDashSpeed, dashSpeed, dashDamping;
    public int maxReflectTime;
    public Vector2 dashDirection;
    public int dashKeyPositionIndex = 0;
    public List<Vector2> dashKeyPositions = new List<Vector2>();

    //小光粒
    public bool hasSpark;
    public GameObject sparkPrefeb;
    public float sparkTimer, sparkInterval;
    public int sparkNum;

    public bool allowDash;

    //攻击方式：0.冲刺 1.发光
    public int attackType;

    //激光参数
    public List<GameObject> laserObjList;
    public GameObject deadExplod;

    public float rotateSpeed_aim = 20;
    public float aimTimer, aimInterval;
    public float shootTimer, shootInterval;

    public int hitNum = 0, maxHitNum = 15;

    //冲刺参数
    public GameObject dashLineObj, soundObj;
    private LineRenderer dashLine;

    public float dashTimer, dashInterval;
    public float deadTimer = 1f;

    public bool allowHurtPlayer;
    public float damage;

    public enum BossState
    {
        chooseWeapon,
        aiming,
        dashing,
        lasering
    }
    public BossState bossState;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        dashLine = dashLineObj.GetComponent<LineRenderer>();
        dashTimer = dashInterval;
        CopyParameters();
    }

    // Update is called once per frame
    void Update()
    {
        if (!LevelManager.instance.levelPause)
        {
            BossHandler();
        }

        if (hitNum >= maxHitNum)
        {
            Dead();
            hitNum = -1;
        }
    }
    private void FixedUpdate()
    {
        
    }

    //复制玩家数值
    public void CopyParameters()
    {
        moveSpeed = PlayerControllor.instance.moveSpeed;
        dashSpeed = PlayerControllor.instance.oriDashSpeed;
        dashDamping = PlayerControllor.instance.dashDamping;
        maxReflectTime = PlayerControllor.instance.maxReflectTime + 1;

    }
    //操控器
    public void BossHandler()
    {
        if(bossState == BossState.chooseWeapon)
        {
            attackType = Random.Range(0,2);
            if(attackType == 0)
            {
                for (int i = 0; i < laserObjList.Count; i++)
                {
                    laserObjList[i].SetActive(false);
                }
                dashLineObj.SetActive(true);
            }
            else if(attackType == 1)
            {
                for(int i = 0; i < laserObjList.Count; i++)
                {
                    laserObjList[i].SetActive(true);
                }
                dashLineObj.SetActive(false);
            }
            bossState = BossState.aiming;
        }
        else if (bossState == BossState.aiming)
        {
            if(attackType == 0)
            {
                Aim_dash();
            }
            else if(attackType == 1)
            {
                Aim_laser();
            }
        }
        else if(bossState == BossState.dashing)
        {
            Shoot_dash();
        }
        else if(bossState == BossState.lasering)
        {
            Shoot_laser();
        }

        if(bossState != BossState.dashing)
        {
            Move();
            Rotate();
        }

    }

    public void Aim_laser()
    {
        //transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z + rotateSpeed_aim);
        transform.Rotate(Vector3.forward, -rotateSpeed_aim * Time.deltaTime);
        rotateSpeed_aim = Mathf.Lerp(rotateSpeed_aim, 0, 0.01f);
        for (int i = 0; i < laserObjList.Count; i++)
        {
            laserObjList[i].GetComponent<LaserControl>().allowHurtPlayer = false;
            laserObjList[i].GetComponent<SpriteRenderer>().material.SetColor("_Color", new Color(155 / 255f, 155 / 255f, 155 / 255f, 1f));
        }
        if (rotateSpeed_aim <= 5f)
        {
            bossState = BossState.lasering;
            shootTimer = shootInterval;
        }
    }
    public void Shoot_laser()
    {
        transform.Rotate(Vector3.forward, 50 * Time.deltaTime);
        for (int i = 0; i < laserObjList.Count; i++)
        {
            laserObjList[i].GetComponent<LaserControl>().allowHurtPlayer = true;
            laserObjList[i].GetComponent<SpriteRenderer>().material.SetColor("_Color", Color.white);
        }
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0)
        {
            bossState = BossState.chooseWeapon;
            //aimTimer = aimInterval;
            rotateSpeed_aim = 100;
        }
    }

    public void Aim_dash()
    {
        dashDirection = (transform.up).normalized;
        GetDashTrail(dashDirection);

        dashTimer -= Time.deltaTime;
        allowHurtPlayer = false;

        if (dashTimer <= 0)
        {
            bossState = BossState.dashing;
            dashLineObj.SetActive(false);
            dashTimer = dashInterval;
            dashKeyPositionIndex = 0;
        }
    }
    public  void Shoot_dash()
    {
        allowHurtPlayer = true;

        if (dashKeyPositionIndex >= dashKeyPositions.Count)
        {
            dashKeyPositionIndex = dashKeyPositions.Count - 1;
            if (dashKeyPositionIndex < 0)
            {
                dashKeyPositionIndex = 0;
            }
        }

        if (dashKeyPositions.Count > 0)
        {
            transform.position = Vector2.Lerp(transform.position, dashKeyPositions[dashKeyPositionIndex], dashSpeed);
            if (Vector2.Distance(transform.position, dashKeyPositions[dashKeyPositionIndex]) <= 0.15f)
            {
                dashKeyPositionIndex++;

                if (dashKeyPositionIndex < dashKeyPositions.Count)
                {
                    transform.up = (dashKeyPositions[dashKeyPositionIndex] - (Vector2)transform.position).normalized;
                }
            }
        }

        if (dashKeyPositionIndex >= dashKeyPositions.Count)
        {
            bossState = BossState.chooseWeapon;
            //dashLineObj.SetActive(true);
        }
        
    }
    public void GetDashTrail(Vector2 direction)
    {
        dashKeyPositionIndex = 0;
        dashKeyPositions.Clear();
        dashLine.positionCount = 1;
        dashLine.SetPosition(0, transform.position);

        int i = 0;
        string[] layerMask = { "Obstacle", "Player" };
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1000, LayerMask.GetMask(layerMask));
        while (hit.collider != null && i < 2)
        {
            dashLine.positionCount++;
            dashLine.SetPosition(i + 1, hit.point);
            dashKeyPositions.Add(hit.point);
            direction = Vector2.Reflect(direction, hit.normal);
            hit = Physics2D.Raycast(hit.point + direction * 0.01f, direction, 1000, LayerMask.GetMask(layerMask));
            i++;
        }
    }
    public void Move()
    {
        float x = Mathf.PerlinNoise(Time.time / 2, 0) * 2 - 1;
        float y = Mathf.PerlinNoise(0, Time.time / 2) * 2 - 1;

        rig.velocity = new Vector2(x, y) * moveSpeed;
    }
    public void Rotate()
    {
        float angle = (Mathf.PerlinNoise(Time.time / 2, 0) * 2 - 1) * 0.5f;
        transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z + angle);
    }


    public void SeprateSpark()
    {

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
            if (Vector4.Distance(spriteObj.GetComponent<SpriteRenderer>().color, Color.red) < 0.05f)
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
    public void Dead()
    {
        LevelManager.instance.levelEnd = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (PlayerControllor.instance.moveState == MoveState.dash)
            {
                hitNum += 1;
                Hit();
                CameraControllor.instance.CameraShake();
            }
            else
            {
                if (allowHurtPlayer)
                {
                    LevelManager.instance.UpdatePlayerHealth(damage);
                }
            }
        }
    }
}
