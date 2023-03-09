using DG.Tweening;
using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerMergeController : MonoBehaviour
{
    public PlayerSpawnController spawnController;
    public Transform finsihLine;
    public GameObject player2Prefab;


    public void MergePlayers()
    {
        List<GameObject> temp = new List<GameObject>();
        if(spawnController.activePlayersLevel1.Count >= 2)
        {
            var centralPoint = (spawnController.activePlayersLevel1[0].transform.position + spawnController.activePlayersLevel1[1].transform.position) / 2f;
            for (int i = 0; i < 2; i++)
            {
                var Player = spawnController.activePlayersLevel1[0];
                Player.GetComponent<SplineFollower>().followSpeed = 0;
                Player.GetComponent<SplineFollower>().enabled = false;
                temp.Add(Player); 
                spawnController.activePlayersLevel1.Remove(Player);
                temp[0].transform.GetComponent<PlyerEffectController>().smokeParticle.SetActive(true);
                temp[i].transform.DOMove(centralPoint, 0.5f).OnComplete(() => {
                    temp[0].SetActive(false); temp[1].SetActive(false);
                });
            }
            spawnController.AddPlayer2(player2Prefab);
        }
        

    }
}
