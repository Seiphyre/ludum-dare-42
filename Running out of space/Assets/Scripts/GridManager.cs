using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{

	public static GridManager Instance;

	public int MapSizeX;
	public int MapSizeY;
	public int MapSizeZ;

	[SerializeField]
	ItemEntity[][][] gridInfo;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(this);
		}
	}



	public void AddObject(Vector3 pos, Vector3 dimension, ItemEntity entity)
	{
		gridInfo[(int)pos.x][(int)pos.y][(int)pos.z] = entity;

		if (dimension.x > 1)
		{
			for (int x = 1; x < dimension.x; x++)
			{
				gridInfo[(int)pos.x + x][(int)pos.y][(int)pos.z] = entity;
			}
		}

		if (dimension.y > 1)
		{
			for (int y = 1; y < dimension.y; y++)
			{
				gridInfo[(int)pos.x][(int)pos.y + y][(int)pos.z] = entity;
			}
		}

		if (dimension.z > 1)
		{
			for (int z = 1; z < dimension.z; z++)
			{
				gridInfo[(int)pos.x][(int)pos.y][(int)pos.z + z] = entity;
			}
		}
	}

	public bool IsCollideWithAnOtherObject(Vector3 pos, Vector3 dimension)
	{
		if (gridInfo[(int)pos.x][(int)pos.y][(int)pos.z] != null)
			return true;

		if (dimension.x > 1)
		{
			for (int x = 1; x < dimension.x; x++)
			{
				if (gridInfo[(int)pos.x + x][(int)pos.y][(int)pos.z] != null)
					return true;
			}
		}

		if (dimension.y > 1)
		{
			for (int y = 1; y < dimension.y; y++)
			{
				if (gridInfo[(int)pos.x][(int)pos.y + y][(int)pos.z] != null)
					return true;
			}
		}

		if (dimension.z > 1)
		{
			for (int z = 1; z < dimension.z; z++)
			{
				if (gridInfo[(int)pos.x][(int)pos.y][(int)pos.z + z] != null)
					return true;
			}
		}

		return false;
	}

	// Use this for initialization
	void Start()
	{
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

		foreach (var entity in GameObject.FindObjectsOfType<ItemEntity>())
		{
			if (entity.GetComponentInParent<SnapGridTest>() != null)
				continue;

			Vector3 pos = entity.transform.position;
			Vector3 dim = entity.Description.Size;

			FromWorldPosAndDimToGridPos(ref pos, ref dim, entity);
			AddObject(pos, dim, entity);
		}
	}

	// Update is called once per frame
	void Update()
	{
		for (int x = 0; x < MapSizeX; x++)
		{
			for (int y = 0; y < MapSizeY; y++)
			{
				for (int z = 0; z < MapSizeZ; z++)
				{
					if (gridInfo[x][y][z] != null)
						//Debug.Log("pos : " + x + ", " + y + ", " + z);
						Debug.DrawLine(new Vector3(x, y, z), new Vector3(x + 1f, y, z + 1f));
				}
			}
		}
	}

	public void FromWorldPosAndDimToGridPos(ref Vector3 worldPos, ref Vector3 dimension, ItemEntity entity)
	{
		float x, y, z;

		if ((entity.transform.rotation.eulerAngles.y / 90f) % 2 == 1)
		{
			Vector2 newDim = new Vector2(dimension.z, dimension.x);

			dimension.x = newDim.x;
			dimension.z = newDim.y;
		}
		x = worldPos.x;

		x -= (dimension.x - 1) / 2;
		x = Mathf.Floor(x);

		y = worldPos.y;
		//if (dimension.y % 2 == 0)
		//{
		//	y -= 0.5f;
		//}
		//y -= (dimension.y - 1) / 2;
		//y = Mathf.Floor(y);

		z = worldPos.z;
		//if (dimension.z % 2 == 0)
		//{
		//	z -= 0.5f;
		//}
		z -= (dimension.z - 1) / 2;
		z = Mathf.Floor(z);

		worldPos = new Vector3(x, y, z);
	}
}
