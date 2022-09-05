using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
   private GridManager _gridManager;

   [NonSerialized] public int column;
   [NonSerialized] public int row;
   [NonSerialized] public int targetX;
   [NonSerialized] public int targetY;
   private Vector2 tempPosition;
   [SerializeField] private float tileMoveSpeed;

   [NonSerialized] public bool isPressed = false;

   public List<GameObject> currentMatches = new List<GameObject>();
   

   private void Awake()
   {
      row = (int)transform.localPosition.x;
      column = (int)transform.localPosition.y;
   }

   private void Start()
   {
      _gridManager = FindObjectOfType<GridManager>();
   }

   private void Update()
   {
      targetX = row;
      targetY = column;
      FindNeighbours();
   }

   public IEnumerator MovePiecesRoutine()
   {
      yield return new WaitForEndOfFrame();
      //Move towards target
      while (Mathf.Abs(targetY - transform.localPosition.y) > 0.1f)
      {
         tempPosition = new Vector2(transform.localPosition.x, targetY);
         transform.localPosition = Vector2.Lerp(transform.localPosition, tempPosition, tileMoveSpeed);
         yield return new WaitForEndOfFrame();
         //Set grid position
         if ( _gridManager.tileGrid[row, column] != this.gameObject)
            _gridManager.tileGrid[row, column] = this.gameObject;
      }
      //Directly set position
      tempPosition = new Vector2(transform.localPosition.x, targetY);
      transform.localPosition = tempPosition;
   }
   
   private void FindNeighbours()
   {
      List<GameObject> neighbourTiles = new List<GameObject>();
      //Check from middle tiles
     if (row > 0 && row < _gridManager.width - 1)
      {
         GameObject leftTile = _gridManager.tileGrid[row - 1, column];
         GameObject rightTile = _gridManager.tileGrid[row + 1, column];
         if(!neighbourTiles.Contains(leftTile))
            neighbourTiles.Add(leftTile);
         if(!neighbourTiles.Contains(rightTile))
            neighbourTiles.Add(rightTile);
      }

      if (column > 0 && column < _gridManager.height -1)
      {
         GameObject downTile = _gridManager.tileGrid[row, column - 1];
         GameObject upTile = _gridManager.tileGrid[row, column + 1];
         if(!neighbourTiles.Contains(downTile))
            neighbourTiles.Add(downTile);
         if(!neighbourTiles.Contains(upTile))
            neighbourTiles.Add(upTile);
      }
      //Check from edge tiles
      if (row == 0)
      {
         GameObject rightTile = _gridManager.tileGrid[row + 1, column];
         if (!neighbourTiles.Contains(rightTile))
            neighbourTiles.Add(rightTile);
      }
      
      if (row == _gridManager.width -1)
      {
         GameObject leftTile = _gridManager.tileGrid[row - 1, column];
         if (!neighbourTiles.Contains(leftTile))
            neighbourTiles.Add(leftTile);
      }
      
      if (column == 0)
      {
         GameObject upTile = _gridManager.tileGrid[row, column + 1];
         if (!neighbourTiles.Contains(upTile))
            neighbourTiles.Add(upTile);
      }
      
      if (column == _gridManager.height - 1)
      {
         GameObject downTile = _gridManager.tileGrid[row, column - 1];
         if (!neighbourTiles.Contains(downTile))
            neighbourTiles.Add(downTile);
      }

      foreach (var tile in neighbourTiles)
      {
         GetLocalMatches(tile);
      }
   }

   private void GetLocalMatches(GameObject tile)
   {
      if (tile != null && tile.layer == gameObject.layer)
      {
         if (!currentMatches.Contains(tile))
            currentMatches.Add(tile);
         
         if(!currentMatches.Contains(gameObject))
            currentMatches.Add(gameObject);
         // Get list from middle tiles
         List<GameObject> neighboursLists = tile.GetComponent<Tile>().currentMatches;
         foreach (var var in neighboursLists)
         {
            if (!currentMatches.Contains(var))
               currentMatches.Add(var);
         }
      }
   }

   private void OnMouseDown()
   {
      isPressed = true;
      _gridManager.DestroyMatches();
   }

   private void OnMouseUp()
   {
      isPressed = false;
   }
}
