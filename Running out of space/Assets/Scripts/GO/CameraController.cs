using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraController : MonoBehaviour
{

    [SerializeField] Wall wall_N, wall_E, wall_S, wall_W;
    [SerializeField] Direction currentCameraDirection;
    [SerializeField] float rotationStep = 1;
    bool positiveRotation;
    [SerializeField] Vector3 rot;
    Coroutine corout;

    public delegate void ChangeWalls(Direction newDir);

    public Direction CurrentCameraDirection
    {
        get
        {
            return currentCameraDirection;
        }

        set
        {
            if (value < 0)
                value = Direction.LENGTH - 1;
            else if (value >= Direction.LENGTH)
                value = 0;
            currentCameraDirection = value;
        }
    }

    public void SetWallVisibility(Direction vis)
    {
        switch (vis)
        {
            default:
            case Direction.North:
                wall_N.SetVisualShadow(false);
                break;
            case Direction.East:
                wall_E.SetVisualShadow(false);
                break;
            case Direction.South:
                wall_S.SetVisualShadow(false);
                break;
            case Direction.West:
                wall_W.SetVisualShadow(false);
                break;
        }
    }

    #region Unity Functions
    // Use this for initialization
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("RotateCameraRight"))
        {
            CurrentCameraDirection--;
            positiveRotation = false;
            TriggerInput();
        }
        else if (Input.GetButtonDown("RotateCameraLeft"))
        {
            CurrentCameraDirection++;
            positiveRotation = true;
            TriggerInput();
        }
    }
    #endregion

    private void TriggerInput()
    {
        if (corout != null)
            StopCoroutine(corout);
        rot = GetCamRot(CurrentCameraDirection);
        corout = StartCoroutine(CamRotate(
			()=>{
			chdWall(CurrentCameraDirection);
			return null;
		}));
    }

    private IEnumerator CamRotate(Func<object> p)
    {

        Vector3 _currentRot = transform.rotation.eulerAngles;
		bool _hasShowedWalls = false;
        //rot = transform.rotation.eulerAngles;
        while (Mathf.Abs(transform.rotation.eulerAngles.y - rot.y) % 360 > rotationStep)
        {
			if (Mathf.Abs(transform.rotation.eulerAngles.y - rot.y) % 360 < 45 && !_hasShowedWalls)
			{
				p.Invoke();
				_hasShowedWalls = true;
			}
            //rot = new Vector3(rot.x, rot.y + rotationStep, rot.z);
            transform.Rotate(0, (positiveRotation ? 1 : -1) * rotationStep, 0);
            yield return new WaitForFixedUpdate();
        }
        transform.rotation = Quaternion.Euler(rot);
    }

    private void chdWall(Direction dir)
    {
		wall_N.SetVisualShadow(true);
		wall_E.SetVisualShadow(true);
		wall_S.SetVisualShadow(true);
		wall_W.SetVisualShadow(true);
		 switch (dir)
        {
            default:
            case Direction.North:
                wall_S.SetVisualShadow(false);
				break;
            case Direction.East:
                wall_W.SetVisualShadow(false);
				break;
            case Direction.South:
                wall_N.SetVisualShadow(false);
				break;
            case Direction.West:
                wall_E.SetVisualShadow(false);
				break;
        }
				print("AAAAAAA");
    }

    private Vector3 GetCamRot(Direction _camDir)
    {
        switch (_camDir)
        {
            default:
            case Direction.North:
                return new Vector3(0, 0, 0);
            case Direction.East:
                return new Vector3(0, 90, 0);
            case Direction.South:
                return new Vector3(0, 180, 0);
            case Direction.West:
                return new Vector3(0, -90, 0);
        }
    }
    


}

[System.Serializable]
public enum Direction
{
    North,
    East,
    South,
    West,
    LENGTH
}

[System.Serializable]
class Wall
{
    public List<MeshRenderer> wallsGameObjects = new List<MeshRenderer>();
    public void SetVisualShadow(bool visible)
    {
        for (int i = 0; i < wallsGameObjects.Count; i++)
            wallsGameObjects[i].shadowCastingMode = visible ? ShadowCastingMode.On : ShadowCastingMode.ShadowsOnly;
    }
}
