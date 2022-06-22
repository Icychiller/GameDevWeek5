using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GameConstants", menuName = "ScriptableObjects/GameConstants", order = 1)]
public class GameConstants : ScriptableObject
{
    // Scoring System
    int currentScore;
    int currentPlayerHealth;

    // Reset Values
    public string firstSceneName = "StarterScene";

    // World Values
    public float groundSurface = 0;
    public float maxCameraOffset = 3f;

    // Player Values
    public float maxSpeed = 10f;
    public float starSpeed = 50f;
    public int playerStartingMaxSpeed = 15;
    public int playerMaxJumpSpeed = 30;
    public int playerDefaultForce = 150;
    public int zoomSpeed = 10;
    public float gravityNormal = 20;
    public float gravityLow = 10;
    public float mudaCooldown = 0.1f;
    public float damageCooldown = 0.3f;

    // Enemy Values
    public int flattenSteps = 5;
    public int maxEnemySpawn = 2;
    public float stompCooldown = 5f;
    public float spawnCooldown = 2f;
    public float aggroTimer = 7f;

    // Debris
    public int debrisSpawnCount = 6;

    // Rotator
    public int rotatorSpeed = 15;
}
