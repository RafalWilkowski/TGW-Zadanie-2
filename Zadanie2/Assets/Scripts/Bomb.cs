using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour , IInteractable
{
    private Fuse _fuse;
    private void Start()
    {
        _fuse = transform.GetChild(0).GetComponentInChildren<Fuse>();
        if (_fuse == null) Debug.Log("fusenotfound");
    }
    public void Interact(int touchID)
    {
        _fuse.gameObject.SetActive(false);
    }

    public void Explode()
    {

    }
}
