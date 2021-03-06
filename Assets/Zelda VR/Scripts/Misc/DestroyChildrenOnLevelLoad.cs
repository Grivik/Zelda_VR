﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyChildrenOnLevelLoad : MonoBehaviour
{
    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //print(" OnSceneLoaded: " + scene);

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}