using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderObject : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float range = 10f;
    [SerializeField] float delay = 1f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask occluderLayer;
    [SerializeField] List<GameObject> prefabs;

    //Transform target;
    bool isDelayComplete;
    BoxCollider playZone;

    void Start()
    {
        StartCoroutine(DelayCoroutine());
        playZone = GameObject.FindWithTag("PlayZone").GetComponent<BoxCollider>();
        //target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    IEnumerator DelayCoroutine()
    {
        yield return new WaitForSeconds(delay);
        isDelayComplete = true;
    }

    void Update()
    {
        if (!isDelayComplete) return;

        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);

        if ((0f <= viewPos.x && viewPos.x <= 1f) && (0f <= viewPos.y && viewPos.y <= 1f))
        {
            // Object is in the camera view, do nothing
            return;
        }
        else
        {
            Vector3 randomPosition;

            if (viewPos.x < 0f)
            {
                randomPosition = Camera.main.ViewportToWorldPoint(new Vector3(0f, Random.value, Camera.main.transform.position.z + range));
            }
            else
            {
                randomPosition = Camera.main.ViewportToWorldPoint(new Vector3(1f, Random.value, Camera.main.transform.position.z + range));
            }

            randomPosition.y = transform.position.y;

            RaycastHit hit;
            if (Physics.Raycast(randomPosition, Vector3.down, out hit, Mathf.Infinity, groundLayer))
            {
                float groundHeight = hit.point.y + (transform.localScale.y / 2f);
                randomPosition.y = Mathf.Max(randomPosition.y, groundHeight);

                Vector3 toCamera = Camera.main.transform.position - randomPosition;
                if (Physics.Raycast(randomPosition, toCamera, out hit, toCamera.magnitude, occluderLayer))
                {
                    Debug.Log($"Object is occluded by {hit.collider.gameObject.name}");
                }
                else
                {
                    transform.position = Vector3.Lerp(transform.position, randomPosition, Time.deltaTime * speed);
                }
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, randomPosition, Time.deltaTime * speed);
            }

            // Check if the object is out of the play zone
            if (!playZone.bounds.Contains(transform.position))
            {
                // Set position to the closest point inside the play zone
                transform.position = playZone.ClosestPoint(transform.position);
            }
        }
    }

}

