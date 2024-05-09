using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuScene : MonoBehaviour
{
    int quitCount = 0;
    void Start()
    {
        Managers.Scene.CurrentScene = Define.Scene.StartMenu;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            quitCount++;
            if (quitCount == 2)
                Application.Quit();
        }
    }
}
