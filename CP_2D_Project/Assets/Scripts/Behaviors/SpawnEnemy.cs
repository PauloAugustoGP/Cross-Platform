using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public List<GameObject> enemies;
    public List<int> enemiesAmount;

    public int id;
    public bool staticCharacters;
    
    void Start()
    {
        if(staticCharacters)
        {
            for (int i = 0; i < enemies.Count; i++)
                Instantiate(enemies[i]);
        }
    }
	
	void Update ()
    {
        if(!staticCharacters)
        {
            for (int i = 0; i < enemiesAmount.Count; i++)
                if (enemiesAmount[i] > 0)
                    DeployEnemy(i);
        }
    }

    private void DeployEnemy(int index)
    {
        GameObject temp = Instantiate(enemies[index], transform.position, Quaternion.identity);
        temp.GetComponent<Enemy>().initialPosition = new Vector3(transform.position.x + Random.Range(-10f, 10f),
                                                                 transform.position.y + Random.Range(-10f, 10f),
                                                                 0f);
        temp.GetComponent<Enemy>().id = id;
        temp.GetComponent<Enemy>().type = index;
        enemiesAmount[index]--;
    }
}
