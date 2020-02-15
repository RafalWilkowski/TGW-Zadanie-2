using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    [SerializeField]
    private GemSpawner.ObjectColor objectColor;

    private BoxCollider2D boxCollider2D;
    private Rigidbody2D rb2D;

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
        else
        {
            print("brak ObjectColoru!!!!");
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
        else
        {
            print("brak ObjectColoru!!!!");
        }
    }

    private void OnMouseDrag()
    {
        //boxCollider2D.enabled = false;
        rb2D.velocity = Vector2.zero;
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
}
