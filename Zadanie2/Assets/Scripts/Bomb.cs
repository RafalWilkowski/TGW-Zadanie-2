using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour , IInteractable
{
    private Fuse _fuse;
    private BombTrigger _bombTrigger;

    private void OnEnable()
    {
        if(_fuse != null) _fuse.gameObject.SetActive(true);
        if (_bombTrigger != null) _bombTrigger.gameObject.SetActive(true);
    }
    private void Start()
    {
        _fuse = transform.GetChild(0).GetComponentInChildren<Fuse>();
        if (_fuse == null) Debug.Log("fusenotfound");
        _bombTrigger = GetComponentInChildren<BombTrigger>();
        
    }
    public void Interact(int touchID)
    {
        if (_fuse.isActiveAndEnabled)
        {
            _fuse.gameObject.SetActive(false);
            _bombTrigger.gameObject.SetActive(false);
        }       
    }

}
