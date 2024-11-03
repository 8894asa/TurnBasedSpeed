using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Recombining_Draw : MonoBehaviour
{
    public Image img = null;

    // Start is called before the first frame update
    void Start()
    {
        img = GetComponentInChildren<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Field.instance.isRDraw && ((Field.instance.turn % 2 == 1 && transform.name == "p1") || (Field.instance.turn % 2 == 0 && transform.name == "p2")))
        {
            img.enabled = true;
        }
        else
        {
            img.enabled = false;
        }
    }
}
