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

    private void Start()
    {
        _stoneBreakOdds = FindObjectOfType<StoneBreakOdds>();
        _currentHealth = _maxHealth;
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
