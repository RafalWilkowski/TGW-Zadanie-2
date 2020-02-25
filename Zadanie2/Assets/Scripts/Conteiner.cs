using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Conteiner : MonoBehaviour
{
    public static Action<ObjectColor> OnColorMatch;
    public static Action OnColorMatched;

    [field: SerializeField]
    public ObjectColor ObjectColor { get; protected set; }

}
