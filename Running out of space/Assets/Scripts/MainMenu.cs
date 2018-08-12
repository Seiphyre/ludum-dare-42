using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Transform Camera;
    public GameObject OptionCanvas;

    public Selectable OptionSelectable;
    public Selectable MainSelectable;
    public Slider SoundSlider;

    public List<Transform> JumpingItems;


    void Awake()
    {
        StartLoopShake();

        MainSelectable.Select();
    }
    public void GoToOption()
    {
        OptionCanvas.SetActive(true);
        OptionSelectable.Select();
    }

    public void GoToMain()
    {
        OptionCanvas.SetActive(false);
        MainSelectable.Select();
    }

    public void GoToGame()
    {
        SceneManager.LoadScene("MainGameplay");
    }

    void StartLoopShake(float magnitude = 0.05f)
    {
        StartCoroutine(Shake(Camera, Random.Range(1f, 5f), magnitude));
    }

    IEnumerator Shake(Transform item, float duration, float magnitude)
    {
        Vector3 originalPosition = item.localPosition;

        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            item.transform.localPosition = Vector3.Lerp(item.transform.localPosition, new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z), 0.1f);

            yield return new WaitForEndOfFrame();

            timeElapsed += Time.deltaTime;
        }

        item.localPosition = originalPosition;

        StartLoopShake(Random.Range(0.01f, 0.05f));
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
