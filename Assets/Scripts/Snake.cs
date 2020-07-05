using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
//using System.Diagnostics;

public class Snake : Agent
{  
    float direction = 0.0f;
    float speed = 2.0f;
    float angularSpeed = 110.0f;
    float width = 0.3f;
    float breakTime = 0.42f;
    float shiftTime = 0.7f;
    float pointSpacing = 0.16f;

    bool tailActive = true;
    bool gameOver = false;
    bool isReady = false;
    bool isCollision = false;

    List<SnakeTail> snakeTails;

    public Color color;
    public SnakeBrain controller;
    public SnakeTail tailPrefab;
    public GameObject map;

    Coroutine thread;
    Vector3 initPosition;
    Vector3 prevPosition;
    CircleCollider2D col;
    Mesh mesh;
    MeshRenderer meshRenderer;
    MeshFilter meshFilter;


    /// ML-Agents functions ///
    
    public override void Initialize()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        meshFilter = gameObject.GetComponent<MeshFilter>();
        col = gameObject.GetComponent<CircleCollider2D>();
        snakeTails = new List<SnakeTail>();

        meshRenderer.material.color = new Vector4(color[0], color[1], color[2], 1.0f);
        makePolygon();       
        col.radius = width / 2.0f;
        breakTime = width * 1.5f;
        initPosition = transform.position;
        //pointSpacing = width / 3.2f; // ??
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //sensor.AddObservation(head.position.x);
        //sensor.AddObservation(head.position.y);
    }

    public override void OnEpisodeBegin()
    {

    }

    public override void Heuristic(float[] actionsOut)
    {
       // actionsOut[0] = Input.GetAxis("Horizontal");
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        //direction = vectorAction[0];
        //Debug.Log(direction);
    }


    /// Regular functions ///

    void FixedUpdate()
    {
        if (gameOver || !isReady)
        {
            return;
        }

        transform.Translate(Vector2.up * speed * Time.fixedDeltaTime, Space.Self);
        if (direction != 0.0f)
        {
            transform.Rotate(Vector3.forward * angularSpeed * direction * Time.fixedDeltaTime);
        }

        if (tailActive)
        {
            if (Vector3.Distance(prevPosition, transform.position) >= pointSpacing)
            {
                GetCurrentSnakeTail().UpdateTail(transform.position);
                prevPosition = transform.position;
            }

        }
    }


    void Update()
    {
        if (gameOver || !isReady)
        {
            return;
        }
        PollPlayerInput();
    }


    public void StartSnake()
    {        
        transform.position = initPosition;
        prevPosition = new Vector3(-99, -99, 0);
        AddTail();
        isReady = true;
        thread = StartCoroutine(TailDrawer());       
    }

    public void ResetSnake()
    {     
        //Debug.Log("Tails count = " +snakeTails.Count);
        for (int i = 0; i < snakeTails.Count; i++)
        {
            snakeTails[i].Destroy();
        }
        snakeTails.Clear();
        snakeTails = new List<SnakeTail>();
        StartSnake();        
    }

    void AddTail()
    {
        SnakeTail tail = Instantiate(tailPrefab, Vector3.zero, Quaternion.identity) as SnakeTail;
        
        tail.SetColor(color);
        tail.SetWidth(width);
        tail.SetOffset((int)(width * 15));
        tail.transform.parent = map.transform; // optional, for convenience
        snakeTails.Add(tail);
        
    }


    IEnumerator TailDrawer()
    {
        Debug.Log("Courutine started");
        tailActive = false;
        yield return new WaitForSeconds(shiftTime);
        tailActive = true;

        while (!gameOver)
        {
            yield return new WaitForSeconds(Random.Range(0.1f, 7.0f));
            tailActive = false;
                      
            yield return new WaitForSeconds(breakTime);

            if (isCollision)
            {
                GameOver();
                //Debug.Log("Edge case");
                Debug.Log("Courutine aborted");
                yield break;                
            }

            AddTail();
            snakeTails[snakeTails.Count - 2].AdjustCollider(); // remove offset in the previous tail

            tailActive = true;     
        }
        Debug.Log("Courutine ended");
    }


    void OnTriggerExit2D(Collider2D other)
    {
        isCollision = false;
        //Debug.Log("CollisionExit");        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        isCollision = true;
        if (tailActive)
        {
            //Debug.Log("Collision at " + other.transform.position);
            GameOver();
        }
        
    }


    void GameOver()
    {
        //gameOver = true;
        StopCoroutine(thread);        
        isReady = false;

        ResetSnake();

        //GameObject.Destroy(head.gameObject);
        //SceneManager.LoadScene(0);
    }


    void PollPlayerInput() // This should depend on the current player controlling the snake..
    {
        direction = controller.Move();
    }

    SnakeTail GetCurrentSnakeTail()
    {
        return snakeTails[snakeTails.Count - 1];
    }


    void makePolygon()
    {
        float w = width / 2.0f + 0.01f;
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[4]
        {
                new Vector3(-w, -w, 0),
                new Vector3(w, -w, 0),
                new Vector3(-w, w, 0),
                new Vector3(w, w, 0)
        };
        mesh.vertices = vertices;

        int[] tris = new int[6]
        {
                // lower left triangle
                0, 2, 1,
                // upper right triangle
                2, 3, 1
        };
        mesh.triangles = tris;

        Vector2[] uv = new Vector2[4]
        {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(0, 1),
                new Vector2(1, 1)
        };
        mesh.uv = uv;
        meshFilter.mesh = mesh;
    }

}