using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GemConteiner : MonoBehaviour , IInteractable
{
    public Action<Vector3> OnBreak;

    public void Interact(int fingerID)
    {
        //TODO change to delegate
       // GemSpawner.Instance.StoneBreak(transform.position);
        OnBreak?.Invoke(transform.position);
        StonePool.Instance.ReturnToPool(this);
    }
}
