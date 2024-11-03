using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public enum Mark
    {
        SPADE, HEART, DIAMOND, CLUB, JOKER
    }
    // Start is called before the first frame update
    void Start()
    {

        Debug.Log("aaa");
        Debug.Log((int)Random.Range(1.0f, 4095.0f));
        int i = (int)Random.Range(1.0f, 4095.0f);
        Debug.Log(i);
        Debug.Log(Mark.SPADE);
        Debug.Log((int)Mark.SPADE);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
