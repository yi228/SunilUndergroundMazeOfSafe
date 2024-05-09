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
        "�츮�� �̾߱�� ���� �ݰ�, �׸��� �װ��� â���ڿ��Լ� ���۵ȴ�. \n\n ���� �ݰ�� �Ѷ� ������ �̲��� ����̾���. \n\n�������� ����� Ź���� �濵 �ɷ����� ������� ���� �ٲٴ� ���� �׵��� ��ǥ����. \n\n �׷��� �� ��� ���� �ڷγ� 19�� â���ϰ� ���ߴ�. ȸ��� �����鿡�� �з� ������ ������� �Ҿ��.",
        "�׷��� �� ȸ�� ���ο��� â������ ������ �߰ߵȴ�.\n\n �״� ������ ���� ������ ��, ��� ����� ������ ���� �ñ��� �ݰ� ȸ�� ���Ͽ� ����ٴ� ����� ������.\n\n �� �ݰ� ã�Ƴ��� �ڰ� ȸ���� �İ��ڰ� �� ���̶�� ���� ������ ȸ�縦 ���Ӱ� ����ų �� �ִ� ������ ����̾���.",
        "���� �÷��̾�� ���� �ݰ��� �İ��ڷμ�,\n\n �ݰ� ã�Ƴ��� ���� ������ �پ���� �Ѵ�.\n\n �̱� �ӿ��� �÷��̾ ��ٸ��� ���� �ܼ��� ������ ����� �ƴϴ�.\n\n �ű⿡�� ���� �ݰ��� â������ ����� ���� ����, \n\n�׸��� �װ� �� �� ��� ���� ��������� ���� �̾߱Ⱑ ����ִ�.",
        "�÷��̾�� �ݰ� ã�Ƴ��� â������ �̾߱⸦ Ǯ����� �ȴ�.\n\n �׸��� �� �̾߱⸦ ���� ���� �ݰ� ���� ��¥ ��ġ�� ���ݰ� �ȴ�. \n\n�ݰ��� ����� �˰� �� �÷��̾�� ���� �� ������ ����� ������� �����ϰ�, \n\n�̷ν� ���� �ݰ��� â������ ������ �̾�ް� �ȴ�.",
        "���� �÷��̾��� ������ ���۵ȴ�. \n\n ���� �ݰ��� ���� ��ã�� ����, �׸��� ������ �� ���� ������ ����� ���� ������ ���۵Ǵ� �����̴�.",
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
