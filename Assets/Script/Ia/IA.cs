using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IA : MonoBehaviour
{
    [Header("Life")]
    public float Life = 100f;

    public float dammage;

    [Header("AI Characteristic")]
    public float startWaitTime = 4;                 //  Wait time of every action
    public float timeToRotate = 2;                  //  Wait time when the enemy detect near the player without seeing
    public float speedWalk = 6;                     //  Walking speed, speed in the nav mesh agent
    public float speedRun = 9;                      //  Running speed

    [Header("Need for Work")]
    public NavMeshAgent navMeshAgent;               //  Nav mesh agent component
    public GameManager GameManger;                  // Need GameManager with public  Transform[] waypoints in this Script
    public Animator animator;                       //  Animator in gameobject
    public DetectPlayer DetectPlayerScript;         //Collider detectPlayer

    [Header("Bool Movement")]
    public bool _Stop;
    public bool Walk;
    public bool Chasse;
    public bool BoolAttack;

    [Header("View AI")]
    public float viewRadius = 15;                   //  Radius of the enemy view
    public float viewAngle = 90;                    //  Angle of the enemy view
    public LayerMask playerMask;                    //  To detect the player with the raycast
    public LayerMask obstacleMask;                  //  To detect the obstacules with the raycast
    public float meshResolution = 1.0f;             //  How many rays will cast per degree
    public int edgeIterations = 4;                  //  Number of iterations to get a better performance of the mesh filter when the raycast hit an obstacule
    public float edgeDistance = 0.5f;               //  Max distance to calcule the a minumun and a maximum raycast when hits something
  

    [Header("Attack AI")]
    public float AttackRadius = 5;
    //  All the waypoints where the enemy patrols

    int m_CurrentWaypointIndex;                     //  Current waypoint where the enemy is going to

    Vector3 playerLastPosition = Vector3.zero;      //  Last position of the player when was near the enemy
    Vector3 m_PlayerPosition;                       //  Last position of the player when the player is seen by the enemy

    float m_WaitTime = 4;                               //  Variable of the wait time that makes the delay
    float m_TimeToRotate;                           //  Variable of the wait time to rotate when the player is near that makes the delay
    bool m_playerInRange;                           //  If the player is in range of vision, state of chasing
    bool m_PlayerNear;                              //  If the player is near, state of hearing
    bool m_IsPatrol;                                //  If the enemy is patrol, state of patroling
    bool m_CaughtPlayer;                            //  if the enemy has caught the player

    void Awake()
    {
        GameManger = FindObjectOfType<GameManager>();
        GameManger.ZombieAlive++;
        m_WaitTime = startWaitTime;                 //  Set the wait time variable that will change
        m_PlayerPosition = Vector3.zero;
        m_TimeToRotate = timeToRotate;
        m_CurrentWaypointIndex = Random.Range(0,10);                 //  Set the initial waypoint
        //Debug.Log(m_CurrentWaypointIndex);
        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speedWalk;             //  Set the navemesh speed with the normal speed of the enemy
        navMeshAgent.SetDestination(GameManger.waypoints[m_CurrentWaypointIndex].position);    //  Set the destination to the first waypoint
        navMeshAgent.speed = speedWalk;
        //Debug.Log(navMeshAgent.speed);
    }

    public void Start()
    {
        m_IsPatrol = true;
        Debug.Log(m_IsPatrol);
        Debug.Log(m_TimeToRotate);
        m_CaughtPlayer = false;
        m_playerInRange = false;
        m_PlayerNear = false;
        Debug.Log(m_PlayerNear);
    }

    private void Update()
    {
        EnviromentView();                       //  Check whether or not the player is in the enemy's field of vision

        if (!m_IsPatrol)
        {
            Chasing();
        }
        else
        {
            Patroling();
        }

        if (_Stop)
        {
            animator.SetBool("Walking" , false);
            animator.SetBool("Chase", false);
        }
        if (Walk)
        {
            animator.SetBool("Walking", true);
            animator.SetBool("Chase", false);
        }
        if (Chasse)
        {
            animator.SetBool("Chase", true);
            animator.SetBool("Walking", false);
        }
        if(Life <= 0)
        {
            Dead();
        }
        if (!DetectPlayerScript.thirdPersonController)
        {
            StopCoroutine(Dammage());
        }
        if (BoolAttack)
        {
            StartCoroutine(Dammage());
        }

    }

    private void Chasing()
    {
        Chasse = true;
        //  The enemy is chasing the player
        m_PlayerNear = false;                       //  Set false that hte player is near beacause the enemy already sees the player
        playerLastPosition = Vector3.zero;          //  Reset the player near position

        if (!m_CaughtPlayer)
        {
            Chassing(speedRun);
            navMeshAgent.SetDestination(m_PlayerPosition);          //  set the destination of the enemy to the player location
            Debug.Log("ennemy target");
        }
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)    //  Control if the enemy arrive to the player location
        {
            if (m_WaitTime <= 0 && !m_CaughtPlayer && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 6f)
            {
                //  Check if the enemy is not near to the player, returns to patrol after the wait time delay
                m_IsPatrol = true;
                m_PlayerNear = false;
                Move(speedWalk);
                m_TimeToRotate = timeToRotate;
                m_WaitTime = startWaitTime;
                navMeshAgent.SetDestination(GameManger.waypoints[m_CurrentWaypointIndex].position);

            }
            else
            {
                if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 2.5f)
                {
                    //  Wait if the current position is not the player position
                    Stop();
                }
                m_WaitTime -= Time.deltaTime;
            }
        }
    }

    private void Patroling()
    {
        if (m_PlayerNear)
        {
            //  Check if the enemy detect near the player, so the enemy will move to that position
            if (m_TimeToRotate <= 0)
            {
                Move(speedWalk);
                LookingPlayer(playerLastPosition);
                
            }
            else
            {
                //  The enemy wait for a moment and then go to the last player position
                Stop();

                m_TimeToRotate -= Time.deltaTime;
            }
        }
        else
        {
            m_PlayerNear = false;           //  The player is no near when the enemy is platroling
            playerLastPosition = Vector3.zero;
            navMeshAgent.SetDestination(GameManger.waypoints[m_CurrentWaypointIndex].position);    //  Set the enemy destination to the next waypoint
            Walk = true;
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                //  If the enemy arrives to the waypoint position then wait for a moment and go to the next
                if (m_WaitTime >= 0)
                {
                    NextPoint();
                    Move(speedWalk);
                }
                else if(m_WaitTime <= 0)
                {
                    Debug.Log("Test");
                    Stop();
                    m_WaitTime = startWaitTime;
                }
            }
        }
    }


    public void NextPoint()
    {
        m_CurrentWaypointIndex = (m_CurrentWaypointIndex + Random.Range(0,GameManger.waypoints.Length)) % GameManger.waypoints.Length;
        navMeshAgent.SetDestination(GameManger.waypoints[m_CurrentWaypointIndex].position);
    }

    void Stop()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0;
        _Stop = true;
        Walk = false;
        Chasse = false;
    }

    void Move(float speed)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
        _Stop = false;
        Walk = true;    
        Chasse = false;
    }
    void Chassing(float speed)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
        _Stop = false;
        Walk = false;
        Chasse = true;
    }

    void CaughtPlayer()
    {
        m_CaughtPlayer = true;
    }

    void LookingPlayer(Vector3 player)
    {
        navMeshAgent.SetDestination(player);
        if (Vector3.Distance(transform.position, player) <= 0.3)
        {
            if (m_WaitTime <= 0)
            {
                m_PlayerNear = false;
                Move(speedWalk);
                navMeshAgent.SetDestination(GameManger.waypoints[m_CurrentWaypointIndex].position);
                m_WaitTime = startWaitTime;
                m_TimeToRotate = timeToRotate;
            }
            else
            {
                Stop();
                m_WaitTime -= Time.deltaTime;
            }
        }
    }

    void EnviromentView()
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, AttackRadius, playerMask);   //  Make an overlap sphere around the enemy to detect the playermask in the view radius
        Collider[] playerInRangeAttack = Physics.OverlapSphere(transform.position, AttackRadius, playerMask);

        for (int i = 0; i < playerInRange.Length; i++)
        {
            Transform playerAttack = playerInRangeAttack[i].transform;
            Transform player = playerInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            Vector3 dirToPlayerAttack = (playerAttack.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                float dstToPlayer = Vector3.Distance(transform.position, player.position);          //  Distance of the enmy and the player
                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                {
                    m_playerInRange = true;             //  The player has been seeing by the enemy and then the nemy starts to chasing the player
                    m_IsPatrol = false;                 //  Change the state to chasing the player
                }
                else
                {
                    /*
                     *  If the player is behind a obstacle the player position will not be registered
                     * */
                    m_playerInRange = false;
                }
            }
            if (Vector3.Angle(transform.forward, dirToPlayerAttack) < viewAngle / 2)
            {
                float dstToPlayerAttack = Vector3.Distance(transform.position, playerAttack.position);          //  Distance of the enmy and the player
                if (!Physics.Raycast(transform.position, dirToPlayerAttack, dstToPlayerAttack, obstacleMask))
                {
                    BoolAttack = true;
                    Debug.Log("Attck ok");
                    animator.SetLayerWeight(1, 1f);
                }
            }
            if (Vector3.Distance(transform.position, player.position) > viewRadius)
            {
                /*
                 *  If the player is further than the view radius, then the enemy will no longer keep the player's current position.
                 *  Or the enemy is a safe zone, the enemy will no chase
                 * */
                m_playerInRange = false;                //  Change the sate of chasing
            }
            if (Vector3.Distance(transform.position, player.position) > AttackRadius)
            {
                BoolAttack = false;
                animator.SetLayerWeight(1, 0f);
                Debug.Log("Attck none");
            }
            if (m_playerInRange)
            {
                /*
                 *  If the enemy no longer sees the player, then the enemy will go to the last position that has been registered
                 * */
                m_PlayerPosition = player.transform.position;       //  Save the player's current position if the player is in range of vision
            }
        }
        for (int i = 0; i < playerInRangeAttack.Length; i++)
        {



        }
    }


    private void Dead()
    {
        GameManger.ZombieAlive--;
        animator.SetBool("Dead",true);
        Debug.Log("Dead");
        Destroy(gameObject);
    }

    IEnumerator Dammage()
    {
        yield return new WaitForSeconds(10f);
        DetectPlayerScript.thirdPersonController.life -= dammage;
        yield return new WaitForSeconds(100f);
    }





}
