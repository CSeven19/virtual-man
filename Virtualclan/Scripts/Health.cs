﻿using UnityEngine;
using UnityEngine.Events;
// 生命值业务类:治疗/受伤/死亡三个功能业务
public class Health : MonoBehaviour
{
    [Tooltip("Maximum amount of health")]
    public float maxHealth = 10f;
    // 危险贫血状态阀值
    [Tooltip("Health ratio at which the critical health vignette starts appearing")]
    public float criticalHealthRatio = 0.3f;

    public UnityAction<float, GameObject> onDamaged;
    public UnityAction<float> onHealed;
    public UnityAction onDie;

    public float currentHealth { get; set; }
    public bool invincible { get; set; }
    public bool canPickup() => currentHealth < maxHealth;

    public float getRatio() => currentHealth / maxHealth;
    public bool isCritical() => getRatio() <= criticalHealthRatio;

    bool m_IsDead;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    // 治疗
    public void Heal(float healAmount)
    {
        float healthBefore = currentHealth;
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        // call OnHeal action
        float trueHealAmount = currentHealth - healthBefore;
        if (trueHealAmount > 0f && onHealed != null)
        {
            onHealed.Invoke(trueHealAmount); // HUD中FeedbackFlashHUD.cs控制血条状态显示
        }
    }
    
    // 受伤
    public void TakeDamage(float damage, GameObject damageSource)
    {
        if (invincible)
            return;

        float healthBefore = currentHealth;
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        // call OnDamage action
        float trueDamageAmount = healthBefore - currentHealth;
        if (trueDamageAmount > 0f && onDamaged != null)
        {
            onDamaged.Invoke(trueDamageAmount, damageSource); // HUD中FeedbackFlashHUD.cs控制血条状态显示
        }

        HandleDeath(); // 死亡处理
    }

    // 杀死角色
    public void Kill()
    {
        currentHealth = 0f;

        // call OnDamage action
        if (onDamaged != null)
        {
            onDamaged.Invoke(maxHealth, null);
        }

        HandleDeath();
    }

    // 处理死亡
    private void HandleDeath()
    {
        if (m_IsDead)
            return;

        // call OnDie action
        if (currentHealth <= 0f)
        {
            if (onDie != null)
            {
                m_IsDead = true;
                onDie.Invoke();
            }
        }
    }
}