using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardUI : MonoBehaviour
{
    [SerializeField]
    private GameObject leaderBoardList;
    [SerializeField]
    private RecordRowUI recordRowPrefab;

    private List<RecordRowUI> recordsInstanced = new List<RecordRowUI>();


    private void OnEnable()
    {
        Login.OnSetLeaderBoardData.AddListener(AddRecords);
        ClearRecords();
        StartCoroutine(Login.GetLeaderBoard());
    }


    private void OnDisable()
    {
        Login.OnSetLeaderBoardData.RemoveListener(AddRecords);
    }

    private void ClearRecords()
    {
        foreach (RecordRowUI go in recordsInstanced)
            Destroy(go.gameObject);

        recordsInstanced.Clear();
    }

    private void AddRecords()
    {
        RectTransform parentRectTransform = leaderBoardList.GetComponent<RectTransform>();
        for (int i = 0; i < Login.LEADERBOARD_DATA.data.Length; ++i)
        {
            RecordRowUI aux = Instantiate(recordRowPrefab);
            RectTransform rect = aux.GetComponent<RectTransform>();
            rect.parent = parentRectTransform;
            aux.UserText.text = Login.LEADERBOARD_DATA.data[i].nombre;
            aux.PointsText.text = Login.LEADERBOARD_DATA.data[i].puntuacion.ToString();

            recordsInstanced.Add(aux);
        }
    }
}
