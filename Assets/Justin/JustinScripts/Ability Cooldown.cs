using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCooldown : MonoBehaviour
{
    private copyController player;
    private HighJumpParachute hjp;
    RectTransform resize;

    // Start is called before the first frame update

    public Image myImage; // Assign your image in the Inspector

    void Start()
    {
        RectTransform transofrm = myImage.GetComponent<RectTransform>();
    }
    // Update is called once per frame
    void Update()
    {
        //if(hjp.getCooldown() > 0)
        //transform.sizeDelta = new Vector2(300f, 150f);

    }
}
