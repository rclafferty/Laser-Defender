using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerBehavior : MonoBehaviour
{
    //GameObject startButton;
    Button startButton;
    //GameObject quitButton;
    Button quitButton;

    Button selectedButton;

    // Start is called before the first frame update
    void Start()
    {
        startButton = GameObject.Find("Start Button").GetComponent<Button>();
        quitButton = GameObject.Find("Quit Button").GetComponent<Button>();

        selectedButton = startButton;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Horizontal") < 0)
        {
            selectedButton = startButton;
            startButton.Select();
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            selectedButton = quitButton;
            quitButton.Select();
        }
        else if (Input.GetButtonDown("Fire"))
        {
            selectedButton.Select();
        }
    }
}
