using DG.Tweening;
using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSpawnController : MonoBehaviour
{
    public GameObject playerPrefab1;
    public GameObject playerPrefab2;
    public Transform spawnPlace;
    public SplineComputer splineComputer;
    public GameObject itemProducer;
    public GameObject dropPlace;


    public List<GameObject> activePlayersLevel1;
    public List<GameObject> activePlayersLevel2;

   



    public void AddPlayer1(GameObject playerPrefab)
    {
        var player = Instantiate(playerPrefab, spawnPlace.transform.position, Quaternion.identity);
        player.GetComponent<SplineFollower>().spline = splineComputer;
        player.GetComponent<PlayerInteractController>().dropPlace = dropPlace;
        activePlayersLevel1.Add(player);
        player.transform.DOPunchScale(Vector3.one, 0.5f, 10);

    }
    public void AddPlayer2(GameObject playerPrefab)
    {
        var player = Instantiate(playerPrefab, spawnPlace.transform.position, Quaternion.identity);
        player.GetComponent<SplineFollower>().spline = splineComputer;
        player.GetComponent<PlayerInteractController>().dropPlace = dropPlace;
        activePlayersLevel2.Add(player);
        player.transform.DOPunchScale(Vector3.one, 0.5f, 10);

    }
}
