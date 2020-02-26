using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTrigger : MonoBehaviour , IInteractable
{
    [SerializeField]
    private GameObject _exlosionPrefab;
    [SerializeField]
    private float _triggerDelayTime = 2f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Gem gem = collision.GetComponent<Gem>();
        if(gem && collision.isTrigger)
        {
            if(_exlosionPrefab != null) Instantiate(_exlosionPrefab, transform.position, Quaternion.identity);
            Bomb bomb = GetComponentInParent<Bomb>();
            DynamitePool.Instance.ReturnToPool(bomb);
        }       
    }

    public void Interact(int touchID)
    {
        transform.parent.GetComponent<IInteractable>().Interact(touchID);
    }
}
