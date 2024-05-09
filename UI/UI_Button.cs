using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Button : MonoBehaviour
{
    public Text text;
    private int _count = 0;

    // 함수 작성
    public void OnButtonClick()
    {
        text.text = $"점수 : {++_count}";
    }
}
