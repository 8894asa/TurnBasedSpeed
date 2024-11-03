using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reserved : MonoBehaviour
{
    public Deck deck;
    private Text text = null;
    private int oldcount = 0;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        if (Field.instance != null)
        {
            text.text = "" + deck.GetReserved();
        }
        else
        {
            Debug.Log("ゲームマネージャー置き忘れてるよ！");
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (oldcount != deck.GetReserved())
        {
            text.text = "" + deck.GetReserved();
            oldcount = deck.GetReserved();
        }
    }
}
