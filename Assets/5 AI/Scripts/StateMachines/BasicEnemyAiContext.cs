using UnityEngine;

[System.Serializable]
public class BasicEnemyAiContext 
{
    public GameObject agent;
    public GameObject player;
    public Transform target;
    public float targetDistance;
}
