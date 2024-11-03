using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    string tagname;
    bool oldstate;
    Card card;
    // Start is called before the first frame update
    void Start()
    {
        tagname = transform.tag;
        card = GetComponent<Card>();
    }

    // Update is called once per frame
    void Update()
    {
        if (tagname == "Synthesize")
        {
            Synthesize();
        }

        if (tagname == "Compress")
        {
            Compress();
        }

        if (tagname == "Recombine")
        {
            Recombine();
        }
    }

    public void Synthesize()
    {
        if (Field.instance.isSynthesizing)
        {
            card.LightOn();
        }
        else
        {
            card.LightOff();
        }
    }

    public void Compress()
    {
        if (Field.instance.isCompressing)
        {
            card.LightOn();
        }
        else
        {
            card.LightOff();
        }
    }

    public void Recombine()
    {
        if (Field.instance.isRecombining)
        {
            card.LightOn();
        }
        else
        {
            card.LightOff();
        }
    }
}
