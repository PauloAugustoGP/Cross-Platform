using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameObject player;

    private GameObject playerLevel;
    private GameObject playerPoints;

    private GameObject playerSTR;
    private GameObject playerDEX;
    private GameObject playerVIT;

    private GameObject playerHP;
    private GameObject playerXP;

    private List<GameObject> playerItems;

    private GameObject playerBank;
    private GameObject playerTrophy;

    private List<GameObject> playerBackpack;

    private string description;
    private float descriptionTime = 0;
    private bool showitem = false;

    private float fadeRate = 0;
    private bool gameVictory = false;

    void Start ()
    {
        player = GameObject.FindWithTag("Player");

        playerLevel = transform.FindChild("LeftPanel").FindChild("Info").FindChild("Level").gameObject;
        playerPoints = transform.FindChild("LeftPanel").FindChild("Info").FindChild("Points").gameObject;

        playerSTR = transform.FindChild("LeftPanel").FindChild("Stats").FindChild("Str").gameObject;
        playerDEX = transform.FindChild("LeftPanel").FindChild("Stats").FindChild("Dex").gameObject;
        playerVIT = transform.FindChild("LeftPanel").FindChild("Stats").FindChild("Vit").gameObject;

        playerHP = transform.FindChild("HealthBar").gameObject;
        playerXP = transform.FindChild("XPBar").gameObject;

        playerItems = new List<GameObject>();
        playerItems.Add(transform.FindChild("RightPanel").FindChild("Equipments").FindChild("Weapon").FindChild("Equip").gameObject);
        playerItems.Add(transform.FindChild("RightPanel").FindChild("Equipments").FindChild("Helm").FindChild("Equip").gameObject);
        playerItems.Add(transform.FindChild("RightPanel").FindChild("Equipments").FindChild("Chest").FindChild("Equip").gameObject);
        playerItems.Add(transform.FindChild("RightPanel").FindChild("Equipments").FindChild("Legs").FindChild("Equip").gameObject);
        playerItems.Add(transform.FindChild("RightPanel").FindChild("Equipments").FindChild("Boots").FindChild("Equip").gameObject);
        playerItems.Add(transform.FindChild("RightPanel").FindChild("Equipments").FindChild("Shield").FindChild("Equip").gameObject);
        
        playerBank = transform.FindChild("RightPanel").FindChild("Bank").FindChild("Text").gameObject;
        playerTrophy = transform.FindChild("RightPanel").FindChild("Trophy").FindChild("Text").gameObject;

        playerBackpack = new List<GameObject>();
        for(int i = 1; i < 21; i++)
        {
            string name = "Slot" + i;
            playerBackpack.Add(transform.FindChild("RightPanel").FindChild("Backpack").FindChild(name).gameObject);
        }
    }
	
	void Update ()
    {
        playerLevel.GetComponent<Text>().text = "Level\n" + player.GetComponent<Player>().level;
        playerPoints.GetComponent<Text>().text = "Points\n" + player.GetComponent<Player>().points;

        playerSTR.GetComponent<Text>().text = "STR = " + player.GetComponent<Player>().str;
        playerDEX.GetComponent<Text>().text = "DEX = " + player.GetComponent<Player>().dex;
        playerVIT.GetComponent<Text>().text = "VIT = " + player.GetComponent<Player>().vit;

        playerHP.GetComponent<Slider>().value = (player.GetComponent<Player>().health / (float)player.GetComponent<Player>().maxHealth) * 100;
        playerHP.transform.FindChild("Text").GetComponent<Text>().text = "Health " + player.GetComponent<Player>().health + "/" + player.GetComponent<Player>().maxHealth;

        playerXP.GetComponent<Slider>().value = (player.GetComponent<Player>().xp / (float)player.GetComponent<Player>().maxXP) * 100;
        playerXP.transform.FindChild("Text").GetComponent<Text>().text = "XP " + player.GetComponent<Player>().xp + "/" + player.GetComponent<Player>().maxXP;

        for(int i = 0; i < 6; i++)
        {
            playerItems[i].GetComponent<Image>().sprite = player.GetComponent<Player>().items[i].GetComponent<Image>().sprite;
            playerItems[i].GetComponent<Item>().damage = player.GetComponent<Player>().items[i].GetComponent<Item>().damage;
            playerItems[i].GetComponent<Item>().armor = player.GetComponent<Player>().items[i].GetComponent<Item>().armor;
            playerItems[i].GetComponent<Item>().description = player.GetComponent<Player>().items[i].GetComponent<Item>().description;
        }

        playerBank.GetComponent<Text>().text = "Bank\n" + player.GetComponent<Player>().bank;
        playerTrophy.GetComponent<Text>().text = "Trophy\n" + player.GetComponent<Player>().trophies;

        for (int j = 0; j < 12; j ++)
        {
            if(player.GetComponent<Backpack>().GetItem(j) == null)
                playerBackpack[j].transform.FindChild("Equip").gameObject.SetActive(false);
            else
            {
                playerBackpack[j].transform.FindChild("Equip").gameObject.SetActive(true);
                playerBackpack[j].transform.FindChild("Equip").GetComponent<Image>().sprite = player.GetComponent<Backpack>().GetItem(j).GetComponent<Image>().sprite;

                playerBackpack[j].transform.FindChild("Equip").GetComponent<Item>().CopyItem(player.GetComponent<Backpack>().GetItem(j).GetComponent<Item>());
            }
        }

        if (showitem)
            descriptionTime += Time.deltaTime;

        if (descriptionTime >= 8f)
        {
            transform.FindChild("Description").gameObject.SetActive(false);
            showitem = false;
        }

        if (player.GetComponent<Player>().dead)
        {
            fadeRate += 0.005f;
            transform.FindChild("BlackScreen").gameObject.SetActive(true);
            transform.FindChild("BlackScreen").FindChild("GameOver").gameObject.SetActive(true);

            if(fadeRate <= 1f)
            {
                transform.FindChild("BlackScreen").GetComponent<Image>().color = new Color(0, 0, 0, fadeRate);
                transform.FindChild("BlackScreen").FindChild("GameOver").GetComponent<Text>().color = new Color(1, 1, 1, fadeRate);
            }
            
            if (fadeRate >= 5f)
                SceneManager.LoadScene("Menu");
        }

        if(gameVictory)
        {
            fadeRate += 0.005f;
            transform.FindChild("BlackScreen").gameObject.SetActive(true);

            if (fadeRate <= 1f)
            {
                transform.FindChild("BlackScreen").GetComponent<Image>().color = new Color(0, 0, 0, fadeRate);
            }

            if (fadeRate >= 2f)
                SceneManager.LoadScene("CutSceneEnd");
        }
    }

    public void GameVictory()
    {
        gameVictory = true;
    }

    public void AddStat( int value )
    {
        if(player.GetComponent<Player>().points > 0)
        {
            player.GetComponent<Player>().points--;
            switch(value)
            {
                case 0:
                    player.GetComponent<Player>().str++;
                    player.GetComponent<Player>().UpdateStats();
                    break;

                case 1:
                    player.GetComponent<Player>().dex++;
                    break;

                case 2:
                    player.GetComponent<Player>().vit++;
                    player.GetComponent<Player>().UpdateStats();
                    break;
            }
        }
    }

    public void ShowPanel( string panel )
    {
        if (panel == "left")
            transform.FindChild("LeftPanel").gameObject.SetActive(true);
        else
            transform.FindChild("RightPanel").gameObject.SetActive(true);
    }

    public void HidePanel( string panel )
    {
        if (panel == "left")
            transform.FindChild("LeftPanel").gameObject.SetActive(false);
        else
            transform.FindChild("RightPanel").gameObject.SetActive(false);
    }

    public void ChangeEquipments( int itemSlot )
    {
        player.GetComponent<Player>().ChangeEquipments(itemSlot);
    }
}
