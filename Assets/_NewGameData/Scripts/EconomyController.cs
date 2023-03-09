using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyController : MonoBehaviour
{
    public static EconomyController Instance;
    public int stackedAmount = 0;

    public int moneyAmount = 220;

    private void Awake()
    {
        Instance = this;
    }

    public void DecreaseMoney(int decreaseAmount)
    {
        moneyAmount -= decreaseAmount;
    }

    public void IncreaseMoney(int IncreaseAmount)
    {
        moneyAmount += IncreaseAmount;
    }
}
