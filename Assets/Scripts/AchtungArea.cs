using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AchtungArea : MonoBehaviour
{
    public int numPlayers = 4;

    public float spawnRange = 4.0f;
    float wallRange;
    float wallWidth = 0.3f;

    public Walls walls;
    public Snake snakePrefab;

    public List<Color> colors;
    List<Snake> snakes;

    // Start is called before the first frame update
    void Start()
    {
        wallRange = numPlayers * 4.0f;
        snakes = new List<Snake>();
        walls.SetWalls(wallRange, wallWidth);

        for (int i = 0; i < numPlayers; i++)
        {
            snakes.Add(createSnake(i));
        }          

        snakes[0].SetModel("human", null);
        //Camera.main.GetComponent<CameraFollow>().setTarget(snakes[0]);
    }


    public void registerDeath(int snakeID)
    {
        int numAlive = 0;
        int lastAlive = -1;

        for (int i = snakes.Count-1; i >= 0; i--)
        {
            if (snakes[i].isAlive)
            {
                numAlive++;
                lastAlive = i;
            }
        }

        if (numAlive <= 1)
        {
            for (int i = 0; i < snakes.Count; i++)
            {
                if(lastAlive == i)
                {
                    snakes[i].SetReward(1);
                }
                else
                {
                    snakes[i].SetReward(-1);
                }
                //snakes[i].Teardown();
                //snakes[i].EndEpisode();
            }
        }

        //Camera.main.GetComponent<CameraFollow>().setTarget(snakes[lastAlive]);
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

}
