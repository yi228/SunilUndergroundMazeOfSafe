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
        "��ħ�� �� ������ �Դ�. \n\n�÷��̾�� ���� �̱��� ������ ���ĳ��� ���� �ݰ� ã�Ƴ´�. \n\n�� ����, ��� ���� ���ߴ� ���ߴ�. \n\n�ݰ� �տ� ���ִ� �÷��̾��� ���տ��� â������ ����, �׸��� ���� �ݰ��� �̷��� ������ �־���.",
        "�÷��̾�� �ݰ� ���� �� �ȿ� ��� ������ ����� ���� ������� �����ߴ�. \n\n�� ����, ���� �ݰ��� â������ ������ �÷��̾�� ��������. \n\n���� �ݰ��� â���ڴ� ������ ����� ����� ���� �ƴ϶�, �װ��� ���� ������ ���� ��¥ ��ġ��� �����ߴ� ���̴�.",
        "�÷��̾�� ���� �ݰ��� â������ ������ �����ϰ�, ���� ������ �̾�޾� ������ �� ���� ������ ������ ����ߴ�. \n\n���� ���� �ݰ�� �ٽñ� ������ �̲��� ������� ��Ȱ�ϰ� �Ǿ���, �� �ٽɿ��� �÷��̾ �־���.",
        "�׸��Ͽ� �÷��̾��� ������ ������, ���ο� ������ ã�ƿԴ�. \n\n���� �ݰ��� ���� ��ã�� �÷��̾�� ���� ������ �� ���� ������ ����� ���� ������ ���ư���. \n\n�׸��� �װ��� �ٷ� ���� �ݰ��� â���ڰ� ���ϴ� �ٷ� �� �����̾���.",
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
