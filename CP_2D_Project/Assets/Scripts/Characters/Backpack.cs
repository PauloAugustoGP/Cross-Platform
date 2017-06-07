using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Backpack : MonoBehaviour
{
    private static int size = 20;
    private List<GameObject> items;

	void Start ()
    {
        items = new List<GameObject>();

        for (int i = 0; i < size; i++)
            items.Add(null);
	}
	
    public GameObject GetItem( int index )
    {
        return items[index];
    }

    public void AddItem( GameObject item )
    {
        GameObject temp = new GameObject();
        temp.AddComponent<Image>().sprite = item.GetComponent<Image>().sprite;

        temp.AddComponent<Item>().CopyItem(item.GetComponent<Item>());

        for (int i = 0; i < size; i++)
        {
            if (items[i] == null)
            {
                items.RemoveAt(i);
                items.Insert(i, Instantiate(temp) );
                break;
            }
        }

        Destroy(temp);
    }

    public void RemoveItem( string itemName )
    {
        int index = -1;
        
        for (int i = 0; i < size; i++)
        {
            if (items[i].GetComponent<Item>().itemName == itemName)
            {
                index = i;
                break;
            }
        }

        if(index != -1)
        {
            Destroy(items[index]);
            items.RemoveAt(index);
        }
            
    }
}
