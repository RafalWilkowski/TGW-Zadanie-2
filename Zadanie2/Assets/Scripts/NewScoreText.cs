﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NewScoreText : MonoBehaviour
{
    public Action OnGlideFinished;

    public void OnGlideEnded()
    {
        OnGlideFinished?.Invoke();
    }
}
