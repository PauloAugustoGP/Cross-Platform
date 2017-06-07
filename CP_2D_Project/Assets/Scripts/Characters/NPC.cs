using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    private GameObject toInstantiate;
    private GameObject target;

    [SerializeField]private List<GameObject> buying;
    private List<GameObject> selling;

    public int type = -1;

    private bool openShop = false;
    [SerializeField]private bool special = false;
    
	void Start ()
    {
        target = GameObject.FindWithTag("Player");
        toInstantiate = transform.FindChild("Shop").gameObject;

        if (!special)
        {
            for (int i = 0; i < 5; i++)
            {
                string slot = "Slot" + (i + 1);
                string item = "Item" + (i + 1);

                toInstantiate.transform.FindChild("BuyPanel").FindChild(slot).FindChild(item).GetComponent<Item>().CopyItem(buying[i].GetComponent<Item>());
                toInstantiate.transform.FindChild("BuyPanel").FindChild(slot).FindChild(item).GetComponent<Image>().sprite = buying[i].GetComponent<Image>().sprite;

                toInstantiate.transform.FindChild("BuyPanel").FindChild(slot).FindChild(item + "Value").GetComponent<Text>().text = "" + buying[i].GetComponent<Item>().value;
            }
        }
    }
	
	void Update ()
    {
        toInstantiate.SetActive(openShop);

        if (openShop && !special)
        {
            if (type > -1)
            {
                for (int i = 0; i < 20; i++)
                {
                    string slot = "Slot" + (i + 1);
                    string item = "Item" + (i + 1);

                    toInstantiate.transform.FindChild("SellPanel").FindChild(slot).FindChild(item).gameObject.SetActive(false);
                    toInstantiate.transform.FindChild("SellPanel").FindChild(slot).FindChild(item + "Value").gameObject.SetActive(false);

                    if (target.GetComponent<Backpack>().GetItem(i) != null)
                    {
                        if (target.GetComponent<Backpack>().GetItem(i).GetComponent<Item>().ItemType() == type)
                        {
                            toInstantiate.transform.FindChild("SellPanel").FindChild(slot).FindChild(item).GetComponent<Item>().CopyItem(target.GetComponent<Backpack>().GetItem(i).GetComponent<Item>());
                            toInstantiate.transform.FindChild("SellPanel").FindChild(slot).FindChild(item).GetComponent<Image>().sprite = target.GetComponent<Backpack>().GetItem(i).GetComponent<Image>().sprite;
                            toInstantiate.transform.FindChild("SellPanel").FindChild(slot).FindChild(item).gameObject.SetActive(true);

                            toInstantiate.transform.FindChild("SellPanel").FindChild(slot).FindChild(item + "Value").GetComponent<Text>().text = "" + target.GetComponent<Backpack>().GetItem(i).GetComponent<Item>().sellValue;
                            toInstantiate.transform.FindChild("SellPanel").FindChild(slot).FindChild(item + "Value").gameObject.SetActive(true);
                        }
                    }
                }
            }
        }
        else if(openShop && special)
        {
            toInstantiate.transform.FindChild("Trophy").FindChild("Amount").GetComponent<Text>().text = "" + target.GetComponent<Player>().trophies;
        }
    }

    public void SellItem(GameObject item)
    {
        target.GetComponent<Player>().bank += item.GetComponent<Item>().sellValue;
        target.GetComponent<Backpack>().RemoveItem(item.GetComponent<Item>().itemName);
    }

    public void BuyItem(GameObject item)
    {
        if(target.GetComponent<Player>().bank > item.GetComponent<Item>().value)
        {
            target.GetComponent<Player>().bank -= item.GetComponent<Item>().value;
            target.GetComponent<Backpack>().AddItem(item);
        }
    }

    public void ChangeTrophy()
    {
        if(target.GetComponent<Player>().trophies > 0)
        {
            target.GetComponent<Player>().trophies--;
            target.GetComponent<Player>().points++;
        }
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if(c.gameObject.tag == "Player")
            openShop = true;
    }

    void OnTriggerExit2D(Collider2D c)
    {
        if (c.gameObject.tag == "Player")
            openShop = false;
    }
}
