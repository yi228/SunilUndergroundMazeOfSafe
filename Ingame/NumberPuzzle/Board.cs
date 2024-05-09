using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
	[SerializeField] private GameObject tilePrefab; // ���� Ÿ�� ������
	[SerializeField] private Transform tileParent; // Ÿ���� ��ġ�Ǵ� "Board" ������Ʈ�� Transform

	private	List<Tile> tileList; // ������ Ÿ�� ���� ����

	private	Vector2Int puzzleSize = new Vector2Int(3, 3); // 4x4 ����
	private float neighborTileDistance; // ������ Ÿ�� ������ �Ÿ�. ������ ����� ���� �ִ�.

	public Vector3 emptyTilePosition { get; set; } // �� Ÿ���� ��ġ
	public int playTime { get; private set; } = 0;
	public int moveCount { get; private set; } = 0; // �̵� Ƚ��

	private GridLayoutGroup gridLayout;

	void Start()
	{
        gridLayout = GetComponent<GridLayoutGroup>();
        neighborTileDistance = gridLayout.cellSize.x + gridLayout.spacing.x;
        tileList = new List<Tile>();

        SpawnTiles();

        LayoutRebuilder.ForceRebuildLayoutImmediate(tileParent.GetComponent<RectTransform>());

        foreach (Tile tile in tileList)
            tile.SetCorrectPosition();

        StartCoroutine(CoOnShuffle());
        StartCoroutine(CoCountTime());
    }
	private void SpawnTiles()
	{
		for ( int y = 0; y < puzzleSize.y; ++y )
		{
			for ( int x = 0; x < puzzleSize.x; ++x )
			{
				GameObject clone = Instantiate(tilePrefab, tileParent);
				Tile _tile = clone.GetComponent<Tile>();

				_tile.Setup(this, puzzleSize.x * puzzleSize.y, y * puzzleSize.x + x + 1);

				tileList.Add(_tile);
			}
		}
	}
	private IEnumerator CoOnShuffle()
	{
		float current = 0;
		float percent = 0;
		float time = 1.5f;

		while ( percent < 1 )
		{
			current += Time.deltaTime;
			percent = current / time;

			int _ind = Random.Range(0, puzzleSize.x * puzzleSize.y);
			tileList[_ind].transform.SetAsLastSibling();

			yield return null;
		}

		emptyTilePosition = tileList[tileList.Count - 1].GetComponent<RectTransform>().localPosition;
	}
	public void MoveTile(Tile tile)
	{
		if ( Vector3.Distance(emptyTilePosition, tile.GetComponent<RectTransform>().localPosition) == neighborTileDistance)
		{
			Vector3 _goalPosition = emptyTilePosition;
			emptyTilePosition = tile.GetComponent<RectTransform>().localPosition;
			tile.OnMoveTo(_goalPosition);
			moveCount ++;
		}
	}
	public void GameOver()
	{
		List<Tile> tiles = tileList.FindAll(x => x.IsCorrected == true);

		Debug.Log("Correct Count : "+tiles.Count);
		if ( tiles.Count == puzzleSize.x * puzzleSize.y - 1 )
		{
			Debug.Log("GameClear");
			StopCoroutine("CalculatePlaytime");
			GetComponent<UIController>().OnResultPanel();
		}
	}
	private IEnumerator CoCountTime()
	{
		while (true)
		{
			playTime++;
			yield return new WaitForSeconds(1f);
		}
	}
}
