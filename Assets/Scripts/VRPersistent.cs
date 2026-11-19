using UnityEngine;

public class VRPersistent : MonoBehaviour
{
   private void Awake(){ 
        DontDestroyOnLoad(gameObject);
   }
}
