using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowLoot : MonoBehaviour
{
    private GameObject window;

    [SerializeField]private List<GameObject> loot;
    [SerializeField]private List<int> lootAmount;
    [SerializeField]private List<int> lootRate;

    private List<GameObject> drop;
    private List<int> dropAmount;

    private GameObject target;
    private float targetDistance = 0;

	void Start ()
    {
        drop = new List<GameObject>();
        dropAmount = new List<int>();

        window = transform.FindChild("LootWindow").gameObject;
        window.SetActive(false);
        
        for(int i = 0; i < 4; i++)
        {
            if(Random.Range(0, 100) < lootRate[i])
            {
                drop.Add(loot[i]);
                dropAmount.Add(Random.Range(1, lootAmount[i]+1));
            }
        }

        target = GameObject.FindWithTag("Player");
	}
	
	void Update ()
    {
        targetDistance = Vector3.Distance(transform.position, target.transform.position);
        
        if(targetDistance >= 4f)
        {
            targetDistance = 0;
            window.SetActive(false);
        }
	}

    void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(1))
            SetupWindow();
    }

    void SetupWindow()
    {
        window.SetActive(true);

        string name;
        for(int i = 0; i < drop.Count; i++)
        {
            name = "Item" + (i + 1);
            window.transform.FindChild(name).FindChild("loot").gameObject.SetActive(true);
            window.transform.FindChild(name).FindChild("loot").GetComponent<Image>().sprite = drop[i].GetComponent<Image>().sprite;

            window.transform.FindChild(name).FindChild("loot").GetComponent<Item>().CopyItem(drop[i].GetComponent<Item>());

            window.transform.FindChild(name).FindChild("loot").FindChild("Amount").GetComponent<Text>().text = "" + dropAmount[i];
        }
    }

    public void AddItemPlayer(GameObject item)
    {
        string type = item.GetComponent<Item>().itemName;
        
        if (type == "Gold Coins")
        {
            target.GetComponent<Player>().bank += int.Parse(item.transform.FindChild("Amount").GetComponent<Text>().text);
        }
        else if(type == "Soul Trophy")
        {
            target.GetComponent<Player>().trophies++;
        }
        else
        {
            GameObject.FindWithTag("Player").GetComponent<Backpack>().AddItem(item);
        }

        string name;
        int index = 0;
        
        for (int i = 0; i < drop.Count; i++)
        {
            name = "Item" + (i + 1);
            window.transform.FindChild(name).FindChild("loot").gameObject.SetActive(false);
            
            if (drop[i].GetComponent<Image>().sprite.name == type)
                index = i;
        }
        
        drop.RemoveAt(index);
        dropAmount.RemoveAt(index);

        SetupWindow();
    }
}
