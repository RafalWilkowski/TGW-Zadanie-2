using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Conteiner : MonoBehaviour
{
    public static Action<ObjectColor> OnColorMatch;

    [field: SerializeField]
    public ObjectColor ObjectColor { get; private set; }

}
