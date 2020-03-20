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
	Collider2D ownCollider;
	ParticleSystem _crackOpenEffect;

	private void Start()
    {
        _stoneBreakOdds = FindObjectOfType<StoneBreakOdds>();
		_crackOpenEffect = GetComponentInChildren<ParticleSystem>();
		ownCollider = GetComponentInChildren<Collider2D>();
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

			if (ownSprite) ownSprite.enabled = false;
			if (ownCollider) ownCollider.enabled = false;
			if (_crackOpenEffect)_crackOpenEffect.Play();
			
            OnBreak?.Invoke(quantity, transform.position);  
			
        } 
    }

	private void OnDisable()
	{
		_currentHealth = _maxHealth;
		StonePool.Instance.ReturnToPool(this);
		if (ownSprite) ownSprite.enabled = true;
		if (ownCollider) ownCollider.enabled = false;
	}
}
