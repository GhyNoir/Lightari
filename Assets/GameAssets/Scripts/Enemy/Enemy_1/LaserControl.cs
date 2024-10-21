using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LaserControl : MonoBehaviour
{

    public float SightAngle;
    public float MaxDistance;
    public const int BUFFER_SIZE = 256;

    private float[] m_aDepthBuffer;
    private Material m_ConeOfSightMat;
    private float rotSpeed;

    public bool onHitPlayer, allowHurtPlayer;
    public float damageTimer, damageInterval;
    public float damage;

    void Start()
    {
        m_ConeOfSightMat = GetComponent<SpriteRenderer>().material;
        m_aDepthBuffer = new float[BUFFER_SIZE];
        damageTimer = damageInterval;
        allowHurtPlayer = false;
    }

    void Update()
    {
        if (onHitPlayer && allowHurtPlayer)
        {
            damageTimer -= Time.deltaTime;
            if(damageTimer <= 0)
            {
                damageTimer = damageInterval;
                LevelManager.instance.UpdatePlayerHealth(damage);
            }
        }
    }
    private void FixedUpdate()
    {
        if (!LevelManager.instance.levelPause)
        {
            UpdateViewDepthBuffer();
        }
    }

    private void UpdateViewDepthBuffer()
    {
        float tempAngleStep = SightAngle / BUFFER_SIZE;
        float tempViewAngle = transform.eulerAngles.z;
        int tempBufferIndex = 0;

        for (int i = BUFFER_SIZE; i > 0; i--)
        {
            float tempAngle = tempAngleStep * i + (tempViewAngle - SightAngle / 2.0f);
            Vector3 tempDest = GetVector(tempAngle * Mathf.Deg2Rad, MaxDistance);
            string[] layerMask = { "Obstacle", "Player" };
            RaycastHit2D hit = Physics2D.Raycast(transform.position, tempDest, 1000, LayerMask.GetMask(layerMask));
            if (hit.collider != null)
            {
                m_aDepthBuffer[tempBufferIndex] = (hit.distance / MaxDistance);
                // Debug.DrawRay(transform.position, tempHit.point, Color.red);
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    onHitPlayer = true;
                }
                else
                {
                    onHitPlayer = false;
                }
            }
            else
            {
                m_aDepthBuffer[tempBufferIndex] = -1;
                //Debug.DrawRay(transform.position, tempDest);
                onHitPlayer = false;
            }
            tempBufferIndex++;

        }
        m_ConeOfSightMat.SetFloatArray("_SightDepthBuffer", m_aDepthBuffer);
    }
    private void OnDrawGizmos()
    {
        Handles.color = Color.yellow;
        Handles.DrawWireArc(transform.localPosition, transform.up, Vector3.right, 360, MaxDistance);

        float halfangle = SightAngle * Mathf.Deg2Rad / 2.0f;
        float viewangle = transform.rotation.eulerAngles.y * Mathf.Deg2Rad;

        Vector3 p1 = GetVector(-halfangle - viewangle, MaxDistance);
        Vector3 p2 = GetVector(halfangle - viewangle, MaxDistance);

        // Handles.DrawLine(myObj.transform.position, p1);
        //  Handles.DrawLine(myObj.transform.position, p2);
        Debug.DrawRay(transform.position, p1);
        Debug.DrawRay(transform.position, p2);
    }
    public Vector3 GetVector(float angle, float dist)
    {
        float x = Mathf.Cos(angle) * dist;
        float y = Mathf.Sin(angle) * dist;
        return new Vector3(x, y, 0);

    }
}