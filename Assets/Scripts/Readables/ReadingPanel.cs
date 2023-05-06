using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadingPanel : MonoBehaviour
{
    public ObjectPool readablePool;

    public static ReadingPanel main;

    private void Awake()
    {
        if (readablePool == null)
            readablePool = GetComponent<ObjectPool>();
        main = this;
    }
    
    public void ReadItem(GameObject prefab)
    {
        if (MainPanel != null)
            Close();
        MainPanel = readablePool.PoolItem(prefab);
        if (MainPanel.TryGetComponent(out RectTransform rt))
        {
            rt.anchoredPosition = Vector2.zero;
        }
        LockCursor(false);
    }
    public GameObject MainPanel;
    public void Close()
    {
        readablePool.DeactivateObject(MainPanel);
        LockCursor(true);
    }
    #region Cursor
    public void LockCursor(bool locked )//Should be handled in its own component
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }

    #endregion
    #region Item 
    public TMPro.TextMeshProUGUI selectedReadableTooltip;

    #endregion
}
