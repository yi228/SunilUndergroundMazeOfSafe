using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class UI_ChatGPTPopup : UI_Popup
{
    enum Texts
    {
        InputText,
        AnswerText
    }
    enum Buttons
    {
        SendButton
    }
    enum State
    {
        Start,
        End
    }
    private Button sendButton;
    private TextMeshProUGUI inputText;
    private TextMeshProUGUI answerText;
    private OpenAIApi openAI;
    private List<ChatMessage> messages = new List<ChatMessage>();
    private string keyUrl = "https://evenidemonickitchen.s3.ap-northeast-2.amazonaws.com/key.json";
    private string keyValue;
    [SerializeField] private Sprite[] buttonImages;
    State state = State.Start;
    public async void AskChatGPT(string newText)
    {
        state = State.Start;
        sendButton.interactable = false;
        // 프롬프트 추가 코드
        newText = $"선일 관련하여 {newText}";

        ChatMessage newMessage = new ChatMessage();
        newMessage.Content = newText;
        newMessage.Role = "user";

        messages.Add(newMessage);

        CreateChatCompletionRequest request = new CreateChatCompletionRequest();
        request.Messages = messages;
        request.Model = "gpt-3.5-turbo";

        answerText.text = "답변 대기중";
        StartCoroutine(CoStartWaiting());
        var response = await openAI.CreateChatCompletion(request);

        if (response.Choices != null && response.Choices.Count > 0)
        {
            state = State.End;
            var chatResponse = response.Choices[0].Message;
            messages.Add(chatResponse);
            Debug.Log(chatResponse.Content);
            answerText.text = chatResponse.Content;
            sendButton.interactable = true;
        }
    }
    IEnumerator CoStartWaiting()
    {
        answerText.text += "·";
        yield return new WaitForSeconds(1f);
        if (state == State.Start)
            StartCoroutine(CoStartWaiting());
    }

    IEnumerator CoDownloadAPIKey()
    {
        answerText.text = "서버와 연결중입니다";
        state = State.Start;
        StartCoroutine(CoStartWaiting());
        UnityWebRequest www = UnityWebRequest.Get(keyUrl);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error: " + www.error);
            answerText.text = "서버와 연결에 실패했습니다";
        }
        else
        {
            KeyData keyData = JsonConvert.DeserializeObject<KeyData>(www.downloadHandler.text);
            keyValue = keyData.api_key;
            openAI = new OpenAIApi(keyValue);
            state = State.End;
            sendButton.interactable = true;
            answerText.text = "서버와 연결에 성공했습니다";
        }
    }
    void Start()
    {
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        sendButton = GetButton((int)Buttons.SendButton);
        inputText = GetTextMeshProUGUI((int)Texts.InputText);
        answerText = GetTextMeshProUGUI((int)Texts.AnswerText);
        sendButton.interactable = false;
        sendButton.onClick.AddListener(()=> AskChatGPT(inputText.text));
        StartCoroutine(CoDownloadAPIKey());
    }
    public class KeyData
    {
        public string api_key;
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
    public void CloseButtonClicked()
    {
        Managers.Scene.LoadScene("StartMenu");
    }
}
