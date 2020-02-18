using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemConteiner : MonoBehaviour , IInteractable
{
    public void Interact(int fingerID)
    {
        //TODO change to delegate
        GemSpawner.Instance.StoneBreak(transform.position);
        StonePool.Instance.ReturnToPool(this);
    }
}
