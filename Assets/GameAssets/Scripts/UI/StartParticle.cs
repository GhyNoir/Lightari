using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartParticle : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public float triggerRadius = 1.0f;
    private bool isTriggered = false;
    // Start is called before the first frame update
    void Start()
    {
        if(particleSystem == null)
        {
            Debug.LogError("NO PARTICLE");
        }

        particleSystem.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //获取鼠标指针位置
        mouseWorldPosition.z = 0;
        float distance = Vector3.Distance(transform.position, mouseWorldPosition);
        //计算指针和粒子系统位置距离

        if (distance <= triggerRadius && !isTriggered)
        {
            isTriggered = true;
            particleSystem.Play(); // 播放粒子系统  
        }
        else if (distance > triggerRadius && isTriggered)
        {
            isTriggered = false;
            particleSystem.Stop(); // 停止粒子系统  
        }
    }
}
