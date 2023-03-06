using DG.Tweening;
using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMergeController : MonoBehaviour
{
    public PlayerSpawnController spawnController;


    public void MergePlayers()
    {
        if(spawnController.activePlayersLevel1.Count >= 2)
        {
            var firstPlayer = spawnController.activePlayersLevel1[spawnController.activePlayersLevel1.Count - 1];
            var secondPlayer = spawnController.activePlayersLevel1[spawnController.activePlayersLevel1.Count - 2];

            firstPlayer.GetComponent<SplineFollower>().followSpeed = 0;
            firstPlayer.GetComponent<SplineFollower>().enabled = false;

            secondPlayer.GetComponent<SplineFollower>().followSpeed = 0;
            secondPlayer.GetComponent<SplineFollower>().enabled = false;


            var centralPoint = (firstPlayer.transform.position + secondPlayer.transform.position) / 2f;

            //firstPlayer.transform.position = centralPoint;
            firstPlayer.transform.DOMove(centralPoint, 0.5f);
            firstPlayer.transform.GetComponent<PlyerEffectController>().smokeParticle.SetActive(true);
            Destroy(firstPlayer, 0.6f);
            spawnController.activePlayersLevel1.RemoveAt(spawnController.activePlayersLevel1.Count - 1);

            //secondPlayer.transform.position = centralPoint;
            secondPlayer.transform.DOMove(centralPoint, 0.5f);
            secondPlayer.transform.GetComponent<PlyerEffectController>().smokeParticle.SetActive(true);
            Destroy(secondPlayer, 0.6f);
            spawnController.activePlayersLevel2.RemoveAt(spawnController.activePlayersLevel1.Count - 2);
        }
        

    }
}
