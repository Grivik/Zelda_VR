﻿using UnityEngine;

public class GrottoSpawnPoint : MonoBehaviour
{
    public enum GrottoType
    {
        UniqueItem,
        Shop,
        Gamble,
        PayRupees,
        Gift,
        Message,
        Medicine,
        Warp,
        HeartContainer,
        PayForInfo
    }


    public GameObject grottoPrefab;
    public GameObject marker;

    public GrottoType grottoType;
    public string text;
    public int giftAmount;

    public GameObject npcSpawnPointPrefab;

    public Collectible uniqueCollectiblePrefab;
    public Collectible giftPrefab;

    public bool showEntranceWalls;
    public bool HasSpecialResourceBeenTapped        // (i.e. HeartContainer or Potion collected, Gift collected, UniqueCollectible collected)
    {
        get { return _overworldInfo.HasGrottoBeenTapped(this); }
        set { _overworldInfo.SetGrottoHasBeenTapped(this, value); }
    }      


    public Collectible saleItemPrefabA, saleItemPrefabB, saleItemPrefabC;
    public int saleItemPriceA, saleItemPriceB, saleItemPriceC;

    public string[] payForInfoText;


    OverworldInfo _overworldInfo;
    Transform _grottosContainer;


    public Grotto SpawnedGrotto { get; private set; }


    void Awake()
    {
        _grottosContainer = GameObject.Find("Grottos").transform;
        _overworldInfo = GameObject.FindGameObjectWithTag("OverworldInfo").GetComponent<OverworldInfo>();

        marker.SetActive(false);
    }


    public Grotto SpawnGrotto()
    {
        GameObject g = Instantiate(grottoPrefab, transform.position, transform.rotation) as GameObject;

        SpawnedGrotto = g.GetComponent<Grotto>();
        SpawnedGrotto.name = "Grotto - " + grottoType.ToString();
        SpawnedGrotto.transform.parent = _grottosContainer;
        SpawnedGrotto.GrottoSpawnPoint = this;

        return SpawnedGrotto;
    }

    public void DestroyGrotto()
    {
        Destroy(SpawnedGrotto.gameObject);
        SpawnedGrotto = null;
    }


    #region Serialization

    public class Serializable
    {
        public bool hasSpecialResourceBeenTapped;
    }

    public Serializable GetSerializable()
    {
        Serializable info = new Serializable();

        info.hasSpecialResourceBeenTapped = HasSpecialResourceBeenTapped;

        return info;
    }

    public void InitWithSerializable(Serializable info)
    {
        HasSpecialResourceBeenTapped = info.hasSpecialResourceBeenTapped;
    }

    #endregion
}