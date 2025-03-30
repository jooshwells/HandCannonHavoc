using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesKilledUISlide : MonoBehaviour
{
    Vector3 targetPos;
    RectTransform rT;
    // Start is called before the first frame update
    void Start()
    {
        rT = gameObject.GetComponent<RectTransform>();
        targetPos = new Vector3(-543.9f, rT.anchoredPosition.y, 0);
    }

    // Update is called once per frame
    void Update()
    {
        rT.anchoredPosition = Vector3.MoveTowards(rT.anchoredPosition, targetPos, 700f * Time.deltaTime);
    }
}
