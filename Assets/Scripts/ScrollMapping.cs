using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollMapping : MonoBehaviour
{

   public float fScrollSpeed = 0.5f;
   float fTarget_Offset = 0;
   
   // Update is called once per frame
   void Update()
   {
       fTarget_Offset += Time.deltaTime * fScrollSpeed;
   
       Renderer render = this.GetComponent<Renderer>();
       render.material.mainTextureOffset =
                               new Vector2(fTarget_Offset, 0);
   }
}
