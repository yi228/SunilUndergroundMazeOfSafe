using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

class MapGenerator : MonoBehaviour
{
	// 위 아래 왼쪽 오른쪽
	int[] dy = { 1, -1, 0, 0 };
	int[] dx = { 0, 0, -1, 1 };
	struct Pos
    {
        public int y;
        public int x;
        public Pos(int y, int x)
        {
            this.y = y;
            this.x = x;
        }
    }
	public Define.TileType[,] Tiles { get; private set; }
	public int Size { get; private set; }

	public int DestY { get; private set; }
	public int DestX { get; private set; }
    public int StartY;
    public int StartX;
    public int[,] visited;
	private Queue<Pos> queue = new Queue<Pos>();
	public void Initialize(int size)
	{
		if (size % 2 == 0)
			return;

        Tiles = new Define.TileType[size, size];
		Size = size;

		DestY = Size - 2;
		DestX = Size - 2;
		visited = new int[size, size];
		for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
				visited[y, x] = 0;
            }
        }
		// Mazes for Programmers
		GenerateBySideWinder();
		FindGoal();
	}
	void FindGoal()
    {
		queue.Enqueue(new Pos(1, Size - 2));
		visited[1, Size - 2] = 1;
		while (queue.Count != 0)
		{
            Pos current = queue.Dequeue();
            int y = current.y;
            int x = current.x;
            for (int i = 0; i < 4; i++)
			{
				int ny = y + dy[i];
				int nx = x + dx[i];
				// 범위
				if (!(0 <= ny && ny < Size && 0 <= nx && nx < Size)) continue;
				// 길이 있나
				if (Tiles[ny, nx] == Define.TileType.Wall) continue;
				// 가본 적 있나
				if (visited[ny, nx] != 0) continue;
				visited[ny, nx] = visited[y, x] + 1;
				queue.Enqueue(new Pos(ny, nx));
			}
		}
		Pos goalPos = new Pos(); 
		int max = 0;
		for (int y = 0; y < Size; y++)
		{
			for (int x = 0; x < Size; x++)
			{
				if (max < visited[y, x])
                {
					max = visited[y, x];
					goalPos.y = y;
					goalPos.x = x;
                }
			}
        }
        Tiles[goalPos.y, goalPos.x] = Define.TileType.Boss;
        string s = "";
        for (int y = 0; y < Size; y++)
        {
            for (int x = 0; x < Size; x++)
            {
				s += visited[y, x];
            }
			s += '\n';
        }
	}
	void GenerateBySideWinder()
	{
		// 일단 길을 다 막아버리는 작업
		for (int y = 0; y < Size; y++)
		{
			for (int x = 0; x < Size; x++)
			{
				if (x % 2 == 0 || y % 2 == 0)
                    Tiles[y, x] = Define.TileType.Wall;
				else
                    Tiles[y, x] = Define.TileType.Common;
			}
		}

        // 랜덤으로 우측 혹은 아래로 길을 뚫는 작업
        System.Random rand = new System.Random();
		for (int y = 0; y < Size; y++)
		{
			int count = 1;
			for (int x = 0; x < Size; x++)
			{
				if (x % 2 == 0 || y % 2 == 0)
					continue;

				if (y == Size - 2 && x == Size - 2)
					continue;

				if (y == Size - 2)
				{
                    Tiles[y, x + 1] = Define.TileType.Common;
					continue;
				}

				if (x == Size - 2)
				{
                    Tiles[y + 1, x] = Define.TileType.Common;
					continue;
				}

				if (rand.Next(0, 2) == 0)
				{
                    Tiles[y, x + 1] = Define.TileType.Common;
					count++;
				}
				else
				{
					int randomIndex = rand.Next(0, count);
                    Tiles[y + 1, x - randomIndex * 2] = Define.TileType.Common;
					count = 1;
				}
			}
		}
        Tiles[1, Size - 2] = Define.TileType.Start;
		StartY = 1;
		StartX = Size - 2;
	}
}
