using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BombController : MonoBehaviour
{
    [Header("Bomb")]
    [SerializeField]
    private GameObject bombPrefab;

    [SerializeField]
    private KeyCode inputKey = KeyCode.Space;

    [SerializeField]
    private float bombFuseTime = 3.0f;

    [SerializeField]
    private int bombAmount = 1;

    [SerializeField]
    private int bombsRemaining = 0;

    [Header("Explosion")]
    [SerializeField]
    private Explosion explosionPrefab;

    [SerializeField]
    private LayerMask explosionLayerMask;

    [SerializeField]
    private float explosionDuration = 1.0f;

    [SerializeField]
    private int explosionRadius = 1;

    [Header("Destructible")]
    [SerializeField]
    private Tilemap destructibleTiles;
    [SerializeField]
    private Destructible destructiblePrefab;

    private void OnEnable() 
    {
        bombsRemaining = bombAmount;
    }

    private void Update() 
    {
        if (bombsRemaining > 0 && Input.GetKeyDown(inputKey))
        {
            StartCoroutine(PlaceBomb());
        }    
    }

    private IEnumerator PlaceBomb()
    {
        Vector2 position = transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        GameObject bomb = Instantiate(bombPrefab, position, Quaternion.identity);
        bombsRemaining--;

        yield return new WaitForSeconds(bombFuseTime);

        position = bomb.transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRenderer(explosion.start);
        explosion.DestroyAfter(explosionDuration);

        Explode(position, Vector2.up, explosionRadius);
        Explode(position, Vector2.down, explosionRadius);
        Explode(position, Vector2.left, explosionRadius);
        Explode(position, Vector2.right, explosionRadius);

        Destroy(bomb);
        bombsRemaining++;
    }

    private void Explode(Vector2 position, Vector2 direction, int lenght)
    {
        //escape from recursive function
        if (lenght <= 0)
            return;

        position += direction;

        if (Physics2D.OverlapBox(position, Vector2.one / 2f, 0f, explosionLayerMask))
        {   
            ClearDestructible(position);
            return; 
        }

        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        //ternary statement
        explosion.SetActiveRenderer(lenght > 1 ? explosion.middle : explosion.end);
        explosion.SetDirection(direction);
        explosion.DestroyAfter(explosionDuration);

        //recursive function
        Explode(position, direction, lenght - 1);
    }

    private void ClearDestructible(Vector2 position)
    {
        Vector3Int cell = destructibleTiles.WorldToCell(position);
        TileBase tile = destructibleTiles.GetTile(cell);

        if (tile != null)
        {
            Instantiate(destructiblePrefab, position, Quaternion.identity);
            destructibleTiles.SetTile(cell, null);
        }
    }

    public void AddBomb()
    {
        bombAmount++;
        bombsRemaining++;
    }

    public void AddExplosionRadius()
    {
        explosionRadius++;
    }

    private void OnTriggerExit2D(Collider2D collider) 
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Bomb"))
            collider.isTrigger = false;
    }
}
