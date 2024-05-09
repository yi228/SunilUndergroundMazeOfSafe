using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MazeGeneratorByRecursiveBacktracking : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;

    private int[,] map;

    private const int ROAD = 0;
    private const int WALL = 1;
    private const int CHECK = 2;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject baseBlock;
    [SerializeField] private GameObject wallBlock;
    [SerializeField] private GameObject destBlockPrefab;

    private GameObject player;
    private GameObject dest;

    private Vector2Int[] dir = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
    private Vector2Int pos = Vector2Int.zero;
    private Stack<Vector2Int> stackDir = new Stack<Vector2Int>(); //지나온 길의 방향 저장

    void Start()
    {
        Camera.main.gameObject.SetActive(false);
        Generate();
    }
    public void ResetMaze()
    {
        GameObject[] _baseBlocks = GameObject.FindGameObjectsWithTag("mazeBase");
        GameObject[] _wallBlocks = GameObject.FindGameObjectsWithTag("mazeWall");

        foreach (GameObject _baseBlock in _baseBlocks)
            Destroy(_baseBlock);
        foreach (GameObject _wallBlock in _wallBlocks)
            Destroy(_wallBlock);
        Destroy(player);
        Destroy(dest);

        Generate();
    }
    public void Generate()
    {
        Init();
        RandomPosSelect();
        GenerateMaze();
    }
    private void Init()
    {
        map = new int[width, height];
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                map[x, y] = WALL;
    }
    private void RandomPosSelect()
    {
        while (true)
        {
            pos = new Vector2Int(Random.Range(0, width), Random.Range(0, height));

            if (pos.x % 2 != 0 && pos.y % 2 != 0)
                break;
        }

        map[pos.x, pos.y] = ROAD;
    }
    private void RandomDir()
    {
        for (int i = 0; i < dir.Length; i++)
        {
            int randNum = Random.Range(0, dir.Length); 
            Vector2Int temp = dir[randNum]; 
            dir[randNum] = dir[i];
            dir[i] = temp;
        }
    }
    private void GenerateMaze()
    {
        while (true)
        {
            int index = -1; 

            RandomDir(); 

            for (int i = 0; i < dir.Length; i++)
            {
                if (CheckBlock(i))
                {
                    index = i; 
                    break;
                }
            }
            if (index != -1) 
            {
                for (int i = 0; i < 2; i++) 
                {
                    stackDir.Push(dir[index]); 
                    pos += dir[index]; 
                    map[pos.x, pos.y] = CHECK;
                }
            }
            else 
            {
                for (int i = 0; i < 2; i++) 
                {
                    map[pos.x, pos.y] = ROAD; 
                    pos += stackDir.Pop() * -1; 
                }
            }
            if (stackDir.Count == 0)
                break;
        }

        SpawnBlock();
    }
    private void SpawnBlock()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (map[x, y] == WALL)
                    Instantiate(wallBlock, new Vector2(x-100, y), Quaternion.identity, transform);
                else if(map[x, y] == ROAD)
                    Instantiate(baseBlock, new Vector2(x-100, y), Quaternion.identity, transform);
            }
        }
        player = Instantiate(playerPrefab, new Vector2(-99, 1), Quaternion.identity, transform);
        dest = Instantiate(destBlockPrefab, new Vector2(width - 102, height - 2), Quaternion.identity, transform);
    }
    private bool CheckBlock(int index)
    {
        Vector2Int _futurePos = pos + dir[index] * 2;
        if (_futurePos.x > width - 2 || _futurePos.y > height - 2 || _futurePos.x < 0 || _futurePos.y < 0 || map[_futurePos.x, _futurePos.y] != WALL)
            return false;
        else
            return true;
    }
}
