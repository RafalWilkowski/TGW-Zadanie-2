using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTrigger : MonoBehaviour , IInteractable
{
    [SerializeField]
    private GameObject _exlosionPrefab;
    [SerializeField]
    private float _triggerDelayTime = 2f;

    private CapsuleCollider2D _capsuleCollider2D;

    private void Start()
    {
        _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    private void OnEnable()
    {
        if(_capsuleCollider2D != null) _capsuleCollider2D.enabled = false;
        StartCoroutine(TriggerDelay());
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Gem gem = collision.GetComponent<Gem>();
        if(gem)
        {
            if(_exlosionPrefab != null) Instantiate(_exlosionPrefab, transform.position, Quaternion.identity);
            Bomb bomb = GetComponentInParent<Bomb>();
            DynamitePool.Instance.ReturnToPool(bomb);
        }       
    }

    private IEnumerator TriggerDelay()
    {
        yield return new WaitForSeconds(_triggerDelayTime);
        _capsuleCollider2D.enabled = true;
    }

    public void Interact(int touchID)
    {
        transform.parent.GetComponent<IInteractable>().Interact(touchID);
    }
}
