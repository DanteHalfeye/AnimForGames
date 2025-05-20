using System;
using UnityEngine;

public class Game : MonoBehaviour
{
    #region Singleton
    private static Game _instance;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void CreateGameInstance()
    {
        GameObject gameGo = new GameObject("[GAME]");
        _instance = gameGo.AddComponent<Game>();
        DontDestroyOnLoad(gameGo);
    }
    
    public static Game Instance
    {
        get
        {
            if (_instance == null)
            {
                CreateGameInstance();
            }
            return _instance;
        }
    }
    #endregion
    
    private CharacterState playerOne;
    
    private void CreatePlayer()
    {
        GameObject playerGo = new GameObject("[PLAYER 1]");
        playerOne = playerGo.AddComponent<CharacterState>();
        DontDestroyOnLoad(playerGo);
    }

    private void Awake()
    {
        CreatePlayer();
    }

    public CharacterState PlayerOne => playerOne;
}
