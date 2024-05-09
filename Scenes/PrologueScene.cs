using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PrologueScene : MonoBehaviour
{
    private int index = 0;
    public TextMeshProUGUI StoryText;
    public Image StoryImage;
    public Sprite[] Images;
    private string[] Stories = {
        "우리의 이야기는 선일 금고, 그리고 그것의 창립자에게서 시작된다. \n\n 선일 금고는 한때 세상을 이끄는 기업이었다. \n\n혁신적인 기술과 탁월한 경영 능력으로 사람들의 삶을 바꾸는 것이 그들의 목표였다. \n\n 그러나 그 모든 것은 코로나 19가 창궐하고 변했다. 회사는 경쟁사들에게 밀려 서서히 경쟁력을 잃어갔다.",
        "그러던 중 회사 내부에서 창립자의 편지가 발견된다.\n\n 그는 마지막 숨을 내쉬기 전, 모든 기술과 보물을 담은 궁극의 금고를 회사 지하에 숨겼다는 사실을 밝혔다.\n\n 그 금고를 찾아내는 자가 회사의 후계자가 될 것이라는 그의 편지는 회사를 새롭게 일으킬 수 있는 유일한 희망이었다.",
        "이제 플레이어는 선일 금고의 후계자로서,\n\n 금고를 찾아내기 위한 여정에 뛰어들어야 한다.\n\n 미궁 속에서 플레이어를 기다리는 것은 단순히 보물과 기술이 아니다.\n\n 거기에는 선일 금고의 창립자의 역사와 그의 비전, \n\n그리고 그가 왜 이 모든 것을 숨겼는지에 대한 이야기가 담겨있다.",
        "플레이어는 금고를 찾아내며 창립자의 이야기를 풀어나가게 된다.\n\n 그리고 그 이야기를 통해 선일 금고가 가진 진짜 가치를 깨닫게 된다. \n\n금고의 비밀을 알게 된 플레이어는 세상에 그 보물과 기술을 나누기로 결정하고, \n\n이로써 선일 금고의 창립자의 정신을 이어받게 된다.",
        "이제 플레이어의 여정이 시작된다. \n\n 선일 금고의 명예를 되찾기 위한, 그리고 세상을 더 나은 곳으로 만들기 위한 모험이 시작되는 순간이다.",
 };

    void Start()
    {
        PlayerPrefs.SetInt("hasData", 1);
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
        if (index == 5)
        {
            StoryImage.gameObject.SetActive(false);
            StoryText.gameObject.SetActive(false);
            Managers.Scene.LoadScene("InGame");
            return;
        }
        StoryText.text = "";
        StartCoroutine(OnType(0.03f, Stories[index]));
        StoryImage.sprite = Images[index];
        
    }
}
