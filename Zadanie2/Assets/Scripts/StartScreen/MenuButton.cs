﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour , IPointerEnterHandler
{

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.Click();
    }
    
}

