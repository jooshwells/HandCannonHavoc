using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PunchyUI : MonoBehaviour
{
    Vector3 targetPos;
    RectTransform rT;
    [SerializeField] private Animator lightningAnim;
    [SerializeField] private Image lightning;
    // Start is called before the first frame update
    void Start()
    {
        rT = gameObject.GetComponent<RectTransform>();
        targetPos = new Vector3(-441, rT.anchoredPosition.y, 0);

        
    }

    // Update is called once per frame
    void Update()
    {
        rT.anchoredPosition = Vector3.MoveTowards(rT.anchoredPosition, targetPos, 1000f * Time.deltaTime);

        if(rT.anchoredPosition.x >= targetPos.x - 0.1f )
        {
            lightningAnim.SetBool("Activate", true);
            lightning.gameObject.SetActive(true);
        }
    }
}
