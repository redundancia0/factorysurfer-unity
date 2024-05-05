using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [SerializeField]
    Player _player;
    [SerializeField]
    GameObject _child;
   

    // Update is called once per frame
    void Update()
    {
        if(_player.transform.position.x >= _child.transform.position.x)
            transform.position = new Vector3(_child.transform.position.x, transform.position.y, transform.position.z);
    }
}
