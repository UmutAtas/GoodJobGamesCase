using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridManager : MonoBehaviour
{
    [NonSerialized] public int width; //x
    [NonSerialized] public int height; //y
    public GameObject[,] tileGrid;
    public List<GameObject> tileToSpawn = new List<GameObject>();

    private void Awake()
    {
        width = Random.Range(2, 11);
        height = Random.Range(2, 11);
    }

    private void Start()
    {
        tileGrid = new GameObject[width, height];
        SetGrid();
        StartCoroutine(CheckForAllMatches());
    }

    private void SetGrid()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 tempPos = new Vector2(transform.position.x + i, transform.position.y + j);
                GameObject tile = Instantiate(tileToSpawn[Random.Range(0, tileToSpawn.Count)], tempPos,
                    Quaternion.identity, transform);
                tile.name = "( " + i + ", " + j + " )";
                tileGrid[i, j] = tile;
            }
        }
    }
    
    [SerializeField] private float tileRotateAmount;
    [SerializeField] private float tileRotateDuration;
    [SerializeField] private GameObject explosionParticle;

    private void DestroyMatchesAt(int row, int column)
    {
        var tile = tileGrid[row, column].GetComponent<Tile>();
        if (tile.isPressed && tile.currentMatches.Count > 1)
        {
            foreach (var t in tile.currentMatches)
            {
                if (t != null)
                {
                    int x = t.GetComponent<Tile>().row;
                    int y = t.GetComponent<Tile>().column;
                    if (tileGrid[x, y] != null)
                    {
                        Instantiate(explosionParticle, t.transform.position, Quaternion.identity, transform);
                        Destroy(tileGrid[x, y]);
                        tileGrid[x, y] = null;
                    }
                }
            }
            tile.currentMatches.Clear();
            StartCoroutine(DecreaseColumnRoutine());
        }
        //No matches animation
        else if (tile.isPressed)
        {
            DOTween.Sequence().Append(tile.transform.DORotate(Vector3.forward * tileRotateAmount, tileRotateDuration/3).SetEase(Ease.Linear))
                .Append(tile.transform.DORotate(Vector3.forward * (-tileRotateAmount), tileRotateDuration/3).SetEase(Ease.Linear))
                .Append(tile.transform.DORotate(Vector3.zero, tileRotateDuration/3).SetEase(Ease.Linear));
        }
    }
    
    public void DestroyMatches()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (tileGrid[i, j] != null)
                    DestroyMatchesAt(i, j);
            }
        }
    }

    public IEnumerator DecreaseColumnRoutine()
    {
        int nullCount = 0;
        
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (tileGrid[i, j] == null)
                    nullCount++;
                
                else if (nullCount > 0 && tileGrid[i, j] != null)
                {
                    tileGrid[i, j].GetComponent<Tile>().column -= nullCount;
                    StartCoroutine(tileGrid[i, j].GetComponent<Tile>().MovePiecesRoutine());
                    tileGrid[i, j].GetComponent<Tile>().currentMatches.Clear();
                    tileGrid[i, j] = null;
                }
            }

            nullCount = 0;
        }
        yield return new WaitForSeconds(0.3f);
        RefillBoard();
    }

    private void RefillBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (tileGrid[i, j] == null)
                {
                    //Create new tile
                    Vector2 tempPos = new Vector2(transform.position.x + i, transform.position.y + j);
                    GameObject newTile = Instantiate(tileToSpawn[Random.Range(0, tileToSpawn.Count)], tempPos,
                        Quaternion.identity, transform);
                    newTile.name = "( " + i + ", " + j + " )";
                    tileGrid[i, j] = newTile;
                    //Make tile slide down
                    newTile.transform.position += Vector3.up * 5;
                    if (newTile.GetComponent<Tile>() != null)
                        StartCoroutine(newTile.GetComponent<Tile>().MovePiecesRoutine());
                }
                //Clear matches list from existing tile
                else 
                    tileGrid[i,j].GetComponent<Tile>().currentMatches.Clear();
            }
        }

        StartCoroutine(CheckForAllMatches());
    }

    private int matchCount;

    private IEnumerator CheckForAllMatches()
    {
        matchCount = 0;
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (tileGrid[i, j] != null)
                {
                    if (tileGrid[i, j].GetComponent<Tile>().currentMatches.Count > 0)
                    {
                        matchCount++;
                    }   
                }
            }
        }
    }

    public void ShuffleGrid()
    {
        if (matchCount == 0)
        {
            foreach (var tile in tileGrid)
            {
                Destroy(tile);
            }
            SetGrid();
            StartCoroutine(CheckForAllMatches());
        }
    }
}
