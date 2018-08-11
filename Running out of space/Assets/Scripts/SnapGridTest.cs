using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapGridTest : MonoBehaviour
{

	void Start ()
	{
		MoveObjectOnGrid(transform.position, new Vector3(1,1,2));
	}



	private void Update()
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		// Does the ray intersect any objects excluding the player layer
		if (Physics.Raycast(ray, out hit))
		{
			Debug.Log("Pos : " + hit.point);
			MoveObjectOnGrid(hit.point, new Vector3(1, 1, 2));
		}

		// Rotate
		if (Input.GetMouseButtonDown(0))
		{
			transform.Rotate(Vector3.up, 90f);
		}
	}



	private void MoveObjectOnGrid(Vector3 position, Vector3 dimension)
	{
		float x, y, z;

		y = Mathf.Floor(position.y);

		Debug.Log("res : " + transform.rotation.eulerAngles.y / 90f);

		if ( (transform.rotation.eulerAngles.y / 90f) % 2 == 1)
		{
			Vector2 newDim = new Vector2(dimension.z, dimension.x);

			dimension.x = newDim.x;
			dimension.z = newDim.y;
		}


		x = Mathf.Round(position.x);
		if (dimension.x % 2 == 1)
		{
			x = Mathf.Ceil(position.x) - 0.5f;
		}

		z = Mathf.Round(position.z);
		if (dimension.z % 2 == 1)
		{
			z = Mathf.Ceil(position.z) - 0.5f;
		}

		transform.position = new Vector3(x, y, z);
	}
}
