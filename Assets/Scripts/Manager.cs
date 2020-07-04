using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public Snake snakePrefab;
    //public RandomAI randomAIPrefab;

    public Color[] snakeColors = { Color.green, Color.yellow, Color.white, Color.blue, Color.yellow };

    List<Snake> snakes;

    //public GameObject RestartButton;

    void Awake()
    {
        //RestartButton = GetComponent<GameObject>();
    }

    void Start()
    {
        snakes = new List<Snake>();

               
        if (Application.platform == RuntimePlatform.Android)
        {
            AddSnake(0, Vector3.left, 90f, new PlayerTouch());
        }
        else
        {
            AddSnake(0, Vector3.left, 90f, new PlayerKeyboard());
        }


        AddSnake(1, Vector3.right, 45f, new RandomAI());

        for (int i = 0; i < snakes.Count; i++)
        {
            snakes[i].StartSnake();
        }

        FollowSnake(0);
    }

    void AddSnake(int snakeID, Vector3 headPosition, float headAngle, SnakeBrain brain)
    {
        Snake snake = Instantiate(snakePrefab, Vector3.zero, Quaternion.identity) as Snake;
        snake.color = snakeColors[snakeID];
        snake.headAngle = headAngle;
        snake.headPosition = headPosition;
        snake.snakeID = snakeID;
        snake.controller = brain;

        snakes.Add(snake);
    }

    void FollowSnake(int snakeID)
    {
        Camera.main.GetComponent<CameraFollow>().setTarget(snakes[snakeID]);
    }
}