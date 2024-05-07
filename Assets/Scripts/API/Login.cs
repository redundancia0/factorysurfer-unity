using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

public static class Login
{

    [System.Serializable]
    public class LoginResponse
    {
        public string message;
        public LoginUserData data;
    }

    [System.Serializable]
    public class LoginUserData
    {
        public string _id;
        public int monedas;
        public int rango;
        public int puntuacion;
        public string nombre;
        public string correo;
        public string avatar;
        public string clave;
        public string __v;
    }




    [System.Serializable]
    public class GuardarPartidaResponse
    {
        public string status;
        public string message;
        public PartidaUserData data;
    }
    [System.Serializable]
    public class PartidaUserData
    {
        public string usuario_id;
        public int puntuacion;
        public int monedas;
        public string fecha;
        public string _id;
        public int __v;
    }



    [System.Serializable]
    public class LeaderBoardData
    {
        public string status;
        public string message;
        public LeaderBoardPuntuacionData[] data;
    }

    [System.Serializable]
    public class LeaderBoardPuntuacionData
    {
        public string usuario_id;
        public string nombre;
        public string correo;
        public int puntuacion;
    }


    private static Sprite _userAvatar = null;
    public static Sprite UserAvatar { get { return _userAvatar; } }


    private static LoginUserData _userData;
    public static LoginUserData USER_DATA
    {
        get { return _userData; }
        private set
        { 
            _userData = value;
            if (_userData != null)
                OnSetUserData?.Invoke();
        }
    }

    private static LeaderBoardData _leaderBoardData;
    public static LeaderBoardData LEADERBOARD_DATA
    {
        get { return _leaderBoardData; }
        private set
        {
            _leaderBoardData = value;
            if (_leaderBoardData != null)
                OnSetLeaderBoardData?.Invoke();
            else
                Debug.LogWarning("_leaderBoardData = NULL");
        }
    }



    public static UnityEvent OnSetUserData = new UnityEvent();
    public static UnityEvent OnSetLeaderBoardData = new UnityEvent();
    public static UnityEvent OnLogin = new UnityEvent();
    public static UnityEvent<SKIN, int, Button, TMP_Text> OnBuy = new UnityEvent<SKIN, int, Button, TMP_Text>();

    public static IEnumerator SendLoginRequest(string usuario, string contrasena)
    {
        string url = "http://redundancia0.duckdns.org:8080/api/usuarios/login"; // Reemplaza con tu URL real

        // Crear formulario para enviar los datos de inicio de sesi�n
        WWWForm form = new WWWForm();
        form.AddField("nombre", usuario);
        form.AddField("clave", contrasena);

        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = www.downloadHandler.text;
            LoginResponse response = JsonUtility.FromJson<LoginResponse>(jsonResponse);
            if (response.message == "Inicio de sesión exitoso") {
                USER_DATA = response.data;
                OnLogin?.Invoke();
            } else {
                USER_DATA = null;
            }
        }
    }


    public static IEnumerator BuySomething(SKIN _sKIN, int _numCoins, Button button, TMP_Text coinsText)
    {
        string url = "http://redundancia0.duckdns.org:8080/api/usuarios/restarMonedas/findbyid/" + USER_DATA._id; // Reemplaza con tu URL real

        WWWForm form = new WWWForm();
        if (USER_DATA != null)
        {
            form.AddField("monedas", _numCoins);

            UnityWebRequest www = UnityWebRequest.Post(url, form);
            yield return www.SendWebRequest();
        }
        else
            Debug.LogWarning("Primero Logeate");

        OnBuy?.Invoke(_sKIN, _numCoins, button, coinsText);
    }


    public static IEnumerator OnGameEnd(int puntuacion, int monedas)
    {
        string url = "http://redundancia0.duckdns.org:8080/api/partidas/insertarPartida"; // Reemplaza con tu URL real

        // Crear formulario para enviar los datos de inicio de sesi�n
        WWWForm form = new WWWForm();

        if(USER_DATA != null)
        {
            form.AddField("usuario_id", USER_DATA._id);
            form.AddField("puntuacion", puntuacion);
            form.AddField("monedas", monedas);

            UnityWebRequest www = UnityWebRequest.Post(url, form);
            yield return www.SendWebRequest();
        }

        Debug.LogWarning("Primero Logeate");
    }

    public static IEnumerator GetLeaderBoard()
    {
        string url = "http://redundancia0.duckdns.org:8080/api/partidas/top";

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                // Assign the response directly to userId
                string jsonResponse = www.downloadHandler.text;
                Debug.Log("=== GetLeaderBoard ===\n" + jsonResponse);
                LeaderBoardData response = JsonUtility.FromJson<LeaderBoardData>(jsonResponse);
                LEADERBOARD_DATA = response;
                yield return null;
            }
            else
            {
                // Request failed, log the error
                Debug.LogError("User id request failed. Error: " + www.error);
            }
        }
    }


    public static IEnumerator GetUserSprite()
    {
        if (_userData != null)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(USER_DATA.avatar);
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(www);
                if (texture == null)
                    Debug.LogWarning("User Avatar = NULL");

                _userAvatar = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
            }
        }
        else Debug.LogWarning("USER_DATA = NULL" );
    }

}