using System.Collections;
using UnityEngine;



class ObjectPoolMember : MonoBehaviour
{
   ObjectPool tParentPool = null;

   public void SetPool(ObjectPool tPool) { tParentPool = tPool; }
}
