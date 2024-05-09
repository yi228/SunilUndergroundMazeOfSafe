using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class Tile : MonoBehaviour, IPointerClickHandler
{
	private	TextMeshProUGUI	numText;
	private	Board board;
	private	Vector3 correctPos;

	public bool IsCorrected { get; private set; } = false;

	private int number;
	public int Number
	{
		get { return number; }
        set
        {
            number = value;
            numText.text = number.ToString();
        }
    }
	public void Setup(Board _board, int _hideNum, int _num)
	{
		board = _board;
        numText = GetComponentInChildren<TextMeshProUGUI>();

        Number = _num;
		if (Number == _hideNum )
		{
			GetComponent<Image>().enabled = false;
            numText.enabled = false;
		}
	}
	public void SetCorrectPosition()
	{
		correctPos = GetComponent<RectTransform>().localPosition;
	}
	public void OnPointerClick(PointerEventData eventData)
	{
		board.MoveTile(this);
	}
	public void OnMoveTo(Vector3 end)
	{
		StartCoroutine("MoveTo", end);
	}
	private IEnumerator MoveTo(Vector3 end)
	{
		float current = 0f;
		float percent = 0f;
		float moveTime = 0.1f;
		Vector3	start = GetComponent<RectTransform>().localPosition;

		while ( percent < 1 )
		{
			current += Time.deltaTime;
			percent = current / moveTime;

			GetComponent<RectTransform>().localPosition = Vector3.Lerp(start, end, percent);

			yield return null;
		}
		IsCorrected = correctPos == GetComponent<RectTransform>().localPosition ? true : false;
		board.GameOver();
	}
}

