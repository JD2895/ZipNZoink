using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuHelper : MonoBehaviour
{
    public Button startButton;
    public Button quitButton;
    public string startinglevel;

    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(delegate { GameManager.instance.LoadLevel(startinglevel); });
        quitButton.onClick.AddListener(delegate { GameManager.instance.Quit(); });
    }
}
