using System.Collections;
using UnityEngine;

public class LaserTrap : MonoBehaviour
{
    public Sprite[] laserFrames; // [0] = idle, [1], [2], [3] = shoot animation
    public float flashTime = 0.5f;        // how long the flashing phase lasts
    public float flashRate = 0.1f;        // how fast it flickers
    public float frameDuration = 0.1f;    // duration per shoot frame
    public float waitBetweenBursts = 2f;

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        float randomOffset = Random.Range(0f, waitBetweenBursts);
        StartCoroutine(LaserCycle(randomOffset));
    }

    IEnumerator LaserCycle(float initialDelay)
    {
        yield return new WaitForSeconds(initialDelay);

        while (true)
        {
            // 🔁 FLASH PHASE: Flicker by changing alpha
            float flashEndTime = Time.time + flashTime;
            bool visible = true;

            sr.sprite = laserFrames[0]; // use idle/off sprite
            while (Time.time < flashEndTime)
            {
                SetAlpha(visible ? 0.5f : 0f); // alternate visibility
                visible = !visible;
                yield return new WaitForSeconds(flashRate);
            }

            SetAlpha(1f); // full opacity for firing

            //SHOOT PHASE: Play shoot animation
            for (int i = 1; i <= 3; i++)
            {
                sr.sprite = laserFrames[i];
                yield return new WaitForSeconds(frameDuration);
            }

            // ⏸️ IDLE PHASE
            sr.sprite = laserFrames[0];
            SetAlpha(0f); // fully invisible if you want
            yield return new WaitForSeconds(waitBetweenBursts);
        }
    }

    void SetAlpha(float a)
    {
        Color c = sr.color;
        c.a = a;
        sr.color = c;
    }
}
