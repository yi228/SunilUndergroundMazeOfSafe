using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameScene : BaseScene
{
    void Start()
    {
        base.Init();
        Managers.Scene.CurrentScene = Define.Scene.InGame;
        Managers.Room.Init();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Managers.Room.Player.PlayerUI.gameObject.SetActive(false);
            Managers.Scene.LoadScene("StartMenu");
        }
    }
}