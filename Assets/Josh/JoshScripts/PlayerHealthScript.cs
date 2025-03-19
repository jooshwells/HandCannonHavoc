using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthScript : MonoBehaviour
{
    private float maxHp = 100;
    private float hp = 100;
    public Image healthBar;
    private Vector3 originalScale;
    private bool isInvincible = false;
    [SerializeField] private float invincibilityTime = 1.5f;

    public void Hit(int damage)
    {
        if (isInvincible)
        {
            return;
        }

        GetComponent<PlayerHitEffect>().TakeDamage();
        hp -= damage;
        float healthPercent = Mathf.Clamp(hp / maxHp, 0f, 1f);
        healthBar.rectTransform.localScale = new Vector3(Mathf.Max(originalScale.x * healthPercent, 0f), originalScale.y, originalScale.z);
    }

    private IEnumerator InvincibilityTimer()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityTime);
        isInvincible = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        originalScale = healthBar.rectTransform.localScale;   
    }

    // Update is called once per frame
    void Update()
    {
        if(hp <= 0f)
        {
            GameObject.Find("Death Manager").GetComponent<DeathScreenManager>().OnDeath();
        }
    }
}
