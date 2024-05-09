using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class UI_Register : UI_Scene
{
    public class GoogleData
    {
        public string order, result, msg, value;
    }
    enum InputFields
    {
        IDInputField,
        PWInputField,
        ValueInputField
    }
    enum Buttons
    {
        LoginButton,
        LogoutButton,
        RegisterButton,
        SaveValueButton,
        LoadValueButton,
    }
    const string URL = "https://script.google.com/macros/s/AKfycby-OMCLHlfoSmclpNiBGoAMxsHkdJEwxEQFO6xfXtVHfdTwYewfFnuXTVnqTvg54nwe/exec";
    public GoogleData GD;
    private TMP_InputField IDInputField, PWInputField, ValueInputField;
    private Button LoginButton, LogoutButton, RegisterButton, SaveValueButton, LoadValueButton;
    private string id, pw;
    public void SetValue()
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "setValue");
        form.AddField("value", ValueInputField.text);

        StartCoroutine(Post(form));
    }
    public void GetValue()
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "getValue");

        StartCoroutine(Post(form));
    }
    bool IsSetInfo()
    {
        id = IDInputField.text.Trim();
        pw = PWInputField.text.Trim();

        if (id == "" || pw == "")
            return false;
        return true;
    }
    void OnApplicationQuit()
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "logout");

        StartCoroutine(Post(form));
    }
    void Logout()
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "logout");

        StartCoroutine(Post(form));
    }
    void Login()
    {
        if (IsSetInfo() == false)
        {
            Debug.Log("아이디 또는 비밀번호가 비어있습니다");
            return;
        }

        WWWForm form = new WWWForm();
        form.AddField("order", "login");
        form.AddField("id", id);
        form.AddField("pw", pw);

        StartCoroutine(Post(form));
    }
    void Register()
    {
        if (IsSetInfo() == false)
        {
            Debug.Log("아이디 또는 비밀번호가 비어있습니다");
            return;
        }
        WWWForm form = new WWWForm();
        form.AddField("order", "register");
        form.AddField("id", id);
        form.AddField("pw", pw);

        StartCoroutine(Post(form));
    }
    IEnumerator Post(WWWForm form)
    {
        // 안쓰면 통신 안할 때 있드라
        using (UnityWebRequest www = UnityWebRequest.Post(URL, form))
        {
            yield return www.SendWebRequest();

            if (www.isDone)
                Response(www.downloadHandler.text);
            else
                Debug.Log("서버와의 응답이 없습니다");
        }
    }
    void Response(string json)
    {
        if (string.IsNullOrEmpty(json))
            return;
        GD = JsonUtility.FromJson<GoogleData>(json);
        if (GD.result == "ERROR")
        {
            Debug.Log($"{GD.order}을 실행할 수 없습니다. 에러 메시지: {GD.msg}");
            return;
        }

        Debug.Log($"{GD.order}을 실행했습니다. 메시지: {GD.msg}");

        if (GD.order == "getValue")
        {
            ValueInputField.text = GD.value;
        }
    }
    void InitUI()
    {
        // InputFields
        IDInputField = Get<TMP_InputField>((int)InputFields.IDInputField);
        PWInputField = Get<TMP_InputField>((int)InputFields.PWInputField);
        ValueInputField = Get<TMP_InputField>((int)InputFields.ValueInputField);

        // Buttons
        LoginButton = GetButton((int)Buttons.LoginButton);
        LogoutButton = GetButton((int)Buttons.LogoutButton);
        RegisterButton = GetButton((int)Buttons.RegisterButton);
        SaveValueButton = GetButton((int)Buttons.SaveValueButton);
        LoadValueButton = GetButton((int)Buttons.LoadValueButton);

        LoginButton.onClick.AddListener(Login);
        LogoutButton.onClick.AddListener(Logout);
        RegisterButton.onClick.AddListener(Register);
        SaveValueButton.onClick.AddListener(SetValue);
        LoadValueButton.onClick.AddListener(GetValue);
    }
    void Start()
    {
        Bind<TMP_InputField>(typeof(InputFields));
        Bind<Button>(typeof(Buttons));
        InitUI();
    }
}