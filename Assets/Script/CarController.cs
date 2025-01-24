using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
   public int gas = 100;
   private float moveSpeed = 1f;
   
   public int Gas
   {
      get => gas;
   }

   private void Start()
   {
      StartCoroutine(GasCoroutine());
   }

   IEnumerator GasCoroutine()
   {
      while (true)
      {
         gas -= 10;
         if (gas <= 0)
         {
            break;
         }
         yield return new WaitForSeconds(1f);
      }
      //TODO: 게임종료
   }
   
   public void Move(float direction)
   {
      transform.Translate(Vector3.right * direction * Time.deltaTime);
      transform.position = new Vector3(Mathf.Clamp(transform.position.x,-2f, 2f), 0, transform.position.z);
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.tag == "Gas")
      {
         gas += 30;
         
         //TODO: 가스아이템 제거
      }
   }
}
