using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadablePanel : MonoBehaviour
{
    private void OnEnable()
    {
        SetTextEnabled(false);
    }
    #region Aid Text
    public TMPro.TextMeshProUGUI assistText;
    public void ToggleText()
    {
        if (assistText!=null)
        {
            assistText.gameObject.SetActive(!assistText.gameObject.activeSelf);
        }
    }
    public void SetTextEnabled(bool value)
    {
        if (assistText != null)
        {
            assistText.gameObject.SetActive(value);
        }
    }
    #endregion
    #region Close
    public void ClosePanel()
    {
        ReadingPanel.main?.Close();
    }
    #endregion
}
