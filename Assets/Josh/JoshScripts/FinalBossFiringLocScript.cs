using System.Collections;
using UnityEngine;

public class FinalBossFiringLocScript : MonoBehaviour
{
    public float bobDistance = 0.5f;    // How far it moves up and down
    public float bobSpeed = 2f;         // Speed of the bobbing motion
    [SerializeField] private GameObject projectile;

    private Vector3 startPosition;
    private float timer;

    private bool isRunning = false;
    public Transform enemyGFX;
    private Rigidbody2D rb;

    private bool movingRight = false;
    private bool movingLeft = false;

    private bool paused = false;

    [SerializeField] private AudioClip shootSound;
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

    void Start()
    {
        rb = transform.GetComponentInParent<Rigidbody2D>();
        startPosition = transform.localPosition;
    }
    private int projCount = 0;
    public void DecrementProjectileCount() { projCount--; }

    void Update()
    {
        if (paused)
        {
            return;
        }
        if (!isRunning && projCount < 5)
        {
            StartCoroutine(Attack());
        }

        // Bobbing effect
        timer += Time.deltaTime * bobSpeed;
        float offset = Mathf.Lerp(-bobDistance, bobDistance, (Mathf.Sin(timer) + 1f) / 2f);
        transform.localPosition = startPosition + new Vector3(0f, offset, 0f);

        if(transform.position.y >= 17.85f && transform.position.x <= 25.01f)
        {
            transform.localPosition = new Vector3(Mathf.Abs(transform.localPosition.x), transform.localPosition.y, transform.localPosition.z);
        } else
        {
            // Move enemyGFX to the opposite side based on direction
            if (rb.velocity.x > 0.01f)
            {
                transform.localPosition = new Vector3(-Mathf.Abs(transform.localPosition.x), transform.localPosition.y, transform.localPosition.z);
            }
            else if (rb.velocity.x < -0.01f)
            {
                transform.localPosition = new Vector3(Mathf.Abs(transform.localPosition.x), transform.localPosition.y, transform.localPosition.z);
            }
        }

        
    }

    public void Pause()
    {
        paused = true;
    }

    public void UnPause() { paused = false; }

    IEnumerator Attack()
    {
        StartCoroutine(PlaySound(shootSound, transform, false));
        isRunning = true;
        // do attacking stuff
        Instantiate(projectile, transform.position, transform.rotation);
        projCount++;
        yield return new WaitForSeconds(Random.Range(2f, 5f));
        isRunning = false;
    }
}
