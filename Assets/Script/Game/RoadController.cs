using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoadController : MonoBehaviour
{
    [SerializeField] private GameObject[] gasObjects;

    private void OnDisable()
    {
        // 모든 가스 아이템 비활성
        foreach (var gasObject in gasObjects)
        {
            gasObject.SetActive(false);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.Instance.SpawnRoad(transform.position + new Vector3(0, 0, 10));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.Instance.DestroyRoad(gameObject);
        }
    }
    
    public void SpawnGas()
    {
        int index = Random.Range(0, gasObjects.Length);
        gasObjects[index].SetActive(true);
    }
}
