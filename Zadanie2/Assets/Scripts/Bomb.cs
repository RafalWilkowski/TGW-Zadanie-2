using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour , IInteractable
{
    private Fuse _fuse;
    private Rigidbody2D _rb2D;
    private bool _firstTouch = true;
    //private int _touchID;

    private void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _fuse = transform.GetChild(0).GetComponentInChildren<Fuse>();
        if (_fuse == null) Debug.Log("fusenotfound");
    }
    public void Interact(int touchID)
    {
        if (_firstTouch)
        {
            _fuse.gameObject.SetActive(false);
            _firstTouch = false;
        }
        else TouchDetector.onSwipeDic.Add(touchID,FlyAway);

    }

    public void Explode()
    {
        
    }

    private void FlyAway()
    {
        _rb2D.velocity = Vector2.up * 5;
    }
}
