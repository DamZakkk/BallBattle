using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeRenderer : MonoBehaviour
{

    [SerializeField]
    [Range(1, 50)]
    private int width = 10;

    [SerializeField]
    [Range(1, 50)]
    private int height = 10;

    [SerializeField]
    private float size = 1f;

    [SerializeField]
    private Transform wallPrefab = null;

    [SerializeField]
    private Transform floorPrefab = null;

    [SerializeField]
    private Transform parent = null;

    // Start is called before the first frame update
    void Start()
    {
        var maze = MazeGenerator.Generate(width, height);
        Draw(maze);
    }

    private void Draw(WallState[,] maze)
    {
        print(transform.root.localScale.x);
        //var floor = Instantiate(floorPrefab, transform);
        //floor.localScale = new Vector3(width, 1, height);

        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                var cell = maze[i, j];
                var position = new Vector3(-width / 2 + i, 0, -height / 2 + j) * transform.root.localScale.x;

                if (cell.HasFlag(WallState.UP) && j != height-1)
                {
                    var topWall = Instantiate(wallPrefab, transform) as Transform;
                    topWall.position = position + new Vector3(0, 0, size / 2) * transform.root.localScale.x;
                    topWall.localScale = new Vector3(size, topWall.localScale.y, topWall.localScale.z);
                }

                if (cell.HasFlag(WallState.LEFT) && i != 0)
                {
                    var leftWall = Instantiate(wallPrefab, transform) as Transform;
                    leftWall.position = position + new Vector3(-size / 2, 0, 0) * transform.root.localScale.x;
                    leftWall.localScale = new Vector3(size, leftWall.localScale.y, leftWall.localScale.z);
                    leftWall.eulerAngles = new Vector3(0, 90, 0);
                }

                //if (i == width - 1)
                //{
                //    //if (cell.HasFlag(WallState.RIGHT))
                //    //{
                //    //    var rightWall = Instantiate(wallPrefab, transform) as Transform;
                //    //    rightWall.position = position + new Vector3(+size / 2, 0, 0);
                //    //    rightWall.localScale = new Vector3(size, rightWall.localScale.y, rightWall.localScale.z);
                //    //    rightWall.eulerAngles = new Vector3(0, 90, 0);
                //    //}
                //}

                //if (j == 0)
                //{
                //    //if (cell.HasFlag(WallState.DOWN))
                //    //{
                //    //    var bottomWall = Instantiate(wallPrefab, transform) as Transform;
                //    //    bottomWall.position = position + new Vector3(0, 0, -size / 2);
                //    //    bottomWall.localScale = new Vector3(size, bottomWall.localScale.y, bottomWall.localScale.z);
                //    //}
                //}
            }

        }

        transform.parent = parent;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;

    }

    // Update is called once per frame
    void Update()
    {

    }
}
