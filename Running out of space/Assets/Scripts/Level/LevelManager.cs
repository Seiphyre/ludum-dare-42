using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public ItemFactory Factory;

    [Header("Prefabs")]
    [Space(5f)]
    public GameObject ItemEntityPrefab;
    [SerializeField] GameObject CameraPrefab;

    [Header("Levels")]
    [Space(5f)]
    public List<GameObject> Levels;

    public CameraController Camera
    {
        get
        {
            if (m_camera == null)
            {
                m_camera =
                Instantiate(CameraPrefab, transform.position, Quaternion.identity)
                .GetComponent<CameraController>();
            }
            return m_camera;
        }
    }

    public int GetItemLeftCount()
    {
        return m_currentLevel.ItemLeftCount();
    }

    void Awake()
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

    [ContextMenu("SpawnNextLevel")]
    void SpawnLevel()
    {
        StartCoroutine(SpawnNextLevel());
    }

    IEnumerator SpawnNextLevel()
    {
        m_currentLevel = Instantiate(Levels[m_currentLevelIndex]).GetComponent<LevelInstance>();

        yield return m_currentLevel.SpawnLevel();

        StartCurrentLevel();

        m_currentLevelIndex++;
    }

    void StartCurrentLevel()
    {
        if (m_currentLevel != null)
        {
            m_currentLevel.StartLevel();
        }
    }

    int m_currentLevelIndex;
    CameraController m_camera;
    LevelInstance m_currentLevel;
}
