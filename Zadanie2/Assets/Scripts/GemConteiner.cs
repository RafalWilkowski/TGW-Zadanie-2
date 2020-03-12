using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GemConteiner : MonoBehaviour , IInteractable
{
    public Action<int,Vector3> OnBreak;
    [SerializeField]
    private int _maxHealth = 2;
    private int _currentHealth;

    private StoneBreakOdds _stoneBreakOdds;
	[SerializeField] Sprite[] sprites;
	SpriteRenderer ownSprite;
	private void OnEnable()
	{
		if (ownSprite)
		{
			int index = UnityEngine.Random.Range(0, sprites.Length);
			ownSprite.sprite = sprites[index];
			Debug.LogFormat("Sprite at {0} set to {1}({2})", ownSprite, index, sprites[index] );
		}
	}
	private void Start()
    {
        _stoneBreakOdds = FindObjectOfType<StoneBreakOdds>();
        _currentHealth = _maxHealth;
		ownSprite = GetComponentInChildren<SpriteRenderer>();
		if (ownSprite)
		{
			int index = UnityEngine.Random.Range(0, sprites.Length);
			ownSprite.sprite = sprites[index];
			Debug.LogFormat("Sprite at {0} set to {1}({2})", ownSprite, index, sprites[index]);
		}
	}
    public void Interact(int fingerID)
    {
        
        if (_currentHealth > 0)
        {
            // play pickaxe hit sound
            AudioManager.Instance.Click();
            _currentHealth--;          
        }
        else
        {
            int quantity = _stoneBreakOdds.GemQuantity();
            _stoneBreakOdds.PlayBreakSound(quantity);
            OnBreak?.Invoke(quantity, transform.position);
            _currentHealth = _maxHealth;
            StonePool.Instance.ReturnToPool(this);
        }
        
    }
}
