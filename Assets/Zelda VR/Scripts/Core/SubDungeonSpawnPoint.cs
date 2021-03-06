﻿using UnityEngine;


public class SubDungeonSpawnPoint : MonoBehaviour
{
    public GameObject subDungeonPrefab;
    public GameObject marker;

    public Collectible collectiblePrefab;
    public SubDungeonSpawnPoint warpTo;


    SubDungeon _spawnedSubDungeon = null;
    Transform _subDungeonContainer;


    public DungeonRoom ParentDungeonRoom { get; set; }


    void Awake()
    {
        _subDungeonContainer = GameObject.Find("SubDungeons").transform;

        marker.SetActive(false);
    }


    public SubDungeon SpawnSubDungeon()
    {
        if (_spawnedSubDungeon != null)
        {
            return _spawnedSubDungeon;
        }

        GameObject g = Instantiate(subDungeonPrefab, transform.position, transform.rotation) as GameObject;
        _spawnedSubDungeon = g.GetComponent<SubDungeon>();
        _spawnedSubDungeon.name = subDungeonPrefab.name;
        _spawnedSubDungeon.transform.parent = _subDungeonContainer;
        _spawnedSubDungeon.SpawnPoint = this;

        // Unique Collectible
        if (collectiblePrefab != null)
        {
            Inventory inv = Inventory.Instance;
            if (!inv.HasItem(collectiblePrefab.itemPrefab.name))
            {
                Collectible uniqueItem = (Instantiate(collectiblePrefab.gameObject) as GameObject).GetComponent<Collectible>();
                uniqueItem.transform.parent = _spawnedSubDungeon.uniqueItemContainer;
                uniqueItem.transform.localPosition = Vector3.zero;
                _spawnedSubDungeon.uniqueItem = uniqueItem;
            }
        }

        return _spawnedSubDungeon;
    }

    public void DestroySubDungeon()
    {
        if (_spawnedSubDungeon == null) { return; }

        Destroy(_spawnedSubDungeon.gameObject);
        _spawnedSubDungeon = null;
    }
}