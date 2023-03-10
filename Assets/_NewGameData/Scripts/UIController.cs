using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    public GameObject gameUI;
    public GameObject tapToPlayUI;
    public GameObject tapToSpeedUpUI;
    public PlayerSpawnController spawnController;
    public GameObject moneyText;
    public GameObject canvas;
    public Transform moneyTextPlace;
    public Slider levelProgressSlider;
    public TextMeshProUGUI sliderBarText;
    public TextMeshProUGUI moneyAmount;

    public GameObject victoryUI;

    private void Awake()
    {
        instance = this;
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
                if (spawnController.activePlayersLevel1[i]!= null)
                {
                    spawnController.activePlayersLevel1[i].GetComponent<SplineFollower>().followSpeed += 4;
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            StartCoroutine(Delay(1f));
            tapToSpeedUpUI.SetActive(true);
            for (int i = 0; i < spawnController.activePlayersLevel1.Count; i++)
            {
                if(spawnController.activePlayersLevel1[i] != null)
                {
                    spawnController.activePlayersLevel1[i].GetComponent<SplineFollower>().followSpeed -= 4;
                }
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
        moneyAmount.text = EconomyController.Instance.moneyAmount.ToString();
        levelProgressSlider.value = EconomyController.Instance.stackedAmount;
        sliderBarText.text = levelProgressSlider.value.ToString();
        if (levelProgressSlider.value == 200)
        {
            gameUI.SetActive(false);
            victoryUI.SetActive(true);
        }
    }



    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MoneyTextDisplayer()
    {
        var moneytext = Instantiate(moneyText,moneyTextPlace.position , Quaternion.identity);
        moneytext.transform.SetParent(canvas.transform);
    }

   
}
