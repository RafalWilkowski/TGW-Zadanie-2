using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemConteiner : MonoBehaviour , IInteractable
{

    public void Interact(int fingerID)
    {
        GemSpawner.Instance.StoneBreak(transform.position);
        Destroy(this.gameObject);
    }
}
