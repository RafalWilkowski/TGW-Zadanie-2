using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
	private Rigidbody2D rb2D;
	private float initialDrag;
	BoxCollider2D ownCollider;
	[SerializeField] float pokeVelocityComponent = 10f;
	[SerializeField]
	//private Cona
	ColorType _gemColor;
	[SerializeField] float targetGravityScale = 5;
	[SerializeField] float colliderInactivityTime = 0.7f;

	bool isClickable = true;
	private void Start()
	{
		rb2D = GetComponent<Rigidbody2D>();
		ownCollider = GetComponentInChildren<BoxCollider2D>();
		initialDrag = rb2D.drag;
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.transform.parent.name == "ConveroyBelt")
		{
			rb2D.gravityScale = targetGravityScale;
			rb2D.drag = 0f;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.transform.parent.name == "ConveroyBelt")
		{
			rb2D.gravityScale = 0;
			rb2D.drag = initialDrag;
		}
	}
	public ColorType GetGemColor()
	{
		return _gemColor;
	}

	private void OnMouseDown()
	{
		if (!isClickable) return;
		rb2D.velocity = new Vector2(-1, 2) * pokeVelocityComponent;
		StartCoroutine(TemporarilyDisableBoxCollider());
	}

	public void ApplyForce(Vector2 vec, ForceMode2D forceMode = ForceMode2D.Force)
	{
		if (rb2D) rb2D.AddForce(vec, forceMode);
	}

	IEnumerator TemporarilyDisableBoxCollider()
	{

		if (!ownCollider) yield break;
		if (rb2D) rb2D.drag = 0;
		isClickable = false;
		ownCollider.enabled = false;
		yield return new WaitForSeconds(colliderInactivityTime);
		if (ownCollider) ownCollider.enabled = true;
		if (rb2D) rb2D.drag = initialDrag;
		isClickable = true;
	}
}
