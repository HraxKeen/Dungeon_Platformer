using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] 
    private int damage = 5;
    private GameObject player;

    public GameObject PC;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        PC.GetComponent<PlayerController>().CheckKnockback();
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player"))
        {
            if(collider.GetComponent<PlayerStats>() != null)
            {
                collider.GetComponent<PlayerStats>().DecreaseHealth(damage);
            }
        }
    }
}
