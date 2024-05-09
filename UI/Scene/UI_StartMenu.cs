using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_StartMenu : UI_Scene
{
    [SerializeField] private Sprite[] buttonImages;
    enum Buttons
    {
        InGameButton,
        ChatGPTButton,
        QuitButton,
    }
    Button inGameButton;
    Button chatGPTButton;
    Button quitButton;
    void Start()
    {
        Bind<Button>(typeof(Buttons));
        inGameButton = GetButton((int)Buttons.InGameButton);
        chatGPTButton = GetButton((int)Buttons.ChatGPTButton);
        quitButton = GetButton((int)Buttons.QuitButton);
        inGameButton.onClick.AddListener(LoadInGame);
        chatGPTButton.onClick.AddListener(() => SceneManager.LoadScene("ChatGPT"));
        quitButton.onClick.AddListener(Application.Quit);
    }
    void LoadInGame()
    {
        //if (PlayerPrefs.GetInt("hasData") == 1)
        //{
        //    Managers.Scene.LoadScene("InGame");
        //}
        //else
        //{
            Managers.Scene.LoadScene("Prologue");
        //}
        gameObject.SetActive(false);
    }
    public void ButtonPressDown(Button _button)
    {
        _button.image.sprite = buttonImages[1];
        Vector3 _textPos = _button.GetComponentInChildren<TextMeshProUGUI>().rectTransform.localPosition;
        _button.GetComponentInChildren<TextMeshProUGUI>().rectTransform.localPosition = new Vector3(_textPos.x, _textPos.y - 20, _textPos.z);
    }
    public void ButtonPressUp(Button _button)
    {
        _button.image.sprite = buttonImages[0];
        Vector3 _textPos = _button.GetComponentInChildren<TextMeshProUGUI>().rectTransform.localPosition;
        _button.GetComponentInChildren<TextMeshProUGUI>().rectTransform.localPosition = new Vector3(_textPos.x, _textPos.y + 20, _textPos.z);
    }
}
