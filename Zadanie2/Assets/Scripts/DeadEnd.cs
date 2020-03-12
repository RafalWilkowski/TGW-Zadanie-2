using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadEnd : MonoBehaviour
{
    [SerializeField]
    private float _fading = 0.01f;
    [SerializeField]
    private float _extraRotation = 20f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.isTrigger)
        {
            // damp velocity
            collision.attachedRigidbody.velocity = new Vector2(1f, 0);
            collision.transform.localScale -= new Vector3(_fading, _fading, _fading);
            float newZ = collision.transform.rotation.z;
            newZ += _extraRotation * Time.deltaTime;
            collision.transform.Rotate(new Vector3(0, 0, _extraRotation));
        }

        if (collision.transform.localScale.x <= 0.1f)
        {
            var bomb = collision.GetComponent<Bomb>();
            var gemContainer = collision.GetComponent<GemConteiner>();
            if (bomb)
            {
                bomb.transform.localScale = new Vector3(1, 1, 1);
                DynamitePool.Instance.ReturnToPool(bomb);
            }
            else if (gemContainer)
            {
                gemContainer.transform.localScale = Vector3.one;
                StonePool.Instance.ReturnToPool(gemContainer);
            }
            else
            {
                GameManager.Instance.GameOver();
            }

        }
    }
}
