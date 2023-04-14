using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PhotoManager : MonoBehaviour
{
    public KeyCode cameraKey = KeyCode.R;
    public KeyCode takePhotoKey = KeyCode.Space;
    public KeyCode inventoryKey = KeyCode.I;

    public GameObject cameraPanel;
    public Camera photoCamera;
    public RawImage photoPreview;

    public GameObject inventoryPanel;
    public GameObject[] inventorySlots;
    public RawImage[] inventoryIcons;
    public Texture2D[] inventoryTextures;

    private bool isTakingPhoto = false;
    private int maxPhotos = 10;
    private int numPhotosTaken = 0;
    private List<Texture2D> photos = new List<Texture2D>();

    void Start()
    {
        cameraPanel.SetActive(false);
        inventoryPanel.SetActive(false);
    }

    void Update()
    {
        // Open camera view
        if (Input.GetKeyDown(cameraKey))
        {
            cameraPanel.SetActive(true);
        }

        // Close camera view
        if (Input.GetKeyDown(cameraKey))
        {
            cameraPanel.SetActive(false);
            photoPreview.texture = null;
        }

        // Take photo
        if (Input.GetKeyDown(takePhotoKey))
        {
            if (!isTakingPhoto && numPhotosTaken < maxPhotos)
            {
                StartCoroutine(TakePhoto());
            }
        }

        // Open inventory
        if (Input.GetKeyDown(inventoryKey))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }

    private IEnumerator TakePhoto()
    {
        isTakingPhoto = true;

        yield return new WaitForEndOfFrame();

        Texture2D photo = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

        photo.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);

        photo.Apply();

        /*byte[] bytes = photo.EncodeToPNG();

        string filePath = Application.persistentDataPath + "/photo" + numPhotosTaken + ".png";
        File.WriteAllBytes(filePath, bytes);*/

        photos.Add( photo);

        numPhotosTaken++;

        Debug.Log("Photo saved");

        isTakingPhoto = false;

        if (numPhotosTaken <= maxPhotos)
        {
            //inventoryTextures[numPhotosTaken - 1] = photo;
            inventoryIcons[numPhotosTaken - 1].texture = photo;
            inventorySlots[numPhotosTaken - 1].SetActive(true);
        }
        /*else
        {
            DeletePhoto(0);
            inventoryTextures[maxPhotos - 1] = photo;
            inventoryIcons[maxPhotos - 1].texture = photo;
            inventorySlots[maxPhotos - 1].SetActive(true);
        }*/
    }

    private void DeletePhoto(int index)
    {
        Texture2D photoToDelete = photos[index];
        photos[index] = null;
        inventoryTextures[index] = null;
        inventoryIcons[index].texture = null;
        inventorySlots[index].SetActive(false);
        numPhotosTaken--;

        string filePath = Application.persistentDataPath + "/photo" + index + ".png";
        File.Delete(filePath);

        for (int i = index + 1; i <= numPhotosTaken; i++)
        {
            photos[i - 1] = photos[i];
            inventoryTextures[i - 1] = inventoryTextures[i];
            inventoryIcons[i - 1].texture = inventoryIcons[i].texture;
            string oldPath = Application.persistentDataPath + "/photo" + i + ".png";
            string newPath = Application.persistentDataPath + "/photo" + (i - 1) + ".png";
            File.Move(oldPath, newPath);
        }
    }
}
