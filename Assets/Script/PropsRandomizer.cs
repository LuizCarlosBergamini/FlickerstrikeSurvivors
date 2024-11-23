using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsRandomizer : MonoBehaviour
{
    public List<GameObject> propsSpawnPoints;
    public List<GameObject> propsPrefabs;


    // Start is called before the first frame update
    void Start()
    {
        SpawnProps();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnProps()
    {
        foreach (GameObject sp in propsSpawnPoints)
        {
            int rand = Random.Range(0, propsPrefabs.Count);
            GameObject prop = Instantiate(propsPrefabs[rand], sp.transform.position, Quaternion.identity);
            prop.transform.parent = sp.transform;
        }
    }
}
