﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game Manager Script")]
    public static GameManager Instance;
    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    [Header("Main game components")]
    public UI_Controller UI_controller;
    public SceneLoader sceneLoader;

    [Header("Game info")]
    public string playerObjectName = "Player";

    public void LevelComplete()
    {
        UI_controller.EndGame(true);
    }

}
