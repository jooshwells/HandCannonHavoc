using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private GameObject instantiator;
    private float attackDamage;

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

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlaySound(GetComponent<AudioSource>().clip));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetInstantiator(GameObject inst)
    {
        instantiator = inst;
    }
    public void SetAttackDamage(float damage)
    {
        attackDamage = damage;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Health Update Event for Enemies
        if (collision.CompareTag("Enemy"))
        {
            GameObject par = null;

            // Basically this is checking whether the object we are colliding with has a parent
                // I (Josh) have some enemies with child object colliders, so essentially just make sure
                // your HealthSystem is only one object down in the hierarchy and this should work fiiine.
                    // (Check my Crocogator prefab for how I did it)
            if (collision.gameObject.transform.parent != null)
            {
                par = ((collision.gameObject).transform.parent).gameObject; // this gets the parent using transform
            }

            if(par != null)
            {
                par.GetComponentInChildren<EnemyHealthScript>().UpdateHealth(attackDamage); // if we have a parent, check its children for a script to update the health
            } 
            else
            {
                collision.gameObject.GetComponentInChildren<EnemyHealthScript>().UpdateHealth(attackDamage); // else just check the collision objects children for the script
            }

            Destroy(gameObject); // destroy the bullet
        }

        else if (!(collision.CompareTag(instantiator.tag) && !collision.CompareTag("Bullet")) ) { 

            {
                Destroy(gameObject);
            }
        }
    }
}
