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

    public void Hit(int damage)
    {
        GetComponent<PlayerHitEffect>().TakeDamage();
        hp -= damage;
        float healthPercent = Mathf.Clamp(hp / maxHp, 0f, 1f);
        healthBar.rectTransform.localScale = new Vector3(Mathf.Max(originalScale.x * healthPercent, 0f), originalScale.y, originalScale.z);

    }

    // Start is called before the first frame update
    void Start()
    {
        originalScale = healthBar.rectTransform.localScale;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
