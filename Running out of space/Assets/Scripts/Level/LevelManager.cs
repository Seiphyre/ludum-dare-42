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
    public GameObject PlayerPrefab;

    public GameObject ContainerCanvas;

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

    public ContainerUI GetContainerCanvas()
    {
        if (m_containerUI == null)
        {
            m_containerUI = ContainerCanvas.GetComponent<ContainerUI>();
        }

        return m_containerUI;
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

        StartCoroutine(SpawnNextLevel());
    }

    private void Start()
    {
        //SoundManager.GetInstance().PlayAmbiantMusic();
    }

    [ContextMenu("SpawnNextLevel")]
    void SpawnLevel()
    {
        if (m_currentLevel != null)
        {
            m_currentLevel.CleanScene();
            Destroy(m_currentLevel.gameObject);
            m_currentLevel = null;
        }
        StartCoroutine(SpawnNextLevel());
    }

    IEnumerator SpawnNextLevel()
    {
        m_currentLevel = Instantiate(Levels[m_currentLevelIndex]).GetComponent<LevelInstance>();

        // Replace the camera
        m_camera.transform.position = new Vector3(m_currentLevel.MapDimension.x / 2f, 0, m_currentLevel.MapDimension.x / 2f);
        // Reset grid with the good dimensions
        GridManager.GetInstance().InitGrid(m_currentLevel.MapDimension);

        yield return m_currentLevel.SpawnLevel();

        // Instantiate player
        GameObject player = Instantiate(PlayerPrefab, m_currentLevel.transform);
        player.transform.position = new Vector3(m_currentLevel.SpawnPlayer.position.x, m_currentLevel.SpawnHeight, m_currentLevel.SpawnPlayer.position.z);

        yield return new WaitForSeconds(1);

        StartCurrentLevel();
    }

    void StartCurrentLevel()
    {
        if (m_currentLevel != null)
        {
            m_currentLevel.StartLevel();
        }
    }

    public void RetryCurrentLevel()
    {
        SpawnLevel();
    }

    public void StartNextLevel()
    {
        m_currentLevelIndex++;

        SpawnLevel();
    }

    public bool HasNextLevel()
    {
        return (m_currentLevelIndex + 1) < Levels.Count;
    }

    int m_currentLevelIndex;
    ContainerUI m_containerUI;
    CameraController m_camera;
    LevelInstance m_currentLevel;
}
