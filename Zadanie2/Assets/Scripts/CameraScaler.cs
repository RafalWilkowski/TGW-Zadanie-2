using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScaler : MonoBehaviour
{
   
        [SerializeField]
        private float width = Screen.height;
        [SerializeField]
        private float height = Screen.width;

        void Awake()
        {
            Camera.main.aspect = width / height;
        }
    
}
