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

    public float mainChance = 60f;
    public float uncommonChance = 25f;
    public float rareChance = 15f;

    private bool TL_base;   // Top  Left
    private bool TC_base;   // "    Center
    private bool TR_base;   // "    Right
    private bool ML_base;   // Middle Left
    private bool MR_base;   // "    Right
    private bool BL_base;   // Bottom Left
    private bool BC_base;   // "    Center
    private bool BR_base;   // "    Right

    public void GenerateTiles()
    {
        // TODO: Check what is null and set bools for them

        BoundsInt plannerBounds = plannerTMap.cellBounds;
        TileBase[] plannerAllTiles = plannerTMap.GetTilesBlock(plannerBounds);
        TileBase[] baseAllTiles = baseTMap.GetTilesBlock(plannerBounds);

        // Load sprites
        Sprite[] groundSourceSprites = Resources.LoadAll<Sprite>(groundSource.name);

        for (int x = 0; x < plannerBounds.size.x; x++)
        {
            for (int y=0; y < plannerBounds.size.y; y++)
            { 
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

                if (TL_base && TC_base && TR_base && ML_base && MR_base && BL_base && BC_base && BR_base)
                {
                    // Center
                }
                
                else if (TC_base && MR_base && ML_base && !BC_base)
                {
                    // Bottom
                }

                else if (TC_base && MR_base && !ML_base && BC_base)
                {
                    // Left
                }

                else if (!TC_base && MR_base && ML_base && BC_base)
                {
                    // Top
                }

                else if (TC_base && !MR_base && ML_base && BC_base)
                {
                    // Right
                }

                else if (!TC_base && !MR_base && ML_base && BC_base)
                {
                    // Top Right
                }

                else if (TC_base && !MR_base && ML_base && !BC_base)
                {
                    // Bottom Right
                }

                else if (TC_base && MR_base && !ML_base && !BC_base)
                {
                    // Bottom Left
                }

                else if (!TC_base && MR_base && !ML_base && BC_base)
                {
                    // Top Left
                }

                else if (TC_base && MR_base && ML_base && BC_base && !TL_base)
                {
                    // Top Left Inner
                }

                else if (TC_base && MR_base && ML_base && BC_base && !TR_base)
                {
                    // Top Right Inner
                }

                else if (TC_base && MR_base && ML_base && BC_base && !BR_base)
                {
                    // Bottom Right Inner
                }

                else if (TC_base && MR_base && ML_base && BC_base && !BL_base)
                {
                    // Bottom Left Inner
                }
            }
        }
    }
}
