using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private enum ItemType
    {
        ExtraBomb,
        BlastRadius,
        SpeedIncrease,
    }

    [SerializeField]
    private ItemType type;

    private void OnItemPickup(GameObject player)
    {
        switch (type)
        {
            case ItemType.ExtraBomb:
                player.GetComponent<BombController>().AddBomb();
                break;
            case ItemType.BlastRadius:
                player.GetComponent<BombController>().AddExplosionRadius();
                break;
            case ItemType.SpeedIncrease:
                player.GetComponent<MovementController>().AddSpeed();
                break;
            default:
                break;
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player"))
        {
            OnItemPickup(collider.gameObject);
        }
    }
}
