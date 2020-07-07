using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AchtungArea : MonoBehaviour
{
    public float spawnRange = 4.0f;
    float wallRange = 10.0f;
    float wallWidth = 0.7f;

    public Walls walls;
    public Snake snakePrefab;

    public List<Color> colors;

    Snake snakeA;
    Snake snakeB;

    // Start is called before the first frame update
    void Start()
    {
        walls.SetWalls(wallRange, wallWidth);

        snakeA = createSnake(0);
        snakeB = createSnake(1);
        

        snakeA.setEnemy(snakeB.transform);
        snakeB.setEnemy(snakeA.transform);

        //Camera.main.GetComponent<CameraFollow>().setTarget(snakeA);
    }


    public void registerDeath(int snakeID)
    {
        if(snakeID == 0)
        {
            snakeA.SetReward(-1);
            snakeB.SetReward(1);
            snakeB.score++;
            //Debug.Log("B wins!");
        }
        else 
        {
            snakeA.SetReward(1);
            snakeB.SetReward(-1);
            snakeA.score++;
            //Debug.Log("A wins!");
        }

        snakeA.Teardown();
        snakeB.Teardown();

        snakeA.EndEpisode();
        snakeB.EndEpisode();
    }


    Snake createSnake(int snakeID)
    {
        Snake snake = Instantiate(snakePrefab, transform.position, Quaternion.identity) as Snake;
        snake.snakeID = snakeID;
        snake.map = this;
        snake.setColor(colors[snakeID]);
        snake.transform.parent = this.transform; 
        return snake;
    }

    /// More than 2 snakes...

    //List<Snake> snakes;
    //List<bool> alive;

    //public void RegisterDeth()
    //{
    //    alive[snakeID] = false;
    //    snakes[snakeID].SetReward(-1);

    //    int winnerID = -1;
    //    int aliveCount = 0;

    //    for (int i = 0; i < snakes.Count; i++)
    //    {
    //        if (alive[i])
    //        {
    //            aliveCount++;
    //            winnerID = i;
    //        }
    //    }

    //    if (aliveCount == 1)
    //    {
    //        snakes[winnerID].SetReward(1);
    //    }
    //}

    //void AddSnake(int snakeID, float offsetX, float offsetY, float angle)
    //{
    //    Snake snake = Instantiate(snakePrefab, Vector3.zero, Quaternion.identity) as Snake;
    //    snake.transform.Rotate(0f, 0f, angle);
    //    snake.transform.Translate(offsetX, offsetY, 0.0f);
    //    snake.color = snakeColors[snakeID];
    //    snake.snakeID = snakeID;
    //    snakes.Add(snake);
    //    alive.Add(true);
    //}

}
