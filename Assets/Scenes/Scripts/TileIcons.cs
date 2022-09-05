using System.Collections.Generic;
using UnityEngine;

public class TileIcons : MonoBehaviour
{
   public List<Sprite> sprites;
   private Tile _tile;
   [SerializeField] private int a;
   [SerializeField] private int b;
   [SerializeField] private int c;
   private SpriteRenderer spriteRenderer;

   private void Start()
   {
      _tile = GetComponent<Tile>();
      spriteRenderer = GetComponent<SpriteRenderer>();
   }

   private void Update()
   {
      CheckIcons();
   }

   private void CheckIcons()
   {
      if (_tile.currentMatches.Count <= a)
         spriteRenderer.sprite = sprites[0];
      else if (_tile.currentMatches.Count > a && _tile.currentMatches.Count <= b )
         spriteRenderer.sprite = sprites[1];
      else if (_tile.currentMatches.Count > b && _tile.currentMatches.Count <= c)
         spriteRenderer.sprite = sprites[2];
      else if (_tile.currentMatches.Count > c)
         spriteRenderer.sprite = sprites[3];
   }
}
