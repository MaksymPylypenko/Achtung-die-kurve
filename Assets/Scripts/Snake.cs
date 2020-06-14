using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Snake : MonoBehaviour
{
    // unique for each snake
    public int snakeID;
    public Color color;

    // objects
    public Transform head;
    public SnakeTail tailPrefab;

    // position in game
    public float headAngle;
    public Vector3 headPosition;

    // properties
    public float speed = 3f;
    public float angularSpeed = 4f;       
    public float width = 0.3f;

    // gaps
    public float lineTime = 3f;
    public float breakTime = 0.45f;

    System.Random rg;
    List<SnakeTail> snakeTails;

    // status
    bool tailActive = true;
    bool gameOver = false;
    bool isReady = false;


    void Awake()
    {
        rg = new System.Random();
        snakeTails = new List<SnakeTail>();
    }

     
    public void StartSnake()
    {
        AddTail();
        StartCoroutine(TailDrawer());
        isReady = true;
    }

    void AddTail()
    {
        SnakeTail tail = Instantiate(tailPrefab, Vector3.zero, Quaternion.identity) as SnakeTail;
        tail.SetColor(color);
        tail.SetWidth(width);

        snakeTails.Add(tail);
    }


    IEnumerator TailDrawer()
    {
        while (!gameOver)
        {
            float randomLineTime = (float)rg.NextDouble() - 0.5f;
            yield return new WaitForSeconds(lineTime + randomLineTime);

            tailActive = false;

            float randomBreakTime = (float)rg.NextDouble() * (breakTime * 0.5f) - (breakTime * 0.5f);
            yield return new WaitForSeconds(breakTime + randomBreakTime);

            AddTail();

            tailActive = true;
        }
    }



    void FixedUpdate()
    {
        if (gameOver || !isReady)
        {
            return;
        }
             
        headAngle += angularSpeed * GetSteerDirection();

        //transform.Translate(Vector2.up * speed * Time.fixedDeltaTime, Space.Self);
        //transform.Rotate(Vector3.forward * headAngle * Time.fixedDeltaTime);

        headPosition.x += speed * Mathf.Cos(headAngle * Mathf.PI / 180f) * Time.fixedDeltaTime;
        headPosition.y += speed * Mathf.Sin(headAngle * Mathf.PI / 180f) * Time.fixedDeltaTime;

        if (tailActive)
        {
            GetCurrentSnakeTail().UpdateTail(headPosition);
            //GetCurrentSnakeTail().UpdateTail(transform.position);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (gameOver || !isReady)
        {
            return;
        }

        head.position = headPosition;
        head.localRotation = Quaternion.Euler(0, 0, headAngle); 
    }



    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Collision detected");
        //GameOver();
    }


    void GameOver()
    {
        gameOver = true;

        for (int i = 0; i < snakeTails.Count; i++)
        {
            GameObject.Destroy(snakeTails[i].gameObject);
        }

        snakeTails.Clear();

        GameObject.Destroy(head.gameObject);
    }


    float GetSteerDirection()
    {
        if (snakeID == 0)
        {
            if (Input.GetKey(KeyCode.A))
            {
                return 1f;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                return -1f;
            }
            else
            {
                return 0;
            }
        }

        if (snakeID == 1)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                return 1f;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                return -1f;
            }
            else
            {
                return 0;
            }
        }

        return 0;
    }

    SnakeTail GetCurrentSnakeTail()
    {
        return snakeTails[snakeTails.Count - 1];
    }
}
