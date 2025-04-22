using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyHealthScript : MonoBehaviour
{
    // health value, initialized to 100
    [SerializeField] private float maxHP;
    private float currentHP;
    public Image healthBar;
    private Vector3 originalScale;
    [SerializeField] AudioClip enemyDieSound;

    [SerializeField] AudioClip myFinalGoodbye;

    public IEnumerator PlaySound(AudioClip clip, Transform enemy, bool isAmbient)
    {
        GameObject tempGO = new GameObject("TempAudio");
        tempGO.transform.parent = enemy;
        tempGO.transform.localPosition = Vector3.zero;

        AudioSource aSource = tempGO.AddComponent<AudioSource>();
        aSource.clip = clip;
        aSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
        aSource.pitch = UnityEngine.Random.Range(0.95f, 1.05f);

        aSource.spatialBlend = 1.0f;
        aSource.minDistance = 1f;
        aSource.maxDistance = 20f;
        aSource.rolloffMode = AudioRolloffMode.Linear;

        aSource.Play();
        Destroy(tempGO, clip.length);
        yield return new WaitForSeconds(clip.length);
        yield return null;
    }
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

    IEnumerator MyFinalMessage()
    {
        GameObject.FindGameObjectWithTag("FinalBoss").transform.GetChild(0).GetComponent<FinalBossFiringLocScript>().Pause();
        GameObject.FindGameObjectWithTag("FinalBoss").GetComponent<FinalBossMovementController>().Pause();
        StartCoroutine(PlaySound(myFinalGoodbye, transform, false));
        yield return new WaitForSeconds(myFinalGoodbye.length + 0.5f);
        dying = true;
        StartCoroutine(PlaySound(enemyDieSound));
        StartCoroutine(DyingAnimation());
    }
    private bool running = false;
    // Update is called once per frame
    void Update()
    {
        if (healthBar == null && !dying && currentHP <= 0f)
        {
            dying = true;
            StartCoroutine(PlaySound(enemyDieSound));
            StartCoroutine(DyingAnimation());
        } else if (!running && healthBar != null && !dying && currentHP <= 0f)
        {
            running = true;
            StartCoroutine(MyFinalMessage());
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
        if(healthBar!=null)SceneManager.LoadScene("HH - Credits");
    }
}
