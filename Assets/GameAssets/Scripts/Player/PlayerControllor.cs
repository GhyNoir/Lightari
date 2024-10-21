using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllor : MonoBehaviour
{
    public static PlayerControllor instance;
    public Rigidbody2D rig;
    public GameObject lightMask;

    public MoveState moveState;
    //move参数
    public float moveSpeed = 3;
    //dash参数
    public float oriDashSpeed, dashSpeed, dashDamping;
    public int maxReflectTime;
    public Vector2 dashDirection;
    public int dashKeyPositionIndex = 0;
    public List<Vector2> dashKeyPositions = new List<Vector2>();
    //能量
    [HideInInspector]
    public float minEnergy = 0, maxEnergy = 1;
    public float currentEnergy, targetEnergy, energyDelta;
    //光照范围
    public float minLightMaskScale = 0.1f, maxLightMaskScale = 0.45f;
    public float lightMaskAdd = 0.2f, lightMaskSubtract = 0.3f;
    public float  targetLightMaskScale;
    //小光粒
    public bool hasSpark;
    public GameObject sparkPrefeb;
    public float sparkTimer, sparkInterval;
    public int sparkNum;
    //全局速度权重
    public bool isGlobalSpeedOverride;
    public float globalSpeed;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        targetLightMaskScale = minLightMaskScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (!LevelManager.instance.levelPause)
        {
            if (Input.GetMouseButtonUp(0))
            {
                dashSpeed = oriDashSpeed;
                dashDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
                GetDashTrail(dashDirection);
                UpdateEnergy(-0.1f);
                moveState = MoveState.dash;
            }

            if (Mathf.Abs(lightMask.transform.localScale.x - targetLightMaskScale) > 0.01f)
            {
                lightMask.transform.localScale = Vector2.Lerp(lightMask.transform.localScale, Vector2.one * targetLightMaskScale, 0.1f);
            }

            if(sparkNum > 0)
            {
                sparkTimer -= Time.deltaTime;
                
                if(sparkTimer <= 0)
                {
                    for(int i = 0; i < sparkNum; i++)
                    {

                    }
                    sparkTimer = sparkInterval;
                }
            }
        }

        if (!isGlobalSpeedOverride)
        {
            globalSpeed = rig.velocity.magnitude;
        }
    }
    private void FixedUpdate()
    {
        if (!LevelManager.instance.levelPause)
        {
            if (moveState == MoveState.dash)
            {
                Dash();
            }
            else
            {
                Move();
            }
        }
    }
    public void Dash()
    {
        transform.position = Vector2.Lerp(transform.position, dashKeyPositions[dashKeyPositionIndex], dashSpeed);
        dashSpeed = Mathf.Lerp(dashSpeed, 0 ,dashDamping);

        if(Vector2.Distance(transform.position, dashKeyPositions[dashKeyPositionIndex]) <= 0.15f)
        {
            dashKeyPositionIndex++;
        }

        if (dashKeyPositionIndex >= dashKeyPositions.Count || dashSpeed <= 0.01f)
        {
            moveState = MoveState.move;
        }
    }
    public void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        rig.velocity = new Vector2(x, y) * moveSpeed;
    }
    public void GetDashTrail(Vector2 direction)
    {
        dashKeyPositionIndex = 0;
        dashKeyPositions.Clear();

        int i = 0;
        string[] layerMask = { "Obstacle", "Enemy" };
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1000, LayerMask.GetMask(layerMask));
        while (hit.collider != null && i < maxReflectTime) 
        {
            dashKeyPositions.Add(hit.point);
            direction = Vector2.Reflect(direction, hit.normal);
            hit = Physics2D.Raycast(hit.point + direction * 0.01f, direction, 1000, LayerMask.GetMask(layerMask));
            i++;
        }
    }

    public void UpdateEnergy(float value)
    {
        targetEnergy += value;
        if (targetEnergy > maxEnergy)
            targetEnergy = maxEnergy;
        if (targetEnergy < minEnergy)
            targetEnergy = minEnergy;

        //targetLightMaskScale = (targetEnergy - minEnergy) / (maxEnergy - minEnergy) * (maxLightMaskScale - minLightMaskScale);
        if(value > 0)
            targetLightMaskScale += lightMaskAdd;
        else
            targetLightMaskScale -= lightMaskSubtract;
        if (targetLightMaskScale < minLightMaskScale)
            targetLightMaskScale = minLightMaskScale;
        if (targetLightMaskScale > maxLightMaskScale)
            targetLightMaskScale = maxLightMaskScale;
    }

    public void SeprateSpark()
    {

    }
}

public enum MoveState
{
    move,
    dash
}
