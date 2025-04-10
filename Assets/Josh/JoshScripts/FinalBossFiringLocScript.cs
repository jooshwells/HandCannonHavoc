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

    void Start()
    {
        startPosition = transform.localPosition;
    }

    void Update()
    {
        if(!isRunning)
        {
            StartCoroutine(Attack());
        }

        timer += Time.deltaTime * bobSpeed;
        float offset = Mathf.Lerp(-bobDistance, bobDistance, (Mathf.Sin(timer) + 1f) / 2f);
        transform.localPosition = startPosition + new Vector3(0f, offset, 0f);
    }

    IEnumerator Attack()
    {
        isRunning = true;
        // do attacking stuff
        Instantiate(projectile, transform.position, transform.rotation);
        yield return new WaitForSeconds(Random.Range(2f, 5f));
        isRunning = false;
    }
}
