﻿using UnityEngine;
using System.Collections.Generic;
using Immersio.Utility;

public class EnemyAI_RiverZora : EnemyAI 
{
    const int WARP_RANGE = 5;


    public float underwaterDuration = 2.0f;
    public float emergeDuration = 1.0f;
    public float surfaceDuration = 3.0f;
    public float submergeDuration = 1.0f;


    float _startTime = float.NegativeInfinity;
    float _timerDuration;
    Vector3 _origin;
    List<Index2> _warpableTiles = new List<Index2>();


    public bool IsUnderwater { get { return AnimatorInstance.GetCurrentAnimatorStateInfo(0).IsTag("Underwater"); } }
    public bool IsEmerging { get { return AnimatorInstance.GetCurrentAnimatorStateInfo(0).IsTag("Emerge"); } }
    public bool IsSubmerging { get { return AnimatorInstance.GetCurrentAnimatorStateInfo(0).IsTag("Submerge"); } }
    public bool IsSurfaced { get { return AnimatorInstance.GetCurrentAnimatorStateInfo(0).IsTag("Surface"); } }


    void Start()
    {
        transform.SetY(GroundPosY);
        _origin = transform.position;

        AnimatorInstance.GetComponent<Renderer>().enabled = false;
        _healthController.isIndestructible = true;

        AssignWarpableTiles();
    }


    void Update()
    {
        transform.SetY(GroundPosY);         // hack?

        if (!_doUpdate) { return; }
        if (IsPreoccupied) { return; }

        bool timesUp = (Time.time - _startTime >= _timerDuration);
        if (timesUp)
        {
            if (IsUnderwater)
            {
                AnimatorInstance.SetTrigger("Emerge");
                AnimatorInstance.GetComponent<Renderer>().enabled = true;
                _timerDuration = emergeDuration;
                WarpToRandomNearbyWaterTile();
            }
            else if (IsEmerging)
            {
                AnimatorInstance.SetTrigger("Surface");
                _timerDuration = surfaceDuration;
                _healthController.isIndestructible = false;
                FacePlayer();
                _enemy.Attack();
            }
            else if (IsSurfaced)
            {
                AnimatorInstance.SetTrigger("Submerge");
                _timerDuration = submergeDuration;
                _healthController.isIndestructible = true;
            }
            else if (IsSubmerging)
            {
                AnimatorInstance.SetTrigger("Underwater");
                AnimatorInstance.GetComponent<Renderer>().enabled = false;
                _timerDuration = underwaterDuration;
                ReplenishHealth();
            }

            _startTime = Time.time;
        }

        if (IsSurfaced)
        {
            FacePlayer();
        }
    }

    void ReplenishHealth()
    {
        _healthController.RestoreHealth();
    }

    void FacePlayer()
    {
        transform.forward = ToPlayer;
    }


    void AssignWarpableTiles()
    {
        TileMap tileMap = CommonObjects.OverworldTileMap;
        if (tileMap == null) { return; }

        Rect area = new Rect(
            (int)_origin.x - WARP_RANGE - EPSILON, 
            (int)_origin.y - WARP_RANGE - EPSILON,
            2 * WARP_RANGE + EPSILON, 
            2 * WARP_RANGE + EPSILON);
        _warpableTiles = tileMap.GetTilesInArea(area, TileInfo.WaterTiles);
    }
    
    void WarpToRandomNearbyWaterTile()
    {
        if (WorldInfo.Instance.IsSpecial) { return; }

        Index2 tile = _warpableTiles[Random.Range(0, _warpableTiles.Count)];
        SetEnemyPositionXZToTile(tile);
    }
}