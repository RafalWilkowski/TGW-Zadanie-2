using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour , IInteractable
{
    [SerializeField]
    private ObjectColor _objectColor;

    #region SOUNDS
    private AudioSource _audio;
    [SerializeField]
    private AudioClip[] _liftingAudios;
    [SerializeField]
    private AudioClip _matchSound;
    [SerializeField]
    private AudioClip _wrongMatchSound;
    [SerializeField]
    private AudioClip _neutrallySound;
    private bool _unmatched = false;
    #endregion

    private BoxCollider2D boxCollider2D;
    private Rigidbody2D rb2D;
    private SpriteRenderer _sprite;

    private int _touchID;
    
    // Start is called before the first frame update
    void Awake()
    {
        _sprite = GetComponentInChildren<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        _audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x > 8.5f)
        {
            GemPool.Instance.ReturnToPool(this);
        }
    }
    public void Init(ObjectColor color, Vector3 position, Sprite sprite)
    {
        //TODOchange after gobals
        _sprite.gameObject.SetActive(true);
        _objectColor = color;
        transform.position = position;
        _sprite.sprite = sprite;
    }
   
    public void Interact(int touchID)
    {
        _touchID = touchID;
        // subscribe to touchdetector
        TouchDetector.onFingerMovedDic.Add(_touchID, OnFingerPositionChanged);
        TouchDetector.onFingerReleasedDic.Add(_touchID, OnFingerReleased);
        // play random liffting sounds
        int randomInt = Random.Range(0, _liftingAudios.Length);
        ChangeClipAndPlay(_liftingAudios[randomInt]);
    }

    private void OnFingerPositionChanged(Vector2 position)
    {
        rb2D.position = position;
        rb2D.velocity = Vector2.zero;
        rb2D.freezeRotation = true;
        transform.position = new Vector3(position.x, position.y, transform.position.z);
    }
    private void OnFingerReleased(Vector2 lastPosition)
    {
        OnFingerPositionChanged(lastPosition);
        _unmatched = false;
        if (IsAboveGoodContainer())
        {
            ChangeClipAndPlay(_matchSound);
            //TODO change to some globals
            _sprite.gameObject.SetActive(false);
            StartCoroutine(ReturnToPoolAfterSound());
        }
        else if(_unmatched)
        {
            ChangeClipAndPlay(_wrongMatchSound);
        }
        else
        {
            ChangeClipAndPlay(_neutrallySound);
        }
        
    
        // unsubscribe from touchdetector
        TouchDetector.onFingerMovedDic.Remove(_touchID);
        TouchDetector.onFingerReleasedDic.Remove(_touchID);

    }

    private IEnumerator ReturnToPoolAfterSound()
    {
        while (_audio.isPlaying)
        {
            yield return null;
        }
        GemPool.Instance.ReturnToPool(this);
    }
    private bool IsAboveGoodContainer()
    {
        ContactFilter2D filter = new ContactFilter2D();
        filter.layerMask = gameObject.layer;
        filter.useTriggers = true;
        List<Collider2D> results = new List<Collider2D>();
        boxCollider2D.OverlapCollider(filter,results);

        foreach(Collider2D collider in results)
        {
            Conteiner container = collider.GetComponent<Conteiner>();
            if(container != null)
            {
                if (_objectColor.HasFlag(container.ObjectColor))
                {
                    Conteiner.OnColorMatch?.Invoke(container.ObjectColor);
                    _unmatched = false;
                    return true;
                }
                else
                {
                    _unmatched = true;
                }
                
            }
			SecondaryObjectiveSocket socket = collider.GetComponent<SecondaryObjectiveSocket>();
			if (socket != null)
			{
                if(_objectColor.HasFlag(socket.SocketColor) && !socket.IsFull)
                {
                    socket.InstallGem(this);
                    _unmatched = false;
                    return true;
                }
                else
                {
                    _unmatched = true;
                }
            }                     
        }
        return false;
    }

    private void ChangeClipAndPlay(AudioClip clip)
    {
        _audio.clip = clip;
        _audio.Play();
    }

	public ObjectColor GemColor { get => _objectColor; }
}
