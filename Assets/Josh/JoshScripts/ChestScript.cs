using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    [SerializeField] private GameObject contents;

    private GameObject player;
    private bool opened = false;
    private AudioSource asrc;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        asrc = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Transform p = player.transform;
        if(!opened && Vector2.Distance(transform.position, p.position) < 2f)
        {
            opened = true;
            //asrc.Play();
            StartCoroutine(OpenChest());
        }
    }

    IEnumerator OpenChest()
    {
        Animator anim = gameObject.GetComponent<Animator>();
        anim.Play("Open");
        yield return new WaitForEndOfFrame();
        //Instantiate(contents, transform);
        //yield return new WaitForSeconds(0.2f);
        asrc.Play();

        // Spawn that grappling hook

    }
}
