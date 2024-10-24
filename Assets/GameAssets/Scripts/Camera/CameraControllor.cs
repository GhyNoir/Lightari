using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControllor : MonoBehaviour
{
    public static CameraControllor instance;
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin noiseProfile;

    public float battleCameraSize = 2.3f, chaseCameraSize = 20f;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        noiseProfile = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        if (ChaseCellManager.instance.levelChase)
        {
            ToChase();
        }
        else
        {
            ToBattle();
        }
    }

    /// <summary>
    /// ÕðÆÁ
    /// </summary>
    /// <param name="duration">Ê±³¤</param>
    /// <param name="amplitude">·ù¶È</param>
    /// <param name="frequency">ÆµÂÊ</param>
    public void CameraShake(float duration = 0.4f, float amplitude = 0.1f, float frequency = 5f)
    {
        if (noiseProfile != null)
        {
            noiseProfile.m_AmplitudeGain = amplitude;
            noiseProfile.m_FrequencyGain = frequency;
            Invoke(nameof(StopShaking), duration);
        }
    }

    /// <summary>
    /// Í£Ö¹ÕðÆÁ
    /// </summary>
    private void StopShaking()
    {
        if (noiseProfile != null)
        {
            noiseProfile.m_AmplitudeGain = 0f;
            noiseProfile.m_FrequencyGain = 0f;
        }
    }

    public void ToBattle()
    {
        virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(virtualCamera.m_Lens.OrthographicSize, battleCameraSize, 0.1f);
    }
    public void ToChase()
    {
        virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(virtualCamera.m_Lens.OrthographicSize, chaseCameraSize, 0.1f);
    }
}
