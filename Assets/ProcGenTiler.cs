using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class ProcGenTiler : MonoBehaviour
{
    public Tilemap plannerTMap = null;
    public Tilemap baseTMap = null;
    public Tilemap hazardTMap = null;

    public TileBase plannerGroundTile = null;
    public TileBase plannerHazardTile = null;

    public Texture2D groundSource = null;
    public Texture2D hazardSource = null;

    public float chance_1 = 50f;
    public float chance_2 = 9f;
    public float chance_3 = 9f;
    public float chance_4 = 9f;
    public float chance_5 = 6f;
    public float chance_6 = 6f;
    public float chance_7 = 5f;
    public float chance_8 = 3f;
    public float chance_9 = 3f;

    private bool TL_base;   // Top  Left
    private bool TC_base;   // "    Center
    private bool TR_base;   // "    Right
    private bool ML_base;   // Middle Left
    private bool MR_base;   // "    Right
    private bool BL_base;   // Bottom Left
    private bool BC_base;   // "    Center
    private bool BR_base;   // "    Right

    private int tilesPerRow = 13;

    private void Start()
    {
        //plannerTMap.SetTile(new Vector3Int(2, 2, 0), plannerHazardTile);
        //plannerTMap.SetTile(new Vector3Int(0, 2, 0), plannerHazardTile);
        //plannerTMap.SetTile(new Vector3Int(2, 0, 0), plannerHazardTile);
        //plannerTMap.SetTile(new Vector3Int(0, 0, 0), plannerHazardTile);
        //plannerTMap.ClearAllTiles();
        
        BoundsInt plannerBounds = plannerTMap.cellBounds;
        TileBase[] plannerAllTiles = plannerTMap.GetTilesBlock(plannerBounds);

        for (int x = 0; x < plannerBounds.size.x; x++)
        {
            for (int y = 0; y < plannerBounds.size.y; y++)
            {
                TileBase plannerTile = plannerAllTiles[x + y * plannerBounds.size.x];

                if (plannerTile == null || plannerTile != plannerGroundTile)
                    continue;

                plannerTMap.SetTile(new Vector3Int(x + plannerTMap.origin.x, y + plannerTMap.origin.y, 0), plannerHazardTile);
            }
        }
                //GenerateTiles();
    }

    public void GenerateTiles()
    {
        // TODO: Check what is null and set bools for them

        BoundsInt plannerBounds = plannerTMap.cellBounds;
        TileBase[] plannerAllTiles = plannerTMap.GetTilesBlock(plannerBounds);
        TileBase[] baseAllTiles = baseTMap.GetTilesBlock(plannerBounds);

        // Load sprites
        Sprite[] groundSourceSprites = Resources.LoadAll<Sprite>(groundSource.name);

        Debug.Log(plannerBounds.size.x + " : " + plannerBounds.size.y);

        for (int x = 0; x < plannerBounds.size.x; x++)
        {
            for (int y=0; y < plannerBounds.size.y; y++)
            {
                #region Tile Chooser
                float tileChance = Random.Range(0f, 100f);
                int tileRow = 0;

                if (tileChance < chance_1)
                    tileRow = 0;
                else if (tileChance < chance_1 + chance_2)
                    tileRow = 1;
                else if (tileChance < chance_1 + chance_2 + chance_3)
                    tileRow = 2;
                else if (tileChance < chance_1 + chance_2 + chance_3 + chance_4)
                    tileRow = 3;
                else if (tileChance < chance_1 + chance_2 + chance_3 + chance_4 + chance_5)
                    tileRow = 4;
                else if (tileChance < chance_1 + chance_2 + chance_3 + chance_4 + chance_5 + chance_6)
                    tileRow = 5;
                else if (tileChance < chance_1 + chance_2 + chance_3 + chance_4 + chance_5 + chance_6 + chance_7)
                    tileRow = 6;
                else if (tileChance < chance_1 + chance_2 + chance_3 + chance_4 + chance_5 + chance_6 + chance_7 + chance_8)
                    tileRow = 7;
                else if (tileChance < chance_1 + chance_2 + chance_3 + chance_4 + chance_5 + chance_6 + chance_7 + chance_8 + chance_9)
                    tileRow = 8;
                #endregion

                #region Reset Tile Bools
                TL_base = false;
                TC_base = false;
                TR_base = false;
                ML_base = false;
                MR_base = false;
                BL_base = false;
                BC_base = false;
                BR_base = false;
                #endregion

                TileBase plannerTile = plannerAllTiles[x + y * plannerBounds.size.x];

                if (plannerTile == null || plannerTile != plannerGroundTile)
                    continue;

                Sprite spriteToSet = null;

                #region Edge of Bounds check and bool set
                if (x == 0)
                {
                    TL_base = true;
                    ML_base = true;
                    BL_base = true;
                }
                else if (x == plannerBounds.size.x - 1)
                {
                    TR_base = true;
                    MR_base = true;
                    BR_base = true;
                }

                if (y == 0)
                {
                    TL_base = true;
                    TC_base = true;
                    TR_base = true;
                }
                else if (y == plannerBounds.size.y - 1)
                {
                    BL_base = true;
                    BC_base = true;
                    BR_base = true;
                }
                #endregion

                #region Check all unchecked directions
                // Top Left
                if (!TL_base)
                {
                    if (plannerAllTiles[(x - 1) + (y - 1) * plannerBounds.size.x] == plannerGroundTile)
                    {
                        TL_base = true;
                    }
                }

                // Top Center
                if (!TC_base)
                {
                    if (plannerAllTiles[(x) + (y - 1) * plannerBounds.size.x] == plannerGroundTile)
                    {
                        TC_base = true;
                    }
                }

                // Top Right
                if (!TR_base)
                {
                    if (plannerAllTiles[(x + 1) + (y - 1) * plannerBounds.size.x] == plannerGroundTile)
                    {
                        TR_base = true;
                    }
                }

                // Middle Left
                if (!ML_base)
                {
                    if (plannerAllTiles[(x - 1) + (y) * plannerBounds.size.x] == plannerGroundTile)
                    {
                        ML_base = true;
                    }
                }

                // Middle Right
                if (!MR_base)
                {
                    if (plannerAllTiles[(x + 1) + (y) * plannerBounds.size.x] == plannerGroundTile)
                    {
                        MR_base = true;
                    }
                }

                // Bottom Left
                if (!BL_base)
                {
                    if (plannerAllTiles[(x - 1) + (y + 1) * plannerBounds.size.x] == plannerGroundTile)
                    {
                        BL_base = true;
                    }
                }

                // Bottom Center
                if (!BC_base)
                {
                    if (plannerAllTiles[(x) + (y + 1) * plannerBounds.size.x] == plannerGroundTile)
                    {
                        BC_base = true;
                    }
                }

                // Bottom Right
                if (!BR_base)
                {
                    if (plannerAllTiles[(x + 1) + (y + 1) * plannerBounds.size.x] == plannerGroundTile)
                    {
                        BR_base = true;
                    }
                }
                #endregion

                Debug.Log(x + " : " + y);

                #region Set Tile based on bools
                if (TL_base && TC_base && TR_base && ML_base && MR_base && BL_base && BC_base && BR_base)
                {
                    // Center
                    spriteToSet = groundSourceSprites[0 + tileRow * tilesPerRow];
                }
                else if (TC_base && MR_base && ML_base && !BC_base)
                {
                    // Bottom
                    spriteToSet = groundSourceSprites[1 + tileRow * tilesPerRow];
                }
                else if (TC_base && MR_base && !ML_base && BC_base)
                {
                    // Left
                    spriteToSet = groundSourceSprites[2 + tileRow * tilesPerRow];
                }
                else if (!TC_base && MR_base && ML_base && BC_base)
                {
                    // Top
                    spriteToSet = groundSourceSprites[3 + tileRow * tilesPerRow];
                }
                else if (TC_base && !MR_base && ML_base && BC_base)
                {
                    // Right
                    spriteToSet = groundSourceSprites[4 + tileRow * tilesPerRow];
                }
                else if (!TC_base && !MR_base && ML_base && BC_base)
                {
                    // Top Right
                    spriteToSet = groundSourceSprites[5 + tileRow * tilesPerRow];
                }
                else if (TC_base && !MR_base && ML_base && !BC_base)
                {
                    // Bottom Right
                    spriteToSet = groundSourceSprites[6 + tileRow * tilesPerRow];
                }
                else if (TC_base && MR_base && !ML_base && !BC_base)
                {
                    // Bottom Left
                    spriteToSet = groundSourceSprites[7 + tileRow * tilesPerRow];
                }
                else if (!TC_base && MR_base && !ML_base && BC_base)
                {
                    // Top Left
                    spriteToSet = groundSourceSprites[8 + tileRow * tilesPerRow];
                }
                else if (TC_base && MR_base && ML_base && BC_base && !TL_base)
                {
                    // Top Left Inner
                    spriteToSet = groundSourceSprites[9 + tileRow * tilesPerRow];
                }
                else if (TC_base && MR_base && ML_base && BC_base && !TR_base)
                {
                    // Top Right Inner
                    spriteToSet = groundSourceSprites[10 + tileRow * tilesPerRow];
                }
                else if (TC_base && MR_base && ML_base && BC_base && !BR_base)
                {
                    // Bottom Right Inner
                    spriteToSet = groundSourceSprites[11 + tileRow * tilesPerRow];
                }
                else if (TC_base && MR_base && ML_base && BC_base && !BL_base)
                {
                    // Bottom Left Inner
                    spriteToSet = groundSourceSprites[12 + tileRow * tilesPerRow];
                }
                #endregion

                //baseAllTiles[x + y * plannerBounds.size.x].;

            }
        }
    }
}
