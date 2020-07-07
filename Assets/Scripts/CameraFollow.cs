using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Snake snake;

    public bool followOrigin = true;
    public bool followDirection = true;

    void FixedUpdate()
    {
        if (snake != null)
        {
            if (followOrigin)
            {
                transform.position = new Vector3(snake.transform.position.x, snake.transform.position.y, transform.position.z);
            }
            if (followDirection)
            {
               transform.localRotation = snake.transform.localRotation;
            }            
        }
    }

    public void setTarget(Snake target)
    {
        snake = target;
    }
}


