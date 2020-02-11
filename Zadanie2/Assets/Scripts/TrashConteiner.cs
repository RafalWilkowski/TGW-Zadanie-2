using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashConteiner : MonoBehaviour
{
    private void OnMouseDrag()
    {
        
    }
    private void OnMouseDown()
    {
        TrashSpawner.Instance.Boom(transform.position);
        Destroy(this.gameObject);
    }
}
