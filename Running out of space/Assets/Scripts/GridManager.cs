using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
	public int MapSizeX = 20;
	public int MapSizeY = 20;
	public int MapSizeZ = 20;

	[Header("Debug")]
	public bool DisplayGridInfosWithGizmos = false;

	private ItemEntity[][][] gridInfo;



	#region -- Singleton ----------------------------

	private static GridManager Instance;

	public static GridManager GetInstance()
	{
		if (Instance == null)
		{
			Instance = FindObjectOfType<GridManager>();

			if (Instance == null)
			{
				GameObject GridManager = new GameObject();
				Instance = GridManager.AddComponent<GridManager>();
			}
		}

		return Instance;
	}

	#endregion



	// -- Awake && Start --------------------------------

	private void Awake()
	{
		// Init Grid
		gridInfo = new ItemEntity[MapSizeX][][];

		for (int x = 0; x < MapSizeX; x++)
		{
			gridInfo[x] = new ItemEntity[MapSizeY][];

			for (int y = 0; y < MapSizeY; y++)
			{
				gridInfo[x][y] = new ItemEntity[MapSizeZ];

				for (int z = 0; z < MapSizeZ; z++)
				{
					gridInfo[x][y][z] = null;
				}
			}
		}
	}

	void Start()
	{
		// Update grid info with object already on the map
		foreach (var entity in GameObject.FindObjectsOfType<ItemEntity>())
		{
			// Ignore item in hand, if there is one
			if (entity.GetComponentInParent<Hand>() != null)
				continue;

			AddObject(entity);
		}
	}



	// -- Update ---------------------------------------

	void Update()
	{
		// Visual debug
		if (DisplayGridInfosWithGizmos == true)
		{
			for (int x = 0; x < MapSizeX; x++)
			{
				for (int y = 0; y < MapSizeY; y++)
				{
					for (int z = 0; z < MapSizeZ; z++)
					{
						if (gridInfo[x][y][z] != null)
						{
							Debug.DrawLine(new Vector3(x, y, z), new Vector3(x + 1f, y + 1, z + 1f));
							Debug.DrawLine(new Vector3(x + 1f, y, z), new Vector3(x, y + 1, z + 1f));
							Debug.DrawLine(new Vector3(x, y, z + 1), new Vector3(x + 1f, y + 1, z));
							Debug.DrawLine(new Vector3(x + 1f, y, z + 1), new Vector3(x, y + 1, z));
						}
					}
				}
			}
		}
	}



	// -- Public functions ---------------------------------------------

	public void InitGrid (Vector3 gridDimention)
	{
		MapSizeX = (int) gridDimention.x;
		MapSizeY = (int) gridDimention.y;
		MapSizeZ = (int) gridDimention.z;

		gridInfo = new ItemEntity[MapSizeX][][];

		for (int x = 0; x < MapSizeX; x++)
		{
			gridInfo[x] = new ItemEntity[MapSizeY][];

			for (int y = 0; y < MapSizeY; y++)
			{
				gridInfo[x][y] = new ItemEntity[MapSizeZ];

				for (int z = 0; z < MapSizeZ; z++)
				{
					gridInfo[x][y][z] = null;
				}
			}
		}
	}

	public void AddObject(ItemEntity entity)
	{
		Vector3 pos;
		Vector3 dimension;

		FromWorldPosAndDimToGridPos(entity, out pos, out dimension);

		for (int x = 0; x < dimension.x; x++)
		{
			for (int y = 0; y < dimension.y; y++)
			{
				for (int z = 0; z < dimension.z; z++)
				{
					//if ((pos.x < 0 || pos.x >= MapSizeX) || (pos.y < 0 || pos.y >= MapSizeY) || (pos.z < 0 || pos.z >= MapSizeZ))
					//	continue;
					gridInfo[(int)pos.x + x][(int)pos.y + y][(int)pos.z + z] = entity;
				}
			}
		}
	}

	public void RemoveObject(ItemEntity entity)
	{
		Vector3 pos;
		Vector3 dimension;

		FromWorldPosAndDimToGridPos(entity, out pos, out dimension);

		for (int x = 0; x < dimension.x; x++)
		{
			for (int y = 0; y < dimension.y; y++)
			{
				for (int z = 0; z < dimension.z; z++)
				{
					//if ((pos.x < 0 || pos.x >= MapSizeX) || (pos.y < 0 || pos.y >= MapSizeY) || (pos.z < 0 || pos.z >= MapSizeZ))
					//	continue;

					gridInfo[(int)pos.x + x][(int)pos.y + y][(int)pos.z + z] = null;
				}
			}
		}
	}

	public bool IsCollideWithAnOtherObject(ItemEntity entity)
	{
		Vector3 pos;
		Vector3 dimension;

		FromWorldPosAndDimToGridPos(entity, out pos, out dimension);

		for (int x = 0; x < dimension.x; x++)
		{
			for (int y = 0; y < dimension.y; y++)
			{
				for (int z = 0; z < dimension.z; z++)
				{
					if (gridInfo[(int)pos.x + x][(int)pos.y + y][(int)pos.z + z] != null)
						return true;
				}
			}
		}

		return false;
	}

	public bool IsCollideWithAnOtherObject(Vector3 pos, out ItemEntity entity)
	{
		Vector3 dimension = Vector3.one;
		entity = null;
		pos = new Vector3(Mathf.Floor(pos.x), Mathf.Round(pos.y), Mathf.Floor(pos.z));

		if (!(pos.x >= 0 && pos.x < MapSizeX))
			return false;

		if (!(pos.y >= 0 && pos.y < MapSizeY))
			return false;

		if (!(pos.z >= 0 && pos.z < MapSizeZ))
			return false;

		if (gridInfo[(int)pos.x][(int)pos.y][(int)pos.z] != null)
		{
			entity = gridInfo[(int)pos.x][(int)pos.y][(int)pos.z];
			return true;
		}
		return false;
	}

	public bool IsThereAContainerObject(Vector3 pos)
	{
		if (gridInfo[(int)pos.x][(int)pos.y][(int)pos.z] != null)
		{
			if (gridInfo[(int)pos.x][(int)pos.y][(int)pos.z].Description.ContainerType == ItemContainerType.Containing)
				return true;
		}

		return false;
	}

	public void FitObjectInGrid(ItemEntity entity)
	{
		float x, y, z;
		Vector3 dimension;
		Vector3 position = entity.transform.position;

		// Do not move object if there is not
		if (entity == null)
			return;

		// Update dimension with rotation
		dimension = entity.Description.Size;

		if ((entity.transform.rotation.eulerAngles.y / 90f) % 2 == 1)
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
				x = position.x;
			else
				x = Mathf.Ceil(position.x) - 0.5f;
		}

		// Calcul position z
		z = Mathf.Round(position.z);
		if (dimension.z % 2 == 1)
		{
			if (position.z % 0.5f == 0)
				z = position.z;
			else
				z = Mathf.Ceil(position.z) - 0.5f;
		}

		//// Calcul position x
		//x = position.x;

		////x -= (dimension.x - 1) / 2;
		////x = Mathf.Floor(x);

		//// Calcul positions y
		//y = position.y;


		//// Calcul position z
		//z = position.z;

		////z -= (dimension.z - 1) / 2;
		////z = Mathf.Floor(z);

		// Update position
		Debug.Log("Debut " + entity.transform.position);
		entity.transform.position = new Vector3(x, entity.transform.position.y, z);
		Debug.Log("Fin " + entity.transform.position);

		//// Collision detection
		//if (GridManager.GetInstance().IsCollideWithAnOtherObject(CurrentObject))
		//{
		//	currentObjRenderer.material.color = Color.red;
		//	canBeRelease = false;
		//}
		//else
		//{
		//	currentObjRenderer.material.color = Color.white;
		//	canBeRelease = true;
		//}
	}

	// -- static functions -----------------

	public static void FromWorldPosAndDimToGridPos(ItemEntity entity, out Vector3 position, out Vector3 dimension)
	{
		float x, y, z;
		Vector3 worldPos = entity.transform.position;
		Vector3 originalDimensions = entity.Description.Size;

		// Update Dimensions
		if ((entity.transform.rotation.eulerAngles.y / 90f) % 2 == 1)
		{
			Vector2 newDim = new Vector2(originalDimensions.z, originalDimensions.x);

			dimension.x = newDim.x;
			dimension.y = originalDimensions.y;
			dimension.z = newDim.y;
		}
		else
			dimension = originalDimensions;

		// Calcul position x
		x = worldPos.x;

		x -= (dimension.x - 1) / 2;
		x = Mathf.Floor(x);

		// Calcul positions y
		y = worldPos.y;


		// Calcul position z
		z = worldPos.z;

		z -= (dimension.z - 1) / 2;
		z = Mathf.Floor(z);

		// Update position
		position = new Vector3(x, y, z);
	}
}
