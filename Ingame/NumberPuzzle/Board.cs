using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
	[SerializeField] private GameObject tilePrefab; // 숫자 타일 프리팹
	[SerializeField] private Transform tileParent; // 타일이 배치되는 "Board" 오브젝트의 Transform

	private	List<Tile> tileList; // 생성한 타일 정보 저장

	private	Vector2Int puzzleSize = new Vector2Int(3, 3); // 4x4 퍼즐
	private float neighborTileDistance; // 인접한 타일 사이의 거리. 별도로 계산할 수도 있다.

	public Vector3 emptyTilePosition { get; set; } // 빈 타일의 위치
	public int playTime { get; private set; } = 0;
	public int moveCount { get; private set; } = 0; // 이동 횟수

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
