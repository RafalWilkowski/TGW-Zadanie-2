using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject _exlosionPrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Gem gem = collision.GetComponent<Gem>();
        if(gem)
        {
            if(_exlosionPrefab != null) Instantiate(_exlosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject.transform.parent.gameObject);
        }       
    }
}
