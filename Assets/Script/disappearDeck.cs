using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class disappearDeck : MonoBehaviour
{
    public Deck deck;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (deck.GetReserved() == 0)
        {
            this.gameObject.SetActive(false);
        }
    }
}
