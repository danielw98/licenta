using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Image foregroundImage;
    [SerializeField]
    private float m_updateSpeedSeconds = 0.2f;

    private void Awake()
    {
        GetComponentInParent<Health>().OnHealthPercentageChange += HandleHealthChanged;
    }

    private void HandleHealthChanged(float percentage)
    {
        ChangeToPercentage(percentage);
    }

    private void ChangeToPercentage(float percentage)
    {
        float preChangePercentage = foregroundImage.fillAmount;
        float elapsed = 0f;

        while (elapsed < m_updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            foregroundImage.fillAmount = Mathf.Lerp(preChangePercentage, percentage, elapsed / m_updateSpeedSeconds);
        }
        foregroundImage.fillAmount = percentage;
    }

    private void LateUpdate()
    {
        //transform.LookAt(new Vector3(transform.position.x, Game.Instance.mainCamera.transform.position.y, transform.position.z));
        //transform.Rotate(90, 0, 0);
    }
}
