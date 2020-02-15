using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampAsynchronizer : MonoBehaviour
{
	Animator[] anims;
	int asyncPropertyHash, speedPropertyHash;
	[SerializeField] string asyncProperty = "async";
	[SerializeField] string speedProperty = "speed";
    // Start is called before the first frame update
    void Awake()
    {
		anims = GetComponentsInChildren<Animator>();
		asyncPropertyHash = Animator.StringToHash(asyncProperty);
		speedPropertyHash = Animator.StringToHash(speedProperty);
		foreach (Animator anim in anims)
		{
			anim.SetFloat(asyncPropertyHash, Random.value);
			anim.SetFloat(speedProperty, Random.Range(0.7f, 1.1f));
		}
		enabled = false;
    }
}
