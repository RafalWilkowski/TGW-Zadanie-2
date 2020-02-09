using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conteiner : MonoBehaviour
{
    public enum ColorType { RED, YELLOW , BLUE, GREEN, NONE}
    [SerializeField]
    private ColorType _conteinerColor;

    private Score score;
	public ColorType ContainerColor { get => _conteinerColor; }

    private void Start()
    {
        score = FindObjectOfType<Score>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Gem gem = collision.GetComponentInParent<Gem>();
        if(gem != null)
        {
            if(_conteinerColor == gem.GetGemColor())
            {
                score.AddScore();
				PrototypeManager.Instance.CheckForCombo(gem);
            }
            else
            {
                StartCoroutine(PrototypeManager.Instance.ResetGame());            
            }
            Destroy(gem.gameObject);
        }
        else
        {
            Debug.Log("Brak gema");
        }
        
    }

    private void OnMouseDown()
    {
        PrototypeManager.Instance.CurrentConteiner(_conteinerColor);
    }
}
