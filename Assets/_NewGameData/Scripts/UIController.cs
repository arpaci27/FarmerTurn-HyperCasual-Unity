using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject gameUI;
    public GameObject tapToPlayUI;
    public GameObject tapToSpeedUpUI;
    public PlayerSpawnController spawnController;

    private void Awake()
    {
        Time.timeScale = 0f;
    }
    public void TapToPlay()
    {
        tapToPlayUI.SetActive(false);
        gameUI.SetActive(true);
        Time.timeScale = 1f;

    }

    public void TapToSpeedUp()
    {
        if (Input.GetMouseButtonDown(0))
        {
            tapToSpeedUpUI.SetActive(false);
            for (int i = 0; i < spawnController.activePlayersLevel1.Count; i++)
            {
                spawnController.activePlayersLevel1[i].GetComponent<SplineFollower>().followSpeed += 4;

            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            StartCoroutine(Delay(1f));
            tapToSpeedUpUI.SetActive(true);
            for (int i = 0; i < spawnController.activePlayersLevel1.Count; i++)
            {
                spawnController.activePlayersLevel1[i].GetComponent<SplineFollower>().followSpeed -= 4;

            }
        }
    }

    private IEnumerator Delay(float time)
    {
        yield return new WaitForSeconds(time);
    }
    private void Update()
    {
        TapToSpeedUp();
    }
}
