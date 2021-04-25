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
    public int minimumObstacleDistance = 5;
    public List<int> layerLimit;
    public int generationThreshold;

    [Header("Prefabs")]
    public List<GameObject> tilePrefabs;
    public List<GameObject> obstacles;
    public GameObject backPrefab;

    List<GameObject> m_spawnedTiles;
    System.Random m_rand;
    int m_randSeed;
    int m_currentObsDist;

    public static float minimumY = 0;
    public static float generationProgress = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_spawnedTiles = new List<GameObject>();
        m_randSeed = new System.Random().Next();
        m_rand = new System.Random(m_randSeed);
        m_currentObsDist = 0;
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

            if(i > 0)
            {
                GameObject back = Instantiate(backPrefab);
                back.transform.position = Vector3.zero + new Vector3(0, 1, 0) * currentY + new Vector3(0, 0, 10);
                back.transform.SetParent(transform);
            }

            while(xLeft > (-Screen.width / 2) / 10.0f - 30)
            {
                GameObject temp = null;
                if(xLeft < -minimumHole / 2)
                { 
                    temp = Instantiate(tilePrefabs[tileId]);
                    temp.transform.position = new Vector3(xLeft, currentY, z);
                    if (leftDirection > 0) temp.transform.localScale = new Vector3(1, -1, 1);
                    else temp.transform.localScale = new Vector3(1, 1, 1);
                    temp.transform.SetParent(transform);
                    if (!first && temp.GetComponent<EdgeCollider2D>() && i > 0) Destroy(temp.GetComponent<EdgeCollider2D>());
                    if (first && m_currentObsDist > minimumObstacleDistance && m_rand.Next(0, 2) > 0)
                    {
                        GameObject obst = Instantiate(obstacles[m_rand.Next(0, obstacles.Count)]);
                        obst.transform.position = temp.transform.position + new Vector3(0, 0, 1);
                        m_currentObsDist = 0;
                    }
                    m_spawnedTiles.Add(temp);
                }

                if(xRight > minimumHole / 2)
                {
                    temp = Instantiate(tilePrefabs[tileId]);
                    temp.transform.position = new Vector3(xRight, currentY, z);
                    if (rightDirection < 0) temp.transform.localScale = new Vector3(-1, -1, 1);
                    else temp.transform.localScale = new Vector3(-1, 1, 1);
                    temp.transform.SetParent(transform);
                    if (!first && temp.GetComponent<EdgeCollider2D>() && i > 0) Destroy(temp.GetComponent<EdgeCollider2D>());
                    if (first && m_currentObsDist > minimumObstacleDistance && m_rand.Next(0, 2) > 0)
                    {
                        GameObject obst = Instantiate(obstacles[m_rand.Next(0, obstacles.Count)]);
                        obst.transform.position = temp.transform.position + new Vector3(0, 0, 1);
                        obst.transform.localScale = new Vector3(-1, 1, 1);
                        m_currentObsDist = 0;
                    }
                    m_spawnedTiles.Add(temp);
                }

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
            ++m_currentObsDist;

            if(m_rand.Next(0, 100) < alterChance && ((currentLeft < -minimumHole / 2) || (currentLeft > -Screen.width / 2 + 1)))
            {
                leftDirection *= -1;
            }
            else if(currentLeft > -minimumHole / 2 && leftDirection > 0)
            {
                leftDirection = -1;
            }
            else if(currentLeft < -Screen.width / 2 + 1 && leftDirection < 0)
            {
                leftDirection = 1;
            }
            else
            {
                currentLeft -= leftDirection * layerOffset;
            }

            if (m_rand.Next(0, 100) < alterChance && ((currentRight > minimumHole / 2) || (currentRight < Screen.width / 2 + 1)))
            {
                rightDirection *= -1;
            }
            else if(currentRight < minimumHole / 2 && rightDirection < 0)
            {
                rightDirection = 1;
            }
            else if (currentRight > Screen.width / 2 + 1 && rightDirection > 0)
            {
                rightDirection = 1;
            }
            else
            {
                currentRight -= rightDirection * layerOffset;
            }

            generationProgress = (float)(i + 1) / (float)layers;
        }

        tileId = tilePrefabs.Count - 1;
        bool firstFloor = true;
        float middle = m_rand.Next(-5, 5);
        
        for (int i = 0; i < bottomLayers; ++i)
        {
            float xLeft = transform.position.x - finalHoleSize / 2;// + i * layerOffset;
            float xRight = transform.position.x + finalHoleSize / 2;// - i * layerOffset;
            float z = 0;

            GameObject back = Instantiate(backPrefab);
            back.transform.SetParent(transform);
            back.transform.position = Vector3.zero + new Vector3(0, 1, 0) * currentY + new Vector3(0, 0, 10);

            while (xLeft > (-Screen.width / 2) / 10.0f - 30)
            {
                GameObject temp = Instantiate(tilePrefabs[tileId]);
                temp.transform.position = new Vector3(xLeft + middle, currentY, z);
                temp.transform.SetParent(transform);
                if (!firstFloor) temp.tag = "Untagged";
                m_spawnedTiles.Add(temp);

                temp = Instantiate(tilePrefabs[tileId]);
                temp.transform.position = new Vector3(xRight + middle, currentY, z);
                temp.transform.localScale = new Vector3(-2, 1, 1);
                temp.transform.SetParent(transform);
                if (!firstFloor) temp.tag = "Untagged";
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
            firstFloor = false;
        }

        minimumY = currentY + tileOffset.y * 5;
    }
}
