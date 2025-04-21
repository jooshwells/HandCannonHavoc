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
    [SerializeField] private AudioClip hitSound;

    public IEnumerator PlaySound(AudioClip clip)
    {
        GameObject tempGO = new GameObject("TempAudio"); // create new GameObject
        AudioSource aSource = tempGO.AddComponent<AudioSource>(); // add AudioSource
        aSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
        aSource.clip = clip;
        aSource.pitch = UnityEngine.Random.Range(0.95f, 1.05f);
        aSource.Play();
        Destroy(tempGO, clip.length);
        yield return null;
    }
    public void Hit(int damage)
    {
        if (isInvincible)
        {
            return;
        }
        StartCoroutine(PlaySound(hitSound));
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
