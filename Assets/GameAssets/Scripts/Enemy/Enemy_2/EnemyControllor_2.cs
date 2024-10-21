using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllor_2 : EnemyControllor
{
    public Rigidbody2D rig;
    public GameObject dashLineObj, spriteObj;
    private LineRenderer dashLine;
    public GameObject deadExplod;

    public float dashTimer, dashInterval;
    public Vector2 dashDirection;
    public int dashKeyPositionIndex = 0;
    public List<Vector2> dashKeyPositions = new List<Vector2>();

    public float deadTimer = 1f;
    public float dashSpeed = 0.3f;

    public bool allowHurtPlayer;
    public float damage;

    public bool isAim, isShoot, isDead;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        dashLine = dashLineObj.GetComponent<LineRenderer>();
        dashTimer = dashInterval;
        isAim = true;
        isShoot = false;
    }


    void Update()
    {
        if (isDead)
        {
            deadTimer -= Time.deltaTime;
            if (deadTimer <= 0)
            {
                EnemyManager.instance.enemyList.Remove(gameObject);
                Destroy(gameObject);
            }
        }

        //·ÀÖ¹³åµ½±ß½çÍâ
        CheckBonds();
    }
    private void FixedUpdate()
    {
        if (!isDead)
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

    }


    public void LaserWeapon()
    {

    }


    public override void Aim()
    {
        dashDirection = (transform.up).normalized;
        GetDashTrail(dashDirection);

        dashTimer -= Time.deltaTime;
        allowHurtPlayer = false;

        if (dashTimer <= 0)
        {
            isAim = false;
            isShoot = true;
            dashLineObj.SetActive(false);
            dashTimer = dashInterval;
            dashKeyPositionIndex = 0;
        }
    }
    public override void Shoot()
    {
        allowHurtPlayer = true;

        if(dashKeyPositionIndex >= dashKeyPositions.Count)
        {
            dashKeyPositionIndex = dashKeyPositions.Count - 1;
            if(dashKeyPositionIndex < 0)
            {
                dashKeyPositionIndex = 0;
            }
        }

        if(dashKeyPositions.Count > 0)
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
            isShoot = false;
            isAim = true;
            dashLineObj.SetActive(true);
        }
    }
    public void GetDashTrail(Vector2 direction)
    {
        dashKeyPositionIndex = 0;
        dashKeyPositions.Clear();
        dashLine.positionCount = 1;
        dashLine.SetPosition(0, transform.position);

        int i = 0;
        string[] layerMask = { "Obstacle", "Enemy" , "Player"};
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1000, LayerMask.GetMask(layerMask));
        while (hit.collider != null && i < 2)
        {
            dashLine.positionCount++;
            dashLine.SetPosition(i+1, hit.point);
            dashKeyPositions.Add(hit.point);
            direction = Vector2.Reflect(direction, hit.normal);
            hit = Physics2D.Raycast(hit.point + direction * 0.01f, direction, 1000, LayerMask.GetMask(layerMask));
            i++;
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
        float angle = (Mathf.PerlinNoise(Time.time / 2, 0) * 2 - 1) * 0.5f;
        transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z + angle);
    }
    public override void Dead()
    {
        isDead = true;
        PlayerControllor.instance.UpdateEnergy(0.1f);
        LevelManager.instance.UpdateExp(1);
        spriteObj.SetActive(false);
        dashLineObj.SetActive(false);
        deadExplod.SetActive(true);
    }
    public void CheckBonds()
    {
        if(Mathf.Abs(transform.position.x) >= 2.31f || Mathf.Abs(transform.position.y) >= 2.31f)
        {
            EnemyManager.instance.enemyList.Remove(gameObject);
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (PlayerControllor.instance.moveState == MoveState.dash)
            {
                Dead();
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
