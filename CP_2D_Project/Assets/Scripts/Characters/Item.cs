using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public string itemName;
    public int damage;
    public int armor;
    public string description;
    public int value;
    public int sellValue;

    public bool isWeapon;
    public bool isHelmet;
    public bool isArmor;
    public bool isLegs;
    public bool isBoots;
    public bool isShield;

    private float time = 0;
    private bool des = false;
    private bool done = false;
    [SerializeField]private GameObject showDescription;
    private GameObject toInstantiate;

    public int ItemType()
    {
        if (isWeapon) return 0;
        if (isHelmet) return 1;
        if (isArmor) return 2;
        if (isLegs) return 3;
        if (isBoots) return 4;
        if (isShield) return 5;

        return -1;
    }

    public void CopyItem( Item item )
    {
        itemName = item.itemName;
        damage = item.damage;
        armor = item.armor;
        description = item.description;
        value = item.value;
        sellValue = item.sellValue;

        isWeapon = item.isWeapon;
        isHelmet = item.isHelmet;
        isArmor = item.isArmor;
        isLegs = item.isLegs;
        isBoots = item.isBoots;
        isShield = item.isShield;
    }

    void Update()
    {
        if(des) time += Time.deltaTime;
        else time = 0;

        if (time >= 5f && !done)
        {
            toInstantiate = Instantiate(showDescription, transform.position, Quaternion.identity);
            toInstantiate.transform.FindChild("Description").FindChild("ItemDescription").GetComponent<Text>().text = "" + itemName + "\n" +
                                                                                                                      "DAMAGE: " + damage + "\n" +
                                                                                                                      "ARMOR: " + armor + "\n" +
                                                                                                                      description; ;
            done = true;
        }
            
    }

    void OnMouseEnter()
    {
        des = true;
    }

    void OnMouseExit()
    {
        Destroy(toInstantiate);
        des = false;
        done = false;
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Destroy(toInstantiate);
            des = false;
            done = false;
        } 
    }
}
