using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class WallStates : MonoBehaviour
{
    public List<MeshRenderer> WallMesh;
    public GameObject WallLittle;

    public void SetWallState(bool hided)
    {
        foreach (var wall in WallMesh)
        {
            wall.shadowCastingMode = hided ? ShadowCastingMode.ShadowsOnly : ShadowCastingMode.On;
            WallLittle.SetActive(hided);
        }
    }
}
