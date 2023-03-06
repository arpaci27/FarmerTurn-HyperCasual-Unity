using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Dreamteck.Splines;

public class PlayerInteractController : MonoBehaviour
{
    [SerializeField] public Transform stackParent;
    public GameObject dropPlace;
    public int carryCapacity = 5;
    public GameObject max;
    private SplineFollower splineFollower;
    private ItemsProduceController ItemsProduceController;


    private void Awake()
    {
        splineFollower = GetComponent<SplineFollower>();
        ItemsProduceController = GetComponent<ItemsProduceController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Stackable"))
        {
            if(stackParent.childCount < carryCapacity)
            {
                other.tag = "Untagged";
                other.transform.SetParent(stackParent);
                other.transform.DOMove(new Vector3(stackParent.position.x,
                    stackParent.position.y + stackParent.childCount,
                    stackParent.position.z
                    ), 0.2f);

                splineFollower.followSpeed--;
            }
            if(stackParent.childCount == carryCapacity)
            {
                max.gameObject.SetActive(true);
            }
           
           
        }
        if(transform.name != "Player1")
        {
            if (other.CompareTag("FinishLine"))
            {
                //ItemsProduceController.Produce();
            }
        }
        
    }
    public void FinishState()
    {
        StartCoroutine(SpeedResetter(15));
    }

    public IEnumerator SpeedResetter(float speed)
    {
        splineFollower.followSpeed = 0;
        int stackco = stackParent.childCount;
        for (int i = 0; i < stackco; i++)
        {
            stackParent.transform.GetChild(0).DOMove(dropPlace.transform.position, 0.4f);
            stackParent.GetChild(0).SetParent(null);
        }
        max.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        splineFollower.followSpeed = speed;
    }

    
}
