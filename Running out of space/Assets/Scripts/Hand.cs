using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{

	public ItemEntity CurrentObject { get; private set; }
	public ItemEntity SelectedObject { get; private set; }

	private float objectRotY;

	private Renderer currentObjRenderer;
	private Renderer selectedObjRenderer;
	private bool canBeRelease;



	// -- Start -------------------------

	void Start ()
	{
		objectRotY = 0;
		canBeRelease = true;
		MoveCurrentObjectOnGrid(transform.position);
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

		ItemEntity obj;
		// Found a selected object
		if (GridManager.GetInstance().IsCollideWithAnOtherObject(transform.position, out obj) == true && CurrentObject == null)
		{
			if ( SelectedObject == null )
			{
				selectedObjRenderer = obj.GetComponentInChildren<Renderer>();
				selectedObjRenderer.material.color = Color.blue;

				SelectedObject = obj;
			}

			else if ( (SelectedObject != null && obj != SelectedObject) )
			{
				// Reset previous selected obj
				selectedObjRenderer.material.color = Color.white;

				// Select new obj
				selectedObjRenderer = obj.GetComponentInChildren<Renderer>();
				selectedObjRenderer.material.color = Color.blue;

				SelectedObject = obj;
			}
		}
		else if (SelectedObject != null)
		{
			// Reset previous selected obj
			selectedObjRenderer.material.color = Color.white;
			SelectedObject = null;
		}

		// Update current position
		MoveCurrentObjectOnGrid(transform.position);

		// Place an object (Controler:A Keyboard:V)
		if (Input.GetButtonDown("Action"))
		{
			if (CurrentObject != null)
				ReleaseObject();

			else if (SelectedObject != null)
				TakeObject(SelectedObject);
		}
	}



	// -- Public functions ---------------------------

	public void TakeObject(ItemEntity entity)
	{
		// Do nothing, if there is already an object in the hand
		if (CurrentObject != null)
			return;

		if (SelectedObject != null)
			SelectedObject = null;

		entity.transform.parent = this.transform;
		currentObjRenderer = entity.GetComponentInChildren<Renderer>();
		GridManager.GetInstance().RemoveObject(entity);
		AllCollidersSetActive(entity, false);
		CurrentObject = entity;
	}

	public void ReleaseObject()
	{
		// Do nothing, if there is no object in the hand
		if (CurrentObject == null)
			return;

		if (canBeRelease == true)
		{
			CurrentObject.transform.parent = null;
			GridManager.GetInstance().AddObject(CurrentObject);
			StartCoroutine(WaitUntilPlayerExitFromTheObject(CurrentObject));
			CurrentObject = null;
		}
	}

	// -- Private functions -------------------------

	private IEnumerator WaitUntilPlayerExitFromTheObject(ItemEntity entity)
	{
		ItemEntity otherEntity;
		while (GridManager.GetInstance().IsCollideWithAnOtherObject(Player.GetInstance().transform.position, out otherEntity) == true)
		{
			yield return null;
		}

		AllCollidersSetActive(entity, true);
	}



	private void AllCollidersSetActive(ItemEntity entity, bool isActive)
	{
		foreach (var collider in entity.GetComponentsInChildren<Collider>())
			collider.enabled = isActive;
	}

	private void MoveCurrentObjectOnGrid(Vector3 position)
	{
		float x, y, z;
		Vector3 dimension;

		// Do not move object if there is not
		if (CurrentObject == null)
			return;

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
			currentObjRenderer.material.color = Color.red;
			canBeRelease = false;
		}
		else
		{
			currentObjRenderer.material.color = Color.white;
			canBeRelease = true;
		}
	}



	// -- Editor --------------------------------------

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawSphere(transform.position, 0.3f);
	}
}
