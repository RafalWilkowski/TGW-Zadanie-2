using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour , IInteractable
{
    [SerializeField]
    private GemSpawner.ObjectColor objectColor;

    private BoxCollider2D boxCollider2D;
    private Rigidbody2D rb2D;

    private int _touchID;
    private bool onGoodContainer = false;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x > 8.5f)
        {
            TouchDetector.onFingerMovedDic.Remove(_touchID);
            TouchDetector.onFingerReleasedDic.Remove(_touchID);
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Conteiner conteiner = collision.gameObject.GetComponent<Conteiner>();
        if (conteiner != null)
        {
            GemSpawner.ObjectColor conteinerColor = conteiner.GetObjectColor();
            if (objectColor == conteinerColor)
            {
                onGoodContainer = true;
                print("match colors!");
            }
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Conteiner conteiner = collision.gameObject.GetComponent<Conteiner>();
        if (conteiner != null)
        {
            GemSpawner.ObjectColor conteinerColor = conteiner.GetObjectColor();
            if (objectColor == conteinerColor)
            {
                onGoodContainer = false;
                print("wyjscie!");
            }
        }
    }

   /* private void OnMouseDrag()
    {
        //boxCollider2D.enabled = false;
        //rb2D.velocity = Vector2.zero;
        float x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        float y = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
        transform.position = new Vector3(x, y, transform.position.z);
    }

    private void OnMouseUpAsButton()
    {
        if (onGoodContainer)
        {
            UIManager.Instance.AddScore();
            Destroy(this.gameObject);
        }
        else
        {

        }
    }
    */
    public void Interact(int touchID)
    {
        _touchID = touchID;
        // subscribe to touchdetector
        TouchDetector.onFingerMovedDic.Add(_touchID, OnFingerPositionChanged);
        TouchDetector.onFingerReleasedDic.Add(_touchID, OnFingerReleased);
    }

    private void OnFingerPositionChanged(Vector2 position)
    {
        rb2D.velocity = Vector2.zero;
        rb2D.freezeRotation = true;
        transform.position = new Vector3(position.x, position.y, transform.position.z);
    }
    private void OnFingerReleased(Vector2 lastPosition)
    {
        OnFingerPositionChanged(lastPosition);

        if (onGoodContainer)
        {
            UIManager.Instance.AddScore();
            Destroy(this.gameObject);
        }

        // unsubscribe from touchdetector
        TouchDetector.onFingerMovedDic.Remove(_touchID);
        TouchDetector.onFingerReleasedDic.Remove(_touchID);
        /*
        TouchDetector.onFingerMoved -= OnFingerPositionChanged;
        TouchDetector.onFingerReleased -= OnFingerReleased;
        */
    }

}
