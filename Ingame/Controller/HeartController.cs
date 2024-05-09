using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartController : MonoBehaviour
{
    public Sprite EmptyHeart;
    public Sprite FullHeart;
    private Define.HeartType _type = Define.HeartType.Full;
    public Define.HeartType Type { 
        get { return _type; }
        set
        {
            _type = value;
            switch (_type)
            {
                case Define.HeartType.Full:
                    GetComponent<Image>().sprite = FullHeart;
                    break;
                case Define.HeartType.Empty:
                    GetComponent<Image>().sprite = EmptyHeart;
                    break;
            }
        }
    }

}
