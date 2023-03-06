using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="NewPlayerType" , menuName ="PlayerType")]
public class PlayerType : ScriptableObject

{
    public Color playerColor = Color.green;
    public float playerSpeed = 15f;
    public int carryCapacity = 5;
}
