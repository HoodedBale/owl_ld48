using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreviceSpawner : MonoBehaviour
{
    [Header("Generation Variables")]
    public int layers = 50;
    public int bottomLayers = 3;
    public float layerOffset = 0.25f;
    public Vector3 tileOffset;
    public float startingHoleSize = 10;
    public float minimumHole = 5;
    public float finalHoleSize = 2;
    public int alterChance = 30;
    public List<int> layerLimit;
    public int generationThreshold;

    [Header("Prefabs")]
    public List<GameObject> tilePrefabs;

    List<GameObject> m_spawnedTiles;
    System.Random m_rand;
    int m_randSeed;

    public static float minimumY = 0;
    public static float generationProgress = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_spawnedTiles = new List<GameObject>();
        m_randSeed = new System.Random().Next();
        m_rand = new System.Random(m_randSeed);
        StartCoroutine(DrawCrevice());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void BuildCrevice()
    {

    }

    IEnumerator DrawCrevice()
    {
        int threshold = 0;
        int tileId = 0;
        float currentY = transform.position.y;
        float leftDirection = -1;
        float rightDirection = 1;
        float currentLeft = transform.position.x - (startingHoleSize / 2);
        float currentRight = transform.position.x + (startingHoleSize / 2);

        for (int i = 0; i < layers; ++i)
        {
            float xLeft = currentLeft;
            float xRight = currentRight;
            float z = 0;
            bool first = true;

            tileId = 0;
            for(int j = 0; j < layerLimit.Count; ++j)
            {
                if(i < layerLimit[j])
                {
                    tileId = j;
                    break;
                }
            }

            while(xLeft > (-Screen.width / 2) / 100.0f - 1)
            {
                GameObject temp = Instantiate(tilePrefabs[tileId]);
                temp.transform.position = new Vector3(xLeft, currentY, z);
                if (leftDirection > 0) temp.transform.localScale = new Vector3(1, -1, 1);
                else temp.transform.localScale = new Vector3(1, 1, 1);
                temp.transform.SetParent(transform);
                if (!first && temp.GetComponent<EdgeCollider2D>() && i > 0) Destroy(temp.GetComponent<EdgeCollider2D>());
                m_spawnedTiles.Add(temp);

                temp = Instantiate(tilePrefabs[tileId]);
                temp.transform.position = new Vector3(xRight, currentY, z);
                if (rightDirection < 0) temp.transform.localScale = new Vector3(-1, -1, 1);
                else temp.transform.localScale = new Vector3(-1, 1, 1);
                temp.transform.SetParent(transform);
                if (!first && temp.GetComponent<EdgeCollider2D>() && i > 0) Destroy(temp.GetComponent<EdgeCollider2D>());
                m_spawnedTiles.Add(temp);

                xLeft -= tileOffset.x;
                xRight += tileOffset.x;
                z += 0.1f;

                ++threshold;
                if(threshold >= generationThreshold)
                {
                    threshold = 0;
                    yield return null;
                }
                first = false;
            }

            currentY -= tileOffset.y;

            if(m_rand.Next(0, 100) < alterChance || (transform.position.x - currentLeft < minimumHole / 2 && leftDirection > 0) || (currentLeft < -Screen.width / 2 + 1 && leftDirection < 0))
            {
                leftDirection *= -1;
            }
            else
            {
                currentLeft -= leftDirection * layerOffset;
            }

            if (m_rand.Next(0, 100) < alterChance || (transform.position.x - currentRight > -minimumHole / 2 && rightDirection < 0) || (currentRight > Screen.width / 2 + 1 && rightDirection > 0))
            {
                rightDirection *= -1;
            }
            else
            {
                currentRight -= rightDirection * layerOffset;
            }

            generationProgress = (float)i / (float)layers;
        }

        tileId = tilePrefabs.Count - 1;

        for (int i = 0; i < bottomLayers; ++i)
        {
            float xLeft = transform.position.x - finalHoleSize / 2;// + i * layerOffset;
            float xRight = transform.position.x + finalHoleSize / 2;// - i * layerOffset;
            float z = 0;

            while (xLeft > (-Screen.width / 2) / 100.0f - 1)
            {
                GameObject temp = Instantiate(tilePrefabs[tileId]);
                temp.transform.position = new Vector3(xLeft, currentY, z);
                temp.transform.SetParent(transform);
                m_spawnedTiles.Add(temp);

                temp = Instantiate(tilePrefabs[tileId]);
                temp.transform.position = new Vector3(xRight, currentY, z);
                temp.transform.localScale = new Vector3(-2, 1, 1);
                temp.transform.SetParent(transform);
                m_spawnedTiles.Add(temp);

                xLeft -= tileOffset.x;
                xRight += tileOffset.x;
                z += 0.1f;

                ++threshold;
                if (threshold >= generationThreshold)
                {
                    threshold = 0;
                    yield return null;
                }
            }

            currentY -= tileOffset.y;
        }

        minimumY = currentY + tileOffset.y * 7;
    }
}
