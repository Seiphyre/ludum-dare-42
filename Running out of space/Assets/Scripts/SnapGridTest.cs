using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapGridTest : MonoBehaviour
{

	public ItemEntity Object { get; set; }

	private float objectRotY;

	private Renderer renderer;

	private void Awake()
	{

	}

	void Start ()
	{
		//tmp
		Object = transform.GetChild(0).GetComponent<ItemEntity>();
		Object.InitializeTest();

		objectRotY = 0;
		renderer = Object.GetComponentInChildren<Renderer>();

		MoveObjectOnGrid(transform.position);
	}



	private void Update()
	{
		//RaycastHit hit;
		//Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		//// Does the ray intersect any objects excluding the player layer
		//if (Physics.Raycast(ray, out hit))
		//{
		//	MoveObjectOnGrid(hit.point, new Vector3(1, 1, 2));
		//}

		// Rotate
		if (Input.GetButtonDown("Rotate"))
		{
			objectRotY += 90f;
			if (objectRotY == 360)
				objectRotY = 0;
		}

		MoveObjectOnGrid(transform.position);

		if (Input.GetButtonDown("Action"))
		{
			Vector3 dim = Object.Description.Size;
			Vector3 pos = Object.transform.position;

			GridManager.Instance.FromWorldPosAndDimToGridPos(ref pos, ref dim, Object);
			GridManager.Instance.AddObject(pos, dim, Object);
		}
	}



	private void MoveObjectOnGrid(Vector3 position)
	{
		Object.transform.eulerAngles = new Vector3(0, objectRotY, 0);
		float x, y, z;
		Vector3 dimension = Object.Description.Size;

		y = Mathf.Floor(position.y);

		if ((Object.transform.rotation.eulerAngles.y / 90f) % 2 == 1)
		{
			Vector2 newDim = new Vector2(dimension.z, dimension.x);

			dimension.x = newDim.x;
			dimension.z = newDim.y;
		}

		if (Mathf.Floor(position.x) + Mathf.Floor(dimension.x / 2) >= GridManager.Instance.MapSizeX)
			position.x = GridManager.Instance.MapSizeX - 0.4f - Mathf.Floor(dimension.x / 2);

		if (Mathf.Floor(position.x) - Mathf.Floor(dimension.x / 2) < 0)
			position.x = 0.4f + Mathf.Floor(dimension.x / 2);

		if (Mathf.Floor(position.z) + Mathf.Floor(dimension.z / 2) >= GridManager.Instance.MapSizeZ)
			position.z = GridManager.Instance.MapSizeZ - 0.4f - Mathf.Floor(dimension.z / 2);

		if (Mathf.Floor(position.z) - Mathf.Floor(dimension.z / 2) < 0)
			position.z = 0.4f + Mathf.Floor(dimension.z / 2);


		x = Mathf.Round(position.x);
		if (dimension.x % 2 == 1)
		{
			if (position.x % 0.5f == 0)
				x -= 0.5f;
			else
				x = Mathf.Ceil(position.x) - 0.5f;
		}

		z = Mathf.Round(position.z);
		if (dimension.z % 2 == 1)
		{
			if (position.z % 0.5f == 0)
				z -= 0.5f;
			else
				z = Mathf.Ceil(position.z) - 0.5f;
		}

		Object.transform.position = new Vector3(x, y, z);

		Vector3 objectGridPos = Object.transform.position;
		Vector3 objectDimPos = Object.Description.Size;

		GridManager.Instance.FromWorldPosAndDimToGridPos(ref objectGridPos, ref objectDimPos, Object);

		if (GridManager.Instance.IsCollideWithAnOtherObject(objectGridPos, objectDimPos))
		{
			renderer.material.color = Color.red;
		}
		else
			renderer.material.color = Color.white;
	}
}
