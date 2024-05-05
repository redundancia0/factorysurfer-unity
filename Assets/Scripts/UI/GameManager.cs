using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SCENE : int
{
    MAIN, GAME, POST_GAME, STORE
}

public enum SKIN : int
{
    BIRD, ARMOR, TANK
}

[System.Serializable]
public struct PlayerPoints
{
    public float distance;
    public int totalPoints;
    public int numCoins;
}

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private string distanceName, numCoinsName, totalPointsName, totalCoins, currentSkinName;
    [SerializeField]
    private string birdSkinName, armorSkinName, tankSkinName;

    public static GameManager Instance;

    private PlayerPoints currentPlayerPoints;

    private bool newRecord = false;

    private int currentCoins = 0;

    private SKIN sKIN;


    public Vector2 MaxScreenBounds { get { return Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z)); } }
    public Vector2 MinScreenBounds { get { return Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.transform.position.z)); } }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            int coins = PlayerPrefs.GetInt(totalCoins, -1);
            if (coins > 0)
            {
                currentCoins = coins;
            }
            sKIN = (SKIN)PlayerPrefs.GetInt(currentSkinName, (int)SKIN.BIRD);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(SCENE _sCENE)
    {
        SceneManager.LoadScene((int)_sCENE);
    }

    public void CloseApp()
    {
        Application.Quit();
    }

    public void SetPlayerPoints(float _distance, int _numCoins)
    {
        currentPlayerPoints.distance = _distance;
        currentPlayerPoints.numCoins = _numCoins;
        currentPlayerPoints.totalPoints = (int)(_distance * _numCoins);

        currentCoins += _numCoins;
        PlayerPrefs.SetInt(totalCoins, currentCoins);

        PlayerPoints record = GetRecordPlayerPoints();
        if (record.totalPoints < currentPlayerPoints.totalPoints)
        {
            SetRecord(currentPlayerPoints);
        }

        PlayerPrefs.Save();
    }

    public bool ExistRecord()
    {
        return PlayerPrefs.GetFloat(distanceName, -1.0f) > 0.0f;
    }

    public PlayerPoints GetRecordPlayerPoints()
    {
        PlayerPoints pP = new PlayerPoints
        {
            distance = PlayerPrefs.GetFloat(distanceName, -1.0f),
            totalPoints = PlayerPrefs.GetInt(totalPointsName, -1),
            numCoins = PlayerPrefs.GetInt(numCoinsName, -1)
        };

        return pP;
    }

    // TODO: Agregar esta info a la BBDD
    private void SetRecord(PlayerPoints _playerPoints)
    {
        PlayerPrefs.SetFloat(distanceName, _playerPoints.distance);
        PlayerPrefs.SetFloat(totalPointsName, _playerPoints.totalPoints);
        PlayerPrefs.SetInt(numCoinsName, _playerPoints.numCoins);
        PlayerPrefs.Save();
        newRecord = true;
    }


    public PlayerPoints GetPlayerPoints()
    {
        return currentPlayerPoints;
    }

    public bool ThereIsNewRecord()
    {
        return newRecord;
    }

    public int GetCurrentNumOfCoins()
    {
        return currentCoins;
    }

    public SKIN GetSkin()
    {
        return sKIN;
    }

    public void SetSkin(SKIN bIRD)
    {
        sKIN = bIRD;
    }

    public bool IsThisSkinUnlock(SKIN _sKIN)
    {
        if (_sKIN.Equals(SKIN.ARMOR))
        {
            return PlayerPrefs.GetInt(armorSkinName, -1) > 0;
        }
        else if (_sKIN.Equals(SKIN.TANK))
        {
            return PlayerPrefs.GetInt(tankSkinName, -1) > 0;
        }
        return true;
    }

    public void UnlockSkin(SKIN _sKIN, int _numCoins)
    {
        currentCoins -= _numCoins;
        PlayerPrefs.SetInt(totalCoins, currentCoins);
        if (_sKIN.Equals(SKIN.ARMOR))
        {
            PlayerPrefs.SetInt(armorSkinName, 1);
            sKIN = SKIN.ARMOR;
        }
        else if (_sKIN.Equals(SKIN.TANK))
        {
            PlayerPrefs.SetInt(tankSkinName, 1);
            sKIN = SKIN.TANK;
        }
        
        PlayerPrefs.Save();
    }

    public bool CanBuyIt(int _numCoins)
    {
        return currentCoins >= _numCoins;
    }
}
