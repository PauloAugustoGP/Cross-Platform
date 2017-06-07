using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int id;
    public int type;

    public float maxHealth;
    public float health;

    [SerializeField]private int killXP;

    public bool dead = false;
    private bool attacking = false;
    private float attackRate = 0;

    [SerializeField]private int damage = 5;
    private int crit = 5;
    private float speed = 0.03f;

    [SerializeField]private GameObject critDamage;
    [SerializeField]private GameObject normalDamage;

    private GameObject target;
    private bool chasing = false;
    private float distance;

    public Vector3 initialPosition;
    private Vector3 randDestination;
    
    public enum stateBehavior : int
    {
        Walking,
        Chase,
        Attack,
        Return,
        Dead
    };
    private int enemyState;

    private Animator anim;

    public enum directions : int
    {
        Idle,
        Rest,
        Die,
        Down,
        DownLeft,
        DownRight,
        Left,
        Right,
        UpLeft,
        UpRight,
        Up,
        Attack_DownLeft,
        Attack_DownRight,
        Attack_UpLeft,
        Attack_UpRight
    };
    public int currentPosition;
    
    void Start ()
    {
        health = maxHealth;
        anim = GetComponent<Animator>();

        target = GameObject.FindWithTag("Player");

        enemyState = (int)stateBehavior.Return;
	}
	
	void Update ()
    {
        if(transform.position.x > target.transform.position.x + 12 ||
           transform.position.x < target.transform.position.x - 12 ||
           transform.position.y > target.transform.position.y + 8 ||
           transform.position.y < target.transform.position.y - 8)
        {
            GetComponent<Renderer>().enabled = false;
        }
        else
        {
            GetComponent<Renderer>().enabled = true;
        }

        if(health <= 0 && !dead)
        {
            transform.localScale = new Vector3(2.5f, 2.5f, 1f);
            enemyState = (int)stateBehavior.Dead;
            currentPosition = (int)directions.Die;
            
            dead = true;
            attacking = false;
            chasing = false;

            GetComponent<CircleCollider2D>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;

            target.GetComponent<Player>().AddXP(killXP);

            Destroy( gameObject.transform.FindChild("Canvas").gameObject );

            transform.FindChild("LootHandler").gameObject.SetActive(true);

            Invoke("DestroyObject", 60f);
        }

        attackRate += Time.deltaTime;
        if(attacking)
        {
            if (enemyState != (int)stateBehavior.Attack)
            {
                if (distance <= 2f && attackRate >= 1)
                {
                    if (Random.Range(0, 100) > crit)
                        target.GetComponent<Player>().TakeDamage(damage, false);
                    else
                        target.GetComponent<Player>().TakeDamage(damage, true);
                    attackRate = 0;
                }
                attacking = false;
            }
            else
                if(attackRate >= 1)
                {
                    if (Random.Range(0, 100) > crit)
                        target.GetComponent<Player>().TakeDamage(damage, false);
                    else
                        target.GetComponent<Player>().TakeDamage(damage, true);
                    attackRate = 0;
                }
        }

        if (chasing)
        {
            distance = Vector3.Distance(transform.position, target.transform.position);
            randDestination = target.transform.position;

            if (distance >= 9f)
                enemyState = (int)stateBehavior.Return;
            else if (distance <= 1.5f)
                enemyState = (int)stateBehavior.Attack;
            else
                enemyState = (int)stateBehavior.Walking;
        }
            

        switch(enemyState)
        {
            case 0:
                {
                    if (transform.position == randDestination)
                    {
                        randDestination = GenerateDestination();
                        currentPosition = (int)directions.Idle;
                    }
                    else
                    {
                        if(randDestination.x > transform.position.x)
                        {
                            transform.localScale = new Vector3(-2.5f, 2.5f, 1f);

                            if (randDestination.y > transform.position.y)           currentPosition = (int)directions.UpRight;
                            else if (randDestination.y == transform.position.y)     currentPosition = (int)directions.Right;
                            else                                                    currentPosition = (int)directions.DownRight;
                        }
                        else if(randDestination.x == transform.position.x)
                        {
                            if (randDestination.y > transform.position.y)           currentPosition = (int)directions.Up;
                            else if (randDestination.y == transform.position.y)     currentPosition = (int)directions.Idle;
                            else                                                    currentPosition = (int)directions.DownRight;
                        }
                        else
                        {
                            transform.localScale = new Vector3(2.5f, 2.5f, 1f);

                            if (randDestination.y > transform.position.y)           currentPosition = (int)directions.UpLeft;
                            else if (randDestination.y == transform.position.y)     currentPosition = (int)directions.Left;
                            else                                                    currentPosition = (int)directions.DownLeft;
                        }
                            
                        transform.position = Vector3.MoveTowards(transform.position, randDestination, speed);
                    }

                    anim.SetInteger("state", currentPosition);
                }break;

            case 1:
                {
                    chasing = true;
                    randDestination = target.transform.position;
                    enemyState = (int)stateBehavior.Walking;
                } break;
                
            case 2:
                {
                    attacking = true;
                    if (target.transform.position.x > transform.position.x)
                    {
                        transform.localScale = new Vector3(-2.5f, 2.5f, 1f);

                        if (target.transform.position.y > transform.position.y)         currentPosition = (int)directions.Attack_UpRight;
                        else if (target.transform.position.y == transform.position.y)   currentPosition = (int)directions.Attack_DownRight;
                        else                                                            currentPosition = (int)directions.Attack_DownRight;
                    }
                    else if (target.transform.position.x == transform.position.x)
                    {
                        if (target.transform.position.y > transform.position.y)         currentPosition = (int)directions.Attack_UpRight;
                        else if (target.transform.position.y == transform.position.y)   currentPosition = (int)directions.Attack_DownRight;
                        else                                                            currentPosition = (int)directions.Attack_DownLeft;
                    }
                    else
                    {
                        transform.localScale = new Vector3(2.5f, 2.5f, 1f);

                        if (target.transform.position.y > transform.position.y)         currentPosition = (int)directions.Attack_UpLeft;
                        else if (target.transform.position.y == transform.position.y)   currentPosition = (int)directions.Attack_DownLeft;
                        else                                                            currentPosition = (int)directions.Attack_DownLeft;
                    }

                    anim.SetInteger("state", currentPosition);
                } break;

            case 3:
                {
                    GetComponent<BoxCollider2D>().enabled = false;
                    chasing = false;
                    randDestination = initialPosition;
                    enemyState = (int)stateBehavior.Walking;
                } break;

            case 4:
                {
                    anim.SetInteger("state", currentPosition);
                }
                break;
        }
	}

    private Vector3 GenerateDestination()
    {
        return new Vector3(initialPosition.x + Random.Range(-1, 1), initialPosition.y + Random.Range(-1, 1), 0f);
    }

    public void TakeDamage(int damage, bool crit)
    {
        if (crit)
        {
            damage *= 2;
            Instantiate(critDamage, new Vector3(transform.position.x, transform.position.y + 1.5f, 0f), Quaternion.identity).transform.FindChild("Strike").FindChild("Value").GetComponent<Text>().text = "" + damage;
        } 
        else
        {
            Instantiate(normalDamage, new Vector3(transform.position.x, transform.position.y + 1.5f, 0f), Quaternion.identity).transform.FindChild("Strike").FindChild("Value").GetComponent<Text>().text = "" + damage;
        }

        if (health - damage < 0)
            health = 0;
        else
            health -= damage;
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "Player" && !dead)
        {
            enemyState = (int)stateBehavior.Chase;
            GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    private void DestroyObject()
    {
        GameObject.Find("SpawnHandler(Clone)").transform.FindChild("Spawn" + id + "(Clone)").GetComponent<SpawnEnemy>().enemiesAmount[type]++;
        Destroy(gameObject);
    }
}
