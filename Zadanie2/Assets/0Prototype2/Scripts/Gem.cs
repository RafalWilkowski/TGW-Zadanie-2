using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    private Rigidbody2D rb2D;
    [SerializeField]
    //private Cona
    Conteiner.ColorType _gemColor;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.transform.parent.name == "ConveroyBelt")
        {
            rb2D.gravityScale = 1f;
        }
    }

    public Conteiner.ColorType GetGemColor()
    {
        return _gemColor;
    }

}
