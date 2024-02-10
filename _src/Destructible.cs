using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField]
    private float destructionTime = 1f;

    [SerializeField][Range(0.0f, 1.0f)]
    private float itemSpawnChance = 0.2f;

    public GameObject[] spawnableItems;

    private void Start()
    {
        Destroy(gameObject, destructionTime);
    }

    private void OnDestroy()
    {
        if(spawnableItems.Length > 0 && Random.value < itemSpawnChance)
        {
            int randomIndex = Random.Range(0, spawnableItems.Length);
            Instantiate(spawnableItems[randomIndex], transform.position, Quaternion.identity);
        }
    }
}
