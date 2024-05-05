using TMPro;
using UnityEngine;

public class RecordRowUI : MonoBehaviour
{
    [SerializeField]
    TMP_Text _userText, _pointsText;

    public TMP_Text UserText { get {  return _userText; } }
    public TMP_Text PointsText { get {  return _pointsText; } }
}
