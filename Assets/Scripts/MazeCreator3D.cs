using System;
using UnityEngine;

public class MazeCreator3D : MonoBehaviour
{
    public int width = 21;
    public int depth = 21;

    public GameObject wallObj;
    public GameObject startObj;
    public GameObject goalObj;

    public float cellSize = 1f;

    // ★追加：高さ調整用
    public float wallHeight = 2f;

    int[,] maze;

    static int Path = 0;
    static int Wall = 1;

    void Awake()
    {
        if (width % 2 == 0) width++;
        if (depth % 2 == 0) depth++;

        maze = Generate2D(width, depth);
        Build();
    }

    int[,] Generate2D(int width, int height)
    {
        int[,] maze = new int[width, height];

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                maze[x, y] = Wall;

        System.Random rnd = new System.Random();

        void Carve(int x, int y)
        {
            maze[x, y] = Path;

            var dirs = new (int dx, int dy)[]
            {
                (2,0), (-2,0), (0,2), (0,-2)
            };

            for (int i = 0; i < dirs.Length; i++)
            {
                var t = dirs[i];
                int r = rnd.Next(i, dirs.Length);
                dirs[i] = dirs[r];
                dirs[r] = t;
            }

            foreach (var (dx, dy) in dirs)
            {
                int nx = x + dx;
                int ny = y + dy;

                if (nx > 0 && ny > 0 &&
                    nx < width - 1 && ny < height - 1 &&
                    maze[nx, ny] == Wall)
                {
                    maze[x + dx / 2, y + dy / 2] = Path;
                    Carve(nx, ny);
                }
            }
        }

        Carve(1, 1);
        return maze;
    }

    void Build()
    {
        for (int x = 0; x < width; x++)
            for (int z = 0; z < depth; z++)
            {
                if (maze[x, z] == Wall)
                {
                    Vector3 pos = new Vector3(
                        x * cellSize,
                        0,
                        z * cellSize
                    );

                    GameObject wall = Instantiate(wallObj, pos, Quaternion.identity, transform);

                    // ★高さ調整ここ
                    wall.transform.localScale = new Vector3(1, wallHeight, 1);

                    // ★地面から浮かせる（重要）
                    wall.transform.position += Vector3.up * (wallHeight / 2f);
                }
            }

        Instantiate(startObj, new Vector3(1 * cellSize, 0, 1 * cellSize), Quaternion.identity);
        Instantiate(goalObj, new Vector3((width - 2) * cellSize, 0, (depth - 2) * cellSize), Quaternion.identity);
    }
}