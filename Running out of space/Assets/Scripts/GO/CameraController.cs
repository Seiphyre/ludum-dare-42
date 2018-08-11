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
            currentCameraDirection = value;
            if (currentCameraDirection < 0)
                currentCameraDirection = Direction.LENGTH - 1;
            else if (currentCameraDirection >= Direction.LENGTH)
                currentCameraDirection = 0;
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

    public void SetWallMeshs(List<MeshRenderer> north, List<MeshRenderer> east, List<MeshRenderer> south, List<MeshRenderer> west)
    {
        wall_N.SetMeshList(north);
        wall_E.SetMeshList(east);
        wall_S.SetMeshList(south);
        wall_W.SetMeshList(west);

        chdWall(CurrentCameraDirection);
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
            positiveRotation = false;
            TriggerInput();
        }
        else if (Input.GetButtonDown("RotateCameraLeft"))
        {
            positiveRotation = true;
            TriggerInput();
        }
    }
    #endregion

    private void TriggerInput()
    {
        if (corout != null)
            StopCoroutine(corout);

        Direction toDir = CurrentCameraDirection;
        toDir += positiveRotation ? 1 : -1;
        if (toDir < 0)
            toDir = Direction.LENGTH - 1;
        else if (toDir >= Direction.LENGTH)
            toDir = 0;

        if (positiveRotation)
            rot = GetCamRot(toDir, CurrentCameraDirection);
        else
            rot = GetCamRot(toDir, CurrentCameraDirection);

        CurrentCameraDirection = toDir;


        corout = StartCoroutine(CamRotate(
            () =>
            {
                chdWall(CurrentCameraDirection);
                return null;
            }));
    }

    private IEnumerator CamRotate(Func<object> p)
    {

        Vector3 _currentRot = transform.rotation.eulerAngles;
        bool _hasShowedWalls = false;
        float angle = Mathf.Abs(rot.y - (transform.rotation.eulerAngles.y)) % 360;
        //rot = transform.rotation.eulerAngles;
        while (angle > rotationStep)
        {
            print(transform.rotation.eulerAngles.y + " || " + rot.y);
            if (angle < 45 && !_hasShowedWalls)
            {
                p.Invoke();
                _hasShowedWalls = true;
            }
            //rot = new Vector3(rot.x, rot.y + rotationStep, rot.z);
            transform.Rotate(0, (positiveRotation ? 1 : -1) * rotationStep, 0);
            yield return new WaitForFixedUpdate();
            angle = Mathf.Abs(transform.rotation.eulerAngles.y - rot.y) % 360;
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
    }

    private Vector3 GetCamRot(Direction _camDir, Direction fromRot)
    {
        switch (_camDir)
        {
            default:
            case Direction.North:
                if (fromRot == Direction.West)
                    return new Vector3(0, 360, 0);
                else
                    return new Vector3(0, 0, 0);

            case Direction.East:
                return new Vector3(0, 90, 0);
            case Direction.South:
                return new Vector3(0, 180, 0);
            case Direction.West:
                return new Vector3(0, 270, 0);
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

    public void SetMeshList(List<MeshRenderer> meshList)
    {
        wallsGameObjects.Clear();

        wallsGameObjects = meshList;
    }
}
