using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Clase que se encarga de seguir al player
public class MainPlatform : MonoBehaviour
{
    [SerializeField]
    private Transform playerTr;

    private void Awake()
    {
        if (playerTr == null)
        {
            playerTr = GameObject.Find("Player").transform;
        }
    }

    void Update()
    {
        transform.position = new Vector3(playerTr.position.x, transform.position.y, 0);
    }
}
