using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField]
    Player _player;
    [SerializeField]
    float xOffset = -2f;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(_player.transform.position.x + xOffset, transform.position.y, transform.position.z);
    }
}
