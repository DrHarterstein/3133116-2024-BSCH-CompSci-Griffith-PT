using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetColor : MonoBehaviour
{
	public float TimeToChange;
	public Color[] ColorToChange;
	void OnEnable()
	{
		StartCoroutine(ChangeColor(TimeToChange));
	}
	int i = 0;
	public string[] tags;

	public void StopThis()
	{
		StopCoroutine(ChangeColor(0));
	}

	IEnumerator ChangeColor(float delay)
	{
		if (i > ColorToChange.Length - 1)
		{
			i = 0;
		}
		GetComponent<SpriteRenderer>().color = ColorToChange[i];
		transform.tag = tags[i];
		yield return new WaitForSeconds(delay);
		i++;
		StartCoroutine(ChangeColor(TimeToChange));
	}
}
