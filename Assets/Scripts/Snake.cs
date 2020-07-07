using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Unity.MLAgents.Policies;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;

//using UnityEngine.Serialization;

//var enumModeStatus : BehaviorType.HeuristicOnly;


public class Snake : Agent
{


    float direction = 0.0f;         // user's input
    float speed = 2.0f;             // speed of snake's head
    float angularSpeed = 110.0f;    // determines the smallest circle that a snake can draw
    float width = 0.3f;             // width of head & tail
    float breakTime = 0.42f;        // the length of a gap between tails
    float shiftTime = 0.7f;         // delay before drawing the 1st tail
    float pointSpacing = 0.16f;     // allows to reduce the number of primitives in the tail (better performance)

    public int score = 0;

    bool tailActive = false;
    bool isAlive = true;
    bool isCollision = false;

    List<SnakeTail> snakeTails;

    Vector3 originalPosition;
    Vector3 prevPosition;
    Quaternion originalRotation;
    Transform enemy;

    public int snakeID;   
    private Color color;
    public SnakeTail tailPrefab;
    public AchtungArea map;

    Coroutine thread; 

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
       
        makePolygon();       
        col.radius = width / 2.0f;
        breakTime = width * 1.5f;
        //pointSpacing = width / 3.2f; // ??
    }


    public void setColor(Color c)
    {
        color = c;
        meshRenderer.material.color = new Vector4(color[0], color[1], color[2], 1.0f);
    }


    public void setPosition(float x, float y, float angle)
    {
        transform.Rotate(0f, 0f, angle);
        transform.Translate(x, y, 0.0f);
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    
    public void setRandomPosition()
    {
        transform.position = map.transform.position;

        float angle = Random.Range(0.0f, 360.0f);
        float d = map.spawnRange;
        float x = Random.Range(-d, d);
        float y = Random.Range(-d, d);

        transform.Rotate(0f, 0f, angle);
        transform.Translate(x, y, 0.0f);
    }

    public void setEnemy(Transform trans)
    {
        enemy = trans;
    }


    public override void CollectObservations(VectorSensor sensor)
    {
        //sensor.AddObservation(map.transform.localPosition);
        //sensor.AddObservation(this.transform.localPosition);
    }

    public override void OnEpisodeBegin()
    {
        setRandomPosition();
        isAlive = true;
        prevPosition = new Vector3(-99, -99, 0);
        AddTail();
        thread = StartCoroutine(TailDrawer());
    }

    public override void Heuristic(float[] actionsOut)
    {
        float action = -Input.GetAxisRaw("Horizontal");

        switch (action)
        {
            case 0.0f:
                // do nothing
                actionsOut[0] = 0.0f;
                break;
            case -1.0f:
                // move left
                actionsOut[0] = 1.0f;
                break;
            case 1.0f:
                // move right
                actionsOut[0] = 2.0f;
                break;
            default:
                Debug.Log("Invalid action value");
                break;
        }
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        SetReward(0.01f);
        var action = Mathf.FloorToInt(vectorAction[0]);
        switch (action)
        {
            case 0:
                // do nothing
                direction = 0;
                break;
            case 1:
                // move left
                direction = -1;
                break;
            case 2:
                // move right
                direction = 1;
                break;
            default:
                Debug.Log("Invalid action value");
                break;
        }
    }


    /// Regular functions ///

    void FixedUpdate()
    {
        if (!isAlive)
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
        if (!isAlive)
        {
            return;
        }
        //PollPlayerInput();
    }


    void AddTail()
    {
        SnakeTail tail = Instantiate(tailPrefab, Vector3.zero, Quaternion.identity) as SnakeTail;
        
        tail.SetColor(color);
        tail.SetWidth(width);
        tail.SetOffset((int)(width * 8));
        tail.transform.parent = map.transform; // optional, for convenience
        snakeTails.Add(tail);
        
    }


    IEnumerator TailDrawer()
    {
        //Debug.Log("Courutine started");
        tailActive = false;
        yield return new WaitForSeconds(shiftTime);
        tailActive = true;

        while (isAlive)
        {
            yield return new WaitForSeconds(Random.Range(0.1f, 7.0f));
            tailActive = false;
                      
            yield return new WaitForSeconds(breakTime);

            if (isCollision)
            {
                Death();
                //Debug.Log("Ghost form ended inside an object! (courutine aborted)");
                yield break;                
            }

            AddTail();
            snakeTails[snakeTails.Count - 2].AdjustCollider(); // remove offset in the previous tail

            tailActive = true;     
        }
        //Debug.Log("Courutine ended");
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
            Death();
        }
        
    }


    public void Teardown()
    {
        StopCoroutine(thread);
        for (int i = 0; i < snakeTails.Count; i++)
        {
            snakeTails[i].Destroy();
        }
        snakeTails.Clear();

        //transform.position = originalPosition;
        //transform.rotation = originalRotation;
    }

    void Death()
    {
        isAlive = false;
        map.registerDeath(snakeID);    
        //GameObject.Destroy(head.gameObject);
        //SceneManager.LoadScene(0);
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