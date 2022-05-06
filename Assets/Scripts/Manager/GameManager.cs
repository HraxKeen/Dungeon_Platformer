using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;
    [SerializeField] public GameObject player;
    [SerializeField] private float respawnTime;

    public static GameManager instance;

    private float respawnTimeStart = 5f;

    private bool respawn = false;

    private CinemachineVirtualCamera CVC;

    private healthBar HB;


    private void Start() 
    {
        instance = this;

        CVC = GameObject.Find("Player Camera").GetComponent<CinemachineVirtualCamera>();    
    }

    public void Update() 
    {
        CheckRespawn();
    }
    public void Respawn()
    {
        respawnTime = Time.time;
        respawn = true;
        SceneManager.LoadScene("_game");

    }
    public void CheckRespawn()
    {

        if(Time.time >= respawnTimeStart + respawnTime && respawn)
        {
            var playerTemp = Instantiate(player, respawnPoint);
            CVC.m_Follow = playerTemp.transform;
            respawn = false;
        }
    }


}
