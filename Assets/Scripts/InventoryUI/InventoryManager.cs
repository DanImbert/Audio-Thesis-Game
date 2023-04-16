using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public bool Horizontal = false;
    public Slider hSlider;
    public Transform transformParent;
    public GameObject copyObject;
    public List<Texture2D> textureList = new List<Texture2D>();

    public GameObject SelectionPolaroid;
    public RawImage SelectedRawTexture;

    private void Awake()
    {
        if (copyObject != null)
            copyObject.SetActive(false);
        CalcMaxEntries();
    }
    private void OnEnable()
    {
    }
    private void OnDisable()
    {
        SelectionPolaroid.SetActive(false);
    }
    #region Max Entries
    protected int nMaxEntries = 10;
    protected virtual int CalcMaxEntries()
    {
        if (Horizontal)
            return Mathf.FloorToInt(GetComponent<RectTransform>().rect.width / copyObject.GetComponent<RectTransform>().rect.width);
        return Mathf.FloorToInt(GetComponent<RectTransform>().rect.height / copyObject.GetComponent<RectTransform>().rect.height);
    }
    #endregion
    #region Slider And Scrollbar
    protected virtual void EnableDisableSlider(bool valeu)
    {
        hSlider.value = 0;
        hSlider.gameObject.SetActive(valeu);
    }
    public virtual float ScrollListStep()
    {
        return (textureList.Count - nMaxEntries) * copyObject.GetComponent<RectTransform>().rect.height;
    }
    public virtual void ScrollList(int entry)
    {
        ScrollList(ScrollListStep() * entry);
    }
    public virtual void ScrollList(float nValue)
    {
        if (transformParent.TryGetComponent<RectTransform>(out RectTransform rect))
        {
            if (Horizontal)
                rect.anchoredPosition = new Vector2(-copyObject.GetComponent<RectTransform>().rect.width * nValue * (textureList.Count - nMaxEntries), rect.anchoredPosition.y);
            else
                rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, copyObject.GetComponent<RectTransform>().rect.height * nValue * (textureList.Count - nMaxEntries));
        }
    }
    #endregion
    #region List Population
    protected virtual void PopulateList()
    {
        ClearList();
        foreach (Texture2D texture in textureList)
        {
            GeneratePolaroidButton(texture);
        }
        EnableDisableSlider(textureList.Count >= nMaxEntries);
    }
    protected virtual void GeneratePolaroidButton(Texture2D texture)
    {
        Button btn = PoolButton();
        btn.gameObject.SetActive(true);
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener
            (() =>
            {
                OnTextureSelected(texture);
            });
    }
    protected virtual Button PoolButton()
    {
        foreach (Transform child in transformParent)
        {
            if (!child.gameObject.activeSelf && child.TryGetComponent<Button>(out Button heroBtn))
            {
                return heroBtn;
            }
        }
        GameObject nGO = GameObject.Instantiate(copyObject);
        nGO.name = "Entry " + transformParent.childCount;
        nGO.transform.SetParent(transformParent);
        if (nGO.TryGetComponent<RectTransform>(out RectTransform rectTransform))
        {
            if (Horizontal)
            {
                rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, 0);
                rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, 0);
                rectTransform.anchoredPosition = copyObject.GetComponent<RectTransform>().anchoredPosition + new Vector2(rectTransform.rect.width * (transformParent.childCount - 1), 0);
            }
            else
            {
                rectTransform.offsetMin = new Vector2(0, rectTransform.offsetMin.y);
                rectTransform.offsetMax = new Vector2(0, rectTransform.offsetMax.y);
                rectTransform.anchoredPosition = copyObject.GetComponent<RectTransform>().anchoredPosition - new Vector2(0, rectTransform.rect.height * (transformParent.childCount - 1));
            }
        }
        return nGO.GetComponent<Button>();
    }
    public virtual void OnTextureSelected(Texture2D texture)
    {
        SelectionPolaroid.SetActive(true);
    SelectedRawTexture.texture = texture;
}
    public virtual 
        void PushTexture(Texture2D texture)
    {
        textureList.Add(texture);
        GeneratePolaroidButton(texture);
        EnableDisableSlider(textureList.Count >= nMaxEntries);
    }
    public virtual void ClearList()
    {
        foreach (Transform child in transformParent)
        {
            child.gameObject.SetActive(false);
        }
    }
    #endregion
}
