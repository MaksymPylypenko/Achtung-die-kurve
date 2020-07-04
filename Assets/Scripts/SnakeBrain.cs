using UnityEngine;

public class SnakeBrain
{
    public virtual float Move()
    {
        return 0.0f;
    }
}


public class RandomAI : SnakeBrain
{
    int thinkTime = 30;
    float currMove = 0.0f;
    public override float Move()
    {
        if (thinkTime == 0)
        {
            currMove = Random.Range(-1, 2);
            thinkTime = Random.Range(5, 30); 
        }
        thinkTime--;
        return currMove;
    }
}


public class PlayerTouch : SnakeBrain
{
    public override float Move()
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            if (touch.position.x < Screen.width / 2)
            {
                Debug.Log("Left click");
                return 1.0f;
            }
            else if (touch.position.x > Screen.width / 2)
            {
                Debug.Log("Right click");
                return -1.0f;
            }

        }
        return 0;
    }
}

public class PlayerKeyboard : SnakeBrain
{
    public override float Move()
    {
        return -Input.GetAxisRaw("Horizontal");
    }
}

