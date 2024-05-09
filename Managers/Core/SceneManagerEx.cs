using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    public Define.Scene _currentScene;
    public Define.Scene CurrentScene
    {
        get { return _currentScene; }
        set
        {
            if (_currentScene == value)
                return;
            _currentScene = value;
            Managers.Sound.ChangeBgm();
        }
    }
    public void LoadScene(Define.Scene type)
    {
        Managers.Clear();

        SceneManager.LoadScene(GetSceneName(type));
    }
    public void LoadScene(string sceneName)
    {
        UI_LoadingScene.Instance.LoadScene(sceneName);
    }

    string GetSceneName(Define.Scene type)
    {
        string name = System.Enum.GetName(typeof(Define.Scene), type);
        return name;
    }

    public void ReloadCurrentScene()
    {
        //int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        //SceneManager.LoadScene(currentSceneIndex);
        Managers.Room.Player.PlayerUI.gameObject.SetActive(false);
        string sceneName = GetSceneName(Managers.Scene.CurrentScene);
        LoadScene(sceneName);
    }
}
