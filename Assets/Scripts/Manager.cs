using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public Snake snakePrefab;

    public Color[] snakeColors;

    List<Snake> snakes;

    //public GameObject RestartButton;

    void Awake()
    {
        //RestartButton = GetComponent<GameObject>();
    }

    void Start()
    {
        snakes = new List<Snake>();

        AddSnake(0, Vector3.left, 90f);
        //AddSnake(1, Vector3.right, 90f);

        for (int i = 0; i < snakes.Count; i++)
        {
            snakes[i].StartSnake();
        }

        FollowSnake(0);
    }

    void AddSnake(int snakeID, Vector3 headPosition, float headAngle)
    {
        Snake snake = Instantiate(snakePrefab, Vector3.zero, Quaternion.identity) as Snake;
        snake.color = snakeColors[snakeID];
        snake.headAngle = headAngle;
        snake.headPosition = headPosition;
        snake.snakeID = snakeID;

        snakes.Add(snake);
    }

    void FollowSnake(int snakeID)
    {
        Camera.main.GetComponent<CameraFollow>().setTarget(snakes[snakeID].transform);
        //cameraMovement.snake = snakes[0];
    }
}