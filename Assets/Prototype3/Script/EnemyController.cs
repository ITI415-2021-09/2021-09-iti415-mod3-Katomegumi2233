using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

    
    private const int ATTACK_DISTANCE = 4;

    
    private const int RUN_TO_ROLE_DISTANCE = 50;

    
    private const int BlOOD_REDUCE_ATTACK1 = 6;
    private const int BlOOD_REDUCE_ATTACK2 = 8;
    private const int BlOOD_REDUCE_ATTACK3 = 10;

    
    private const int STATE_IDLE = 1;
    private const int STATE_RUN = 2;
    private const int STATE_ATTACK = 3;
    private const int STATE_DEAD = 4;
    private const int UNDER_ATTACK = 5;

    
    private int currentState;
    
    private Animation ani;
    
    public GameObject role;
    
    Vector3 destination;
    UnityEngine.AI.NavMeshAgent agent;

    
    private bool isAttacked = false;
    
    private bool isAttacking = false;

    /*
    public class Common
    {
        
        public const int MOVE_SPEED = 1;
        
        public const int ROTATE_SPEED = 20;
    }
    */

    // Use this for initialization
    void Start () {
        ani = GetComponent<Animation>();
        // Cache agent component and destination
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        destination = agent.destination;
        role = GameObject.Find("FPSController");
    }
	
	// Update is called once per frame
	void Update () {
        checkState();
        checkAttack();
        handlerAction();
    }

    
    private void checkState()
    {
        if (Vector3.Distance(role.transform.position, transform.position) <= RUN_TO_ROLE_DISTANCE
&& Vector3.Distance(role.transform.position, transform.position) > ATTACK_DISTANCE && false == isAttacked)
        {
            currentState = STATE_RUN;
            isAttacking = true;  
        }
        else if (Vector3.Distance(role.transform.position, transform.position) <= ATTACK_DISTANCE && false == isAttacked)
        {
            currentState = STATE_ATTACK;
            //isAttacking = true;  
        }
        else if (Vector3.Distance(role.transform.position, transform.position) > ATTACK_DISTANCE && true == isAttacking && false == isAttacked)
        {
            currentState = STATE_RUN;
        }
        else if (true == isAttacked)
        {
            currentState = UNDER_ATTACK;
        }
        else
        {
            currentState = STATE_IDLE;
        }
    }

    
    private void checkAttack()
    {
        if (true == isAttacking)
        {
            //transform.LookAt(role.transform);
            if (Vector3.Distance(role.transform.position, transform.position) > ATTACK_DISTANCE)
            {
                run();
            }
        }
    }

    
    private void run()
    {
        //ani.Play("run");
        ani.CrossFade("run", 0.1f, PlayMode.StopAll);
        /*
        if (false == ani.isPlaying)
        {
            ani.CrossFade("run", 0.1f, PlayMode.StopAll);
        }
        */
        //transform.LookAt(role.transform);
        //Returns this vector with a magnitude of 1 (Read Only).
        //When normalized, a vector keeps the same direction but its length is 1.0.
        //Vector3 dir = (role.transform.position - transform.position).normalized;
        //transform.Translate(-dir * Common.MOVE_SPEED * Time.deltaTime);
        //Debug.Log("runToYou");
        // Update destination if the target moves one unit
        destination = role.transform.position;
        agent.destination = destination;
    } 

    
      private void handlerAction()
    {
        switch (currentState)
        {
            case STATE_IDLE:
                ani.Play("dance");
                //ani.Play("idle");
                break;
            case STATE_RUN:
                run();
                break;
            case STATE_ATTACK:
                
                if (false == ani.isPlaying)
                {
                    attack();
                }
                break;
            case STATE_DEAD:
                dead();
                break;
            case UNDER_ATTACK:
                    ani.Play("die");
                    //ani.CrossFade("die", 0.1f, PlayMode.StopAll);
                break;
            default:
                Debug.Log("error state = " + currentState);
                break;
        }
    }

    
    private void attack()
    {
        isAttacking = true;
        int attackIndex = Random.Range(1, 4);
        switch (attackIndex)
        {
            case STATE_IDLE:
                ani.Play("attack");
                
                
                role.SendMessage("reduceBlood", BlOOD_REDUCE_ATTACK1);
                break;
            case STATE_RUN:
                ani.Play("attack");
                
                role.SendMessage("reduceBlood", BlOOD_REDUCE_ATTACK2);
                break;
            case STATE_ATTACK:
                ani.Play("attack");
                
                role.SendMessage("reduceBlood", BlOOD_REDUCE_ATTACK3);
                break;
            default:
                Debug.Log("error state = " + currentState);
                break;
        }
    }

    
    public void dead()
    {
        isAttacked = true;
        agent.destination = transform.position;
        isAttacking = false;
        agent.enabled = false;
        float time = 1.6f;
        //ani.Play("die");
        Destroy(gameObject, time);
    }
}