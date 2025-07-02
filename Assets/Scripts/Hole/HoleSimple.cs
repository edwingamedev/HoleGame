using UnityEngine;
public class HoleSimple : MonoBehaviour
{
    [SerializeField] private int foodLayer;
    [SerializeField] private int fallLayer;
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter: "+other.gameObject.name);
        
        if (other.gameObject.layer != foodLayer)
        {
            return;
        }
        
        other.gameObject.layer = fallLayer;
    }
    
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("OnTriggerExit: "+other.gameObject.name);
    
        if (other.gameObject.layer != fallLayer)
        {
            return;
        }
        
        other.gameObject.layer = foodLayer;
    }
}
