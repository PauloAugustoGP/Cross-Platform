using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int level;
    public int points;

    public int str;
    public int dex;
    public int vit;

    public List<GameObject> items;

    public int maxXP;
    public int xp;
    [SerializeField]private GameObject levelUp;

    public int maxHealth;
    public int health;

    public int damage = 10;
    public int bank = 0;
    public int trophies = 0;

    [SerializeField]private GameObject critDamage;
    [SerializeField]private GameObject normalDamage;

    public bool dead;
    private bool attacking = false;
    private float attackRate = 0;

    public float moveSpeed;
    private float speed_X;
    private float speed_Y;

    private float restTime;

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
        bank = 0;
        maxHealth = vit * 50;
        health = maxHealth;

        maxXP *= level;

        damage = items[0].GetComponent<Item>().damage + str;

        anim = GetComponent<Animator>();
	}
	
	void Update ()
    {
        attackRate += Time.deltaTime;
        if(!attacking && !dead)
        {
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))       currentPosition = (int)directions.Up;
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))     currentPosition = (int)directions.Down;
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))     currentPosition = (int)directions.Left;
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))    currentPosition = (int)directions.Right;

            if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.UpArrow) ||
                Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W))                     currentPosition = (int)directions.UpRight;
            if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.DownArrow) ||
                Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S))                     currentPosition = (int)directions.DownRight;
            if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.UpArrow) ||
                Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W))                     currentPosition = (int)directions.UpLeft;
            if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.DownArrow) ||
                Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S))                     currentPosition = (int)directions.DownLeft;
        }

        if(Input.GetMouseButton(0) && !attacking && !dead)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 20.0f);

            if (hit)
            {
                if (hit.transform.gameObject.tag == "Enemy")
                {
                    attacking = true;
                    Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    if (position.x > transform.position.x)
                    {
                        if (position.y > transform.position.y) currentPosition = (int)directions.Attack_UpRight;
                        else currentPosition = (int)directions.Attack_DownRight;
                    }
                    else
                    {
                        if (position.y > transform.position.y) currentPosition = (int)directions.Attack_UpLeft;
                        else currentPosition = (int)directions.Attack_DownLeft;
                    }
                }
            }
        }

        if (!Input.anyKey && !dead) currentPosition = (int)directions.Idle;

        if (attacking)
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Idle"))
                attacking = false;


        if (currentPosition == 0)
            restTime += Time.deltaTime;
        else
            restTime = 0;

        if(restTime >= 30)
            currentPosition = (int)directions.Rest;

        MovePlayer();
    }

    public void ChangeEquipments( int index )
    {
        if(index >= 0)
        {
            Item tempItem = new Item();
            tempItem.CopyItem(GetComponent<Backpack>().GetItem(index).GetComponent<Item>());

            Sprite tempSprite = new Sprite();
            tempSprite = GetComponent<Backpack>().GetItem(index).GetComponent<Image>().sprite;

            int equip = tempItem.ItemType();

            GetComponent<Backpack>().GetItem(index).GetComponent<Item>().CopyItem(items[equip].GetComponent<Item>());
            GetComponent<Backpack>().GetItem(index).GetComponent<Image>().sprite = items[equip].GetComponent<Image>().sprite;

            items[equip].GetComponent<Item>().CopyItem(tempItem);
            items[equip].GetComponent<Image>().sprite = tempSprite;

            UpdateStats();
        }
    }

    private void MovePlayer()
    {
        anim.SetInteger("state", currentPosition);

        speed_Y = 0f;
        speed_X = 0f;

        switch (currentPosition)
        {
            case 3:
                {
                    speed_Y = -moveSpeed;
                    speed_X = 0f;
                    transform.localScale = new Vector3(2f, 2f, 0f);
                }
                break;

            case 4:
                {
                    speed_Y = -moveSpeed;
                    speed_X = -moveSpeed;
                    transform.localScale = new Vector3(2f, 2f, 0f);
                }
                break;

            case 5:
                {
                    speed_Y = -moveSpeed;
                    speed_X = moveSpeed;
                    transform.localScale = new Vector3(-2f, 2f, 0f);
                }
                break;

            case 6:
                {
                    speed_Y = 0f;
                    speed_X = -moveSpeed;
                    transform.localScale = new Vector3(2f, 2f, 0f);
                }
                break;

            case 7:
                {
                    speed_Y = 0f;
                    speed_X = moveSpeed;
                    transform.localScale = new Vector3(-2f, 2f, 0f);
                }
                break;

            case 8:
                {
                    speed_Y = moveSpeed;
                    speed_X = -moveSpeed;
                    transform.localScale = new Vector3(2f, 2f, 0f);
                }
                break;

            case 9:
                {
                    speed_Y = moveSpeed;
                    speed_X = moveSpeed;
                    transform.localScale = new Vector3(-2f, 2f, 0f);
                }
                break;

            case 10:
                {
                    speed_Y = moveSpeed;
                    speed_X = 0;
                    transform.localScale = new Vector3(2f, 2f, 0f);
                }
                break;

            case 11: transform.localScale = new Vector3(2f, 2f, 0f); break;
            case 12: transform.localScale = new Vector3(-2f, 2f, 0f); break;
            case 13: transform.localScale = new Vector3(2f, 2f, 0f); break;
            case 14: transform.localScale = new Vector3(-2f, 2f, 0f); break;
        }

        transform.position = new Vector3(transform.position.x + speed_X, transform.position.y + speed_Y, 0f);
    }

    public void TakeDamage( int damage, bool crit )
    {
        int status;
        
        if(crit)
        {
            status = ((damage*2) - items[1].GetComponent<Item>().armor - items[2].GetComponent<Item>().armor - items[3].GetComponent<Item>().armor - items[4].GetComponent<Item>().armor);
            if (status < 0) status = 0;

            Instantiate(critDamage, new Vector3(transform.position.x, transform.position.y + 1.5f, 0f), Quaternion.identity).transform.FindChild("Strike").FindChild("Value").GetComponent<Text>().text = "" + status;
        }
        else
        {
            status = (damage - items[1].GetComponent<Item>().armor - items[2].GetComponent<Item>().armor - items[3].GetComponent<Item>().armor - items[4].GetComponent<Item>().armor);
            if(status < 0) status = 0;

            Instantiate(normalDamage, new Vector3(transform.position.x, transform.position.y + 1.5f, 0f), Quaternion.identity).transform.FindChild("Strike").FindChild("Value").GetComponent<Text>().text = "" + status;
        } 

        if (health - status < 0)
        {
            health = 0;
            currentPosition = (int)directions.Die;
            anim.SetInteger("state", currentPosition);
            dead = true;
            GetComponent<BoxCollider2D>().enabled = false;
        }
        else
            health -= status;
    }

    public void AddXP( int killXP )
    {
        xp += killXP;
        if(xp >= maxXP)
        {
            xp -= maxXP;
            level++;
            maxXP *= level;
            points += 3;

            Instantiate(levelUp, transform.position, Quaternion.identity);
        }
    }

    public void UpdateStats()
    {
        
        if( health == maxHealth)
        {
            maxHealth = vit * 50;
            health = maxHealth;
        }
        else
            maxHealth = vit * 50;
        
        damage = items[0].GetComponent<Item>().damage + str;
    }

    private void OnTriggerStay2D(Collider2D c)
    {
        if (c.GetType() == typeof(BoxCollider2D))
        {
            if (c.gameObject.tag == "Enemy")
            {
                if (attacking && attackRate >= 1)
                {
                    if(Random.Range(0, 100) > dex)
                        c.gameObject.GetComponent<Enemy>().TakeDamage(damage, false);
                    else
                        c.gameObject.GetComponent<Enemy>().TakeDamage(damage, true);
                    attackRate = 0;
                }
            }
        }
    }
}
