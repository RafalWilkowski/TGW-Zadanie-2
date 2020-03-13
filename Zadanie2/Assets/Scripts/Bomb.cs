using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour , IInteractable
{
    private Fuse _fuse;
    private BombTrigger _bombTrigger;
    [SerializeField]
    private AudioClip[] _puttingOutFuse;
    private AudioSource _audio;

	public delegate void BombDelegate(Bomb bomb);
	public event BombDelegate OnBombDisabled;

    private void OnEnable()
    {
        if(_fuse != null) _fuse.gameObject.SetActive(true);
        if (_bombTrigger != null) _bombTrigger.gameObject.SetActive(true);
    }
    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }
    private void Start()
    {
        _fuse = transform.GetChild(0).GetComponentInChildren<Fuse>();
        _bombTrigger = GetComponentInChildren<BombTrigger>();       
    }

    public void Interact(int touchID)
    {
        if (_fuse.isActiveAndEnabled)
        {
            _fuse.gameObject.SetActive(false);
            _bombTrigger.gameObject.SetActive(false);
            int randomClip = Random.Range(0, _puttingOutFuse.Length);
            _audio.PlayOneShot(_puttingOutFuse[randomClip]);
        }       
    }

	private void OnDisable()
	{
		OnBombDisabled?.Invoke(this);
	}

}
