using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class ItemsProduceController : MonoBehaviour
{
    public GameObject itemPrefab;
    public GameObject spawnPlaces;

    [Serializable]
    class coord
    {
        public Vector3 xyz;
        public coord(int x, int y, int z)
        {
            xyz.x = x;
            xyz.y = y;
            xyz.z = z;
        }
    }
    int x, j;



    public void Produce()
    {
        for (int i = 0; i < spawnPlaces.transform.childCount; i++)
        {
            Instantiate(itemPrefab,
                spawnPlaces.transform.GetChild(
                    UnityEngine.Random.Range(0, spawnPlaces.transform.childCount)).transform.position, Quaternion.identity);
        }           
    }

    IEnumerator loopDelay(float time)
    {
        yield return new WaitForSeconds(time);
    }
}
