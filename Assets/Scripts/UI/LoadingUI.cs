using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingUI : MonoBehaviour
{

    [SerializeField]
    float angularSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles += Vector3.forward * angularSpeed * Time.deltaTime;
    }
}
