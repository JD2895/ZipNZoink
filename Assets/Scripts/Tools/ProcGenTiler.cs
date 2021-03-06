using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class ProcGenTiler : MonoBehaviour
{
    public Tilemap plannerTMap = null;
    public TileBase plannerGroundTile = null;
    public TileBase plannerHazardTile = null;

    public Tilemap baseTMap = null;
    public Tilemap hazardTMap = null;

    public List<TileAndChance> center_TileList;
    public List<TileAndChance> bottom_TileList;
    public List<TileAndChance> left_TileList;
    public List<TileAndChance> top_TileList;
    public List<TileAndChance> right_TileList;
    public List<TileAndChance> topRight_TileList;
    public List<TileAndChance> bottomRight_TileList;
    public List<TileAndChance> bottomLeft_TileList;
    public List<TileAndChance> topLeft_TileList;
    public List<TileAndChance> topLeftInner_TileList;
    public List<TileAndChance> topRightInner_TileList;
    public List<TileAndChance> bottomRightInner_TileList;
    public List<TileAndChance> bottomLeftInner_TileList;

    private bool TL_base;   // Top  Left
    private bool TC_base;   // "    Center
    private bool TR_base;   // "    Right
    private bool ML_base;   // Middle Left
    private bool MR_base;   // "    Right
    private bool BL_base;   // Bottom Left
    private bool BC_base;   // "    Center
    private bool BR_base;   // "    Right

    public void ClearTiles()
    {
        baseTMap.ClearAllTiles();
        //hazardTMap.ClearAllTiles();
    }

    public void HideShowPlanner()
    {
        TilemapRenderer tmapRend = plannerTMap.GetComponent<TilemapRenderer>();
        tmapRend.enabled = !tmapRend.enabled;
    }

    public void HideShowActual()
    {
        TilemapRenderer tmapRend1 = baseTMap.GetComponent<TilemapRenderer>();
        TilemapRenderer tmapRend2 = hazardTMap.GetComponent<TilemapRenderer>();
        tmapRend1.enabled = !tmapRend1.enabled;
        tmapRend2.enabled = !tmapRend2.enabled;
    }

    public void GenerateTiles()
    {
        // TODO: Check what is null and set bools for them

        BoundsInt plannerBounds = plannerTMap.cellBounds;
        TileBase[] plannerAllTiles = plannerTMap.GetTilesBlock(plannerBounds);
        TileBase[] baseAllTiles = baseTMap.GetTilesBlock(plannerBounds);

        for (int x = 0; x < plannerBounds.size.x; x++)
        {
            for (int y=0; y < plannerBounds.size.y; y++)
            {
                float tileChance = Random.Range(0f, 100f);
                TileBase tileToSet = null;

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

                #region Set Tile based on bools
                if (TL_base && TC_base && TR_base && ML_base && MR_base && BL_base && BC_base && BR_base)
                {
                    // Center
                    tileToSet = ChooseTile(center_TileList, tileChance);
                }
                else if (!TC_base && MR_base && ML_base && BC_base)
                {
                    // Bottom
                    tileToSet = ChooseTile(bottom_TileList, tileChance);
                }
                else if (TC_base && MR_base && !ML_base && BC_base)
                {
                    // Left
                    tileToSet = ChooseTile(left_TileList, tileChance);
                }
                else if (TC_base && MR_base && ML_base && !BC_base)
                {
                    // Top
                    tileToSet = ChooseTile(top_TileList, tileChance);
                }
                else if (TC_base && !MR_base && ML_base && BC_base)
                {
                    // Right
                    tileToSet = ChooseTile(right_TileList, tileChance);
                }
                else if (TC_base && !MR_base && ML_base && !BC_base)
                {
                    // Top Right
                    tileToSet = ChooseTile(topRight_TileList, tileChance);
                }
                else if (!TC_base && !MR_base && ML_base && BC_base)
                {
                    // Bottom Right
                    tileToSet = ChooseTile(bottomRight_TileList, tileChance);
                }
                else if (!TC_base && MR_base && !ML_base && BC_base)
                {
                    // Bottom Left
                    tileToSet = ChooseTile(bottomLeft_TileList, tileChance);
                }
                else if (TC_base && MR_base && !ML_base && !BC_base)
                {
                    // Top Left
                    tileToSet = ChooseTile(topLeft_TileList, tileChance);
                }
                else if (TC_base && MR_base && ML_base && BC_base && !BL_base)
                {
                    // Top Left Inner
                    tileToSet = ChooseTile(topLeftInner_TileList, tileChance);
                }
                else if (TC_base && MR_base && ML_base && BC_base && !BR_base)
                {
                    // Top Right Inner
                    tileToSet = ChooseTile(topRightInner_TileList, tileChance);
                }
                else if (TC_base && MR_base && ML_base && BC_base && !TR_base)
                {
                    // Bottom Right Inner
                    tileToSet = ChooseTile(bottomRightInner_TileList, tileChance);
                }
                else if (TC_base && MR_base && ML_base && BC_base && !TL_base)
                {
                    // Bottom Left Inner
                    tileToSet = ChooseTile(bottomLeftInner_TileList, tileChance);
                }
                #endregion

                //baseAllTiles[x + y * plannerBounds.size.x].;
                if (tileToSet != null)
                    baseTMap.SetTile(new Vector3Int(x + plannerTMap.origin.x, y + plannerTMap.origin.y, 0), tileToSet);
                else
                    Debug.LogError("No Tile to set");

            }
        }
    }

    private TileBase ChooseTile(List<TileAndChance> toChooseFrom, float randomChoice)
    {
        float chanceTotal = 0f;

        foreach (TileAndChance tileChoice in toChooseFrom)
        {
            if (tileChoice.chance + chanceTotal > randomChoice)
                return tileChoice.tile;
            else
                chanceTotal += tileChoice.chance;
        }

        Debug.LogError("No Tile Chosen");
        return null;
    }
}

[System.Serializable]
public class TileAndChance
{
    public TileBase tile;
    public float chance;
}
