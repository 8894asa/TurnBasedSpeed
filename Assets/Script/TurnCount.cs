using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnCount : MonoBehaviour
{
    public Field field;
    private Text text = null;
    private int oldcount = 0;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        if (Field.instance != null)
        {
            text.text = "Turn\n" + field.turn;
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
        
        if (oldcount != field.turn)
        {
            text.text = "Turn\n" + field.turn;
            oldcount = field.turn;
        }
    }
}
