using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{

	public ItemEntity CurrentObject { get; set; }

	private float objectRotY;

	private Renderer rdr;



	// -- Start -------------------------

	void Start ()
	{
		//tmp
		CurrentObject = transform.GetChild(0).GetComponent<ItemEntity>();
		CurrentObject.InitializeTest();

		objectRotY = 0;
		rdr = CurrentObject.GetComponentInChildren<Renderer>();

		MoveObjectOnGrid(transform.position);
	}



	// -- Update ---------------------------

	private void Update()
	{
		// Rotate (Controler:B Keyboard:R)
		if (Input.GetButtonDown("Rotate"))
		{
			objectRotY += 90f;
			if (objectRotY == 360)
				objectRotY = 0;
		}

		// Update position
		MoveObjectOnGrid(transform.position);

		// Place an object (Controler:A Keyboard:V)
		if (Input.GetButtonDown("Action"))
		{
			GridManager.GetInstance().AddObject(CurrentObject);
		}
	}



	// -- private functions -------------------------

	private void MoveObjectOnGrid(Vector3 position)
	{
		float x, y, z;
		Vector3 dimension;

		// Update rotation
		CurrentObject.transform.eulerAngles = new Vector3(0, objectRotY, 0);

		// Update dimension with rotation
		dimension = CurrentObject.Description.Size;

		if ((CurrentObject.transform.rotation.eulerAngles.y / 90f) % 2 == 1)
		{
			Vector2 newDim = new Vector2(dimension.z, dimension.x);

			dimension.x = newDim.x;
			dimension.z = newDim.y;
		}

		// Boundary check
		if (Mathf.Floor(position.x) + Mathf.Floor(dimension.x / 2) >= GridManager.GetInstance().MapSizeX)
			position.x = GridManager.GetInstance().MapSizeX - 0.4f - Mathf.Floor(dimension.x / 2);

		if (Mathf.Floor(position.x) - Mathf.Floor(dimension.x / 2) < 0)
			position.x = 0.4f + Mathf.Floor(dimension.x / 2);

		if (Mathf.Floor(position.z) + Mathf.Floor(dimension.z / 2) >= GridManager.GetInstance().MapSizeZ)
			position.z = GridManager.GetInstance().MapSizeZ - 0.4f - Mathf.Floor(dimension.z / 2);

		if (Mathf.Floor(position.z) - Mathf.Floor(dimension.z / 2) < 0)
			position.z = 0.4f + Mathf.Floor(dimension.z / 2);

		// Calcul position y
		y = Mathf.Floor(position.y);

		// Calcul position x
		x = Mathf.Round(position.x);
		if (dimension.x % 2 == 1)
		{
			if (position.x % 0.5f == 0)
				x -= 0.5f;
			else
				x = Mathf.Ceil(position.x) - 0.5f;
		}

		// Calcul position z
		z = Mathf.Round(position.z);
		if (dimension.z % 2 == 1)
		{
			if (position.z % 0.5f == 0)
				z -= 0.5f;
			else
				z = Mathf.Ceil(position.z) - 0.5f;
		}

		// Update position
		CurrentObject.transform.position = new Vector3(x, y, z);

		// Collision detection
		if (GridManager.GetInstance().IsCollideWithAnOtherObject(CurrentObject))
		{
			rdr.material.color = Color.red;
		}
		else
			rdr.material.color = Color.white;
	}
}
