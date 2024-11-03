using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Retry : MonoBehaviour
{
    public Field field;

    Text text = null;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        text.text = field.winnername + "Win.";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Retrymethod()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
