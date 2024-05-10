using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationScript : MonoBehaviour
{
	public Vector3 RotationSpeed;

	void Update()
	{
		transform.Rotate(RotationSpeed.x * Time.deltaTime, RotationSpeed.y * Time.deltaTime, RotationSpeed.z * Time.deltaTime);
	}
}
