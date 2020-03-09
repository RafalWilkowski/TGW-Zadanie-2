using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class Gem : MonoBehaviour , IInteractable
{
    [SerializeField]
    private ObjectColor _objectColor;

	[SerializeField]
	ParticleSystem tapParticles, holdParticles;
    #region BACKONBELT
    [Header("Back On Belt Variables:")]
    [SerializeField]
    private float _lerpDuration = 0.25f;
    [SerializeField]
    private float _minXPosition = 0;
    [SerializeField]
    private float _maxXPosition = 0;
    [SerializeField]
    private float _minYPosition = 0;
    [SerializeField]
    private float _maxYPosition = 0;
    [SerializeField]
    private bool _randomizeX = false;
    [SerializeField]
    private bool _randomizeY = false;

    private bool _backOnBelt = false;
    private Vector2 _startPosition;
    private Vector2 _targetPosition;
    private float _lerpElapsedTime;
    #endregion

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

    private CircleCollider2D _circleCollider2D;
    private Rigidbody2D rb2D;
    private SpriteRenderer _sprite;

    private int _touchID;
    
    // Start is called before the first frame update
    void Awake()
    {
        _sprite = GetComponentInChildren<SpriteRenderer>();
        _circleCollider2D = GetComponent<CircleCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        _audio = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update()
    {
        if(_backOnBelt)
        {
            _lerpElapsedTime += Time.deltaTime;
            float t = _lerpElapsedTime / _lerpDuration;
            transform.position = Vector2.Lerp(_startPosition, _targetPosition, t);
            if(t >= 1)
            {
                _lerpElapsedTime = 0;
                _backOnBelt = false;
                _circleCollider2D.isTrigger = false;
            }
        }
    }
    public void Init(ObjectColor color, Vector3 position, Sprite sprite)
    {
        //TODOchange after gobals
        _sprite.gameObject.SetActive(true);
        _objectColor = color;
        float randX = Random.Range(-0.25f, 0.25f);
        transform.position = new Vector3(position.x + randX, position.y,position.z);
        //add random rotation
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, Random.Range(0, 360));
        _sprite.sprite = sprite;
        Light2D light = GetComponentInChildren<Light2D>();
        if (!light)
        {
            Debug.Log("rbrak light");
        }
        else
        {
            switch (color)
            {
                case ObjectColor.BLUE:
                    light.color = Color.blue;
                    break;
                case ObjectColor.RED:
                    light.color = Color.red;
                    break;
                case ObjectColor.YELLOW:
                    light.color = Color.yellow;
                    break;
                case ObjectColor.GREEN:
                    light.color = Color.green;
                    break;
            }
        }
        
        _circleCollider2D.enabled = true;

		if (tapParticles)
		{
			ParticleSystem.MainModule main = tapParticles.main;
			main.loop = false;
			main.playOnAwake = false;
			main.startColor = Color.Lerp(_objectColor.GetColor(), Color.white, 0.6f);
			tapParticles.Stop();
		}
		if (holdParticles)
		{
			ParticleSystem.MainModule main = holdParticles.main;
			main.loop = true;
			main.playOnAwake = false;
			Color col = _objectColor.GetColor();
			main.startColor = Color.Lerp(_objectColor.GetColor(), Color.white, 0.6f);
			holdParticles.Stop();
		}
	}
   
    public void Interact(int touchID)
    {
        _touchID = touchID;
        _circleCollider2D.isTrigger = true;
        // subscribe to touchdetector
        TouchDetector.Instance.onFingerMovedDic.Add(_touchID, OnFingerPositionChanged);
        TouchDetector.Instance.onFingerReleasedDic.Add(_touchID, OnFingerReleased);
        // play random liffting sounds
        int randomInt = Random.Range(0, _liftingAudios.Length);
        ChangeClipAndPlay(_liftingAudios[randomInt]);
		
		if (holdParticles) holdParticles.Play();
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
            _circleCollider2D.enabled = false;
            StartCoroutine(ReturnToPoolAfterSound());
        }
        else
        {
            if (_unmatched)
            {
                ChangeClipAndPlay(_wrongMatchSound);
            }
            else
            {
                ChangeClipAndPlay(_neutrallySound);
            }
            // back on belt
            _backOnBelt = true;
            float posY;
            float posX;
            if (!_randomizeY)
            {
                posY = Mathf.Clamp(transform.position.y, _minYPosition, _maxYPosition);
            }
            else
            {
                posY = Random.Range(_minYPosition, _maxYPosition);
            }
            if (!_randomizeX)
            {
                posX = Mathf.Clamp(transform.position.x, _minXPosition, _maxXPosition);
            }
            else
            {
                posX = Random.Range(_minXPosition, _maxXPosition);
            }

            _targetPosition = new Vector2(posX, posY);
            _startPosition = transform.position;
        }

        if (!_backOnBelt)
        {
            _circleCollider2D.isTrigger = false;
        }
           
        
        // unsubscribe from touchdetector
        TouchDetector.Instance.onFingerMovedDic.Remove(_touchID);
        TouchDetector.Instance.onFingerReleasedDic.Remove(_touchID);
		if (holdParticles) holdParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);

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
        _circleCollider2D.OverlapCollider(filter,results);

        foreach(Collider2D collider in results)
        {
            Conteiner container = collider.GetComponent<Conteiner>();
            if(container != null)
            {
                if (_objectColor.HasFlag(container.ObjectColor))
                {
                    Conteiner.OnColorMatch?.Invoke(container.ObjectColor);
					if (tapParticles) tapParticles.Play();
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
					if (tapParticles) tapParticles.Play();
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
