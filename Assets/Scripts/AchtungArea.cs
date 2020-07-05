using UnityEngine;

public class AchtungArea : MonoBehaviour
{
    public Walls walls;
    public Snake snakeA;
    public Snake snakeB;


    // Start is called before the first frame update
    void Start()
    {
        walls.SetWalls(10.0f, 10.0f);
        walls.SetWidth(0.3f);

        snakeA.StartSnake();
        snakeB.StartSnake();

        snakeA.controller = new PlayerKeyboard();
        snakeB.controller = new RandomAI();
        Camera.main.GetComponent<CameraFollow>().setTarget(snakeA);
    }


    void MatchReset()
    {

    }

}
