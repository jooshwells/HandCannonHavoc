using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthScript2 : MonoBehaviour
{
    // health value, initialized to 100
    [SerializeField] private float maxHP;
    private float currentHP;
    public Image healthBar;
    private Vector3 originalScale;
    [SerializeField] AudioClip enemyDieSound;
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

    public float GetHealthPerc()
    {
       return Mathf.Clamp(currentHP / maxHP, 0f, 1f);
    }
    public void SetHealth(int newHealth)
    {
        maxHP = newHealth;
    }
    public void UpdateHealth(float damage)
    {
        // just in case someone passes in a negative value for damage
        Debug.Log("Attempting to do " + damage + " damage to " + gameObject.name);
        GetComponent<PlayerHitEffect>().TakeDamage();
        currentHP -= damage > 0 ? damage : 0;

        if(healthBar != null)
        {
            float healthPercent = Mathf.Clamp(currentHP / maxHP, 0f, 1f);
            healthBar.rectTransform.localScale = new Vector3(Mathf.Max(originalScale.x * healthPercent, 0f), originalScale.y, originalScale.z);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        if(healthBar != null) originalScale = healthBar.rectTransform.localScale;
        currentHP = maxHP;
    }
    private bool dying = false;
    // Update is called once per frame
    void Update()
    {
        if (!dying && currentHP <= 0f)
        {
            dying = true;
            StartCoroutine(PlaySound(enemyDieSound));
            StartCoroutine(DyingAnimation());
        }
    }
    public float GetMaxHP() 
    {
        return maxHP;
    }
    public float GetCurrentHP() 
    {
        return currentHP;
    }

    IEnumerator DyingAnimation()
    {
        Animator animator = GetComponent<Animator>();
        if(animator == null) animator = GetComponentInChildren<Animator>();

        if(GetComponent<Rigidbody2D>() != null)
            GetComponent<Rigidbody2D>().simulated = false;
        animator.StopPlayback();
        animator.Play("Die State");
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        Destroy(gameObject);
    }
}
