using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDRecord : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI user, coins, distance, points;

    public void SetUp(string _user, string _coins, string _distance, string _points)
    {
        user.text = _user;
        coins.text = _coins;
        distance.text = _distance;
        points.text = _points;
    }
}
