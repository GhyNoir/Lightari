using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyControllor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerControllor.instance.UpdateEnergy(0.1f);
            SupplyManager.instance.supplyList.Remove(gameObject);
            Destroy(gameObject);
        }
    }
}
