using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Conteiner : MonoBehaviour
{
    public Action<ObjectColor> OnColorMatch;
    public Action OnColorMatched;

    [field: SerializeField]
    public ObjectColor ObjectColor { get; private set; }

}
