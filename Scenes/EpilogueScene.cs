using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class EpilogueScene : MonoBehaviour
{
    private int index = 0;
    public TextMeshProUGUI StoryText;
    public Image StoryImage;
    public Sprite[] Images;
    private string[] Stories = {
        "마침내 그 순간이 왔다. \n\n플레이어는 지하 미궁을 끝까지 헤쳐나가 선일 금고를 찾아냈다. \n\n그 순간, 모든 것이 멈추는 듯했다. \n\n금고 앞에 서있는 플레이어의 눈앞에는 창립자의 유산, 그리고 선일 금고의 미래가 펼쳐져 있었다.",
        "플레이어는 금고를 열고 그 안에 담긴 보물과 기술을 세상에 나누기로 결정했다. \n\n그 순간, 선일 금고의 창립자의 정신이 플레이어에게 전해졌다. \n\n선일 금고의 창립자는 보물과 기술을 숨기는 것이 아니라, 그것을 세상에 나누는 것이 진짜 가치라고 생각했던 것이다.",
        "플레이어는 선일 금고의 창립자의 비전을 이해하고, 그의 정신을 이어받아 세상을 더 나은 곳으로 만들기로 결심했다. \n\n이제 선일 금고는 다시금 세상을 이끄는 기업으로 부활하게 되었고, 그 핵심에는 플레이어가 있었다.",
        "그리하여 플레이어의 여정은 끝나고, 새로운 시작이 찾아왔다. \n\n선일 금고의 명예를 되찾은 플레이어는 이제 세상을 더 나은 곳으로 만들기 위해 앞으로 나아갔다. \n\n그리고 그것이 바로 선일 금고의 창립자가 원하던 바로 그 비전이었다.",
    };

    void Start()
    {
        
        StartCoroutine( OnType(0.03f, Stories[0]));
        StoryImage.sprite = Images[0];
    }
    IEnumerator OnType(float interval, string Say)
    {
        foreach (char item in Say)
        {
            StoryText.text += item;
            yield return new WaitForSeconds(interval);
        }
    }
    public void nextStory()
    {
        index++;
        StopAllCoroutines();
        if (index == 4)
        {
            StoryText.gameObject.SetActive(false);
            StoryImage.gameObject.SetActive(false);
            Managers.Scene.LoadScene("StartMenu");
            return;
        }
        StoryText.text = "";
        if(index < 4)
            StartCoroutine(OnType(0.03f, Stories[index]));
        StoryImage.sprite = Images[index];
        
    }
}
