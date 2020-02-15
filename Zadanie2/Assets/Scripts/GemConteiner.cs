using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemConteiner : MonoBehaviour , IInteractable
{

    public void Interact()
    {
        GemSpawner.Instance.Boom(transform.position);
        Destroy(this.gameObject);
    }
}
