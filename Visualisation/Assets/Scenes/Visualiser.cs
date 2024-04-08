using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Grpc.Net.Client;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http;
using Google.Protobuf.Collections;
using Grpc.Net.Client.Web;
using PlayerInterface;
using Random = UnityEngine.Random;
using Cysharp.Net.Http;
using static UnityEngine.Networking.UnityWebRequest; 

public partial class GameEngine : MonoBehaviour
{
    public GameObject Cell;
    public int Width;
    public int Length;
    public int Height;
    public float Spacing = 1.1f;
    private Array Map;
    private Color _food = new Color(0, 0.360f, 0, 1f);
    private Color _empty = new Color(0.360f, 0.360f, 0.360f, 0.05f);
    private List<UpdatedCell> _updatedCells = new List<UpdatedCell>();
    private List<PlayerScore> _playerScores = new List<PlayerScore>();
    private List<Player> Players = new List<Player>();

    public GameObject PlayerUi;
    private AddTextToDisplay playerUiGameObject;

    public GameEngine()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        playerUiGameObject = PlayerUi.GetComponent<AddTextToDisplay>();

        var client = InitGrpc();
        RegisterAndInitMap(client);

        Subscribe(client);
        ListenForPlayerNames(client);
    }

    private PlayerHost.PlayerHostClient InitGrpc()
    {
        var handler = new YetAnotherHttpHandler() { Http2Only = true };
        var channel = GrpcChannel.ForAddress("http://localhost:5168", new GrpcChannelOptions
        {
            HttpHandler = handler
        });
        var client = new PlayerHost.PlayerHostClient(channel);
        return client;
    }

    private async Task Subscribe(PlayerHost.PlayerHostClient client)
    {
        var req = new SubsribeRequest();
        var result = client.Subscribe(req);

        var source = new CancellationTokenSource();
        while (await result.ResponseStream.MoveNext(source.Token))
        {
            lock (_updatedCells)
            {
                _updatedCells.AddRange(result.ResponseStream.Current.UpdatedCells);
                _playerScores.AddRange(result.ResponseStream.Current.PlayerScores);
            }
        }
    }

    private async Task ListenForPlayerNames(PlayerHost.PlayerHostClient client)
    {
        var req = new EmptyRequest();
        var result = client.SubscribeToServerEvents(req);

        var source = new CancellationTokenSource();
        while (await result.ResponseStream.MoveNext(source.Token))
        {
            if (result.ResponseStream.Current.MessageType == MessageType.PlayerJoined)
            {
                lock (Players)
                {
                    AddPlayer(result.ResponseStream.Current.Message);
                }
            }
        }
    }

    private void RegisterAndInitMap(PlayerHost.PlayerHostClient client)
    {
        var reg = new RegisterRequest();
        var settings = client.Register(reg);
        CreateVis(settings.Dimensions.ToArray());
        if(settings.GameStarted)
        {
            var gameState = client.GetGameState(new EmptyRequest());
            lock(_updatedCells)
            {
                _updatedCells.AddRange(gameState.UpdatedCells);
            }
        }
    }

    public void CreateVis(int[] dimensions)
    {
        Map = Array.CreateInstance(typeof(GameObject), dimensions);
        LoopTings(Map, dimensions, new int[dimensions.Length], 0);
    }

    private void LoopTings(Array array, int[] lengths, int[] curIndex, int dimension)
    {
        for (int i = 0; i < lengths[dimension]; i++)
        {
            curIndex[dimension] = i;
            if (dimension == array.Rank - 1)
            {
                int[] copyArray = new int[curIndex.Length];
                curIndex.CopyTo(copyArray, 0);
                var position = GetPositionFromAddress(copyArray, lengths);

                var newCell = Instantiate(Cell, position, new Quaternion());
                newCell.name = $"Cell {string.Join(",", copyArray)}";
                array.SetValue(newCell, curIndex);
            }
            else
            {
                LoopTings(array, lengths, curIndex, dimension + 1);
            }
        }
    }

    private Vector3 GetPositionFromAddress(int[] address, int[] lengths)
    {
        var result = new Vector3(0, 0, 0);
        for (int i = 0; i < address.Length; i++)
        {
            var factor = 1;
            var prevDim = i - 3;
            while (prevDim >= 0)
            {
                factor *= (lengths[prevDim] + 1) ;
                prevDim -= 3;
            }
            result[i % 3] += factor * Spacing * address[i];
        }
        return result;
    }

    private void AddPlayer(string player)
    {
        lock (Players)
        {
            if (GetPlayer(player) == null)
            {
                var newPlayer = new Player()
                {
                    Color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f),
                    Name = player,
                    Score = 0
                };

                Players.Add(newPlayer);
                Debug.Log($"Player {newPlayer.Name} joined the game.");
                playerUiGameObject.UpdatePlayerScores(Players);
            }
        }
    }

    private Color GetColor(string playerName)
    {
        AddPlayer(playerName);
        return GetPlayer(playerName).Color;
    }

    // Update is called once per frame
    void Update()
    {
        if (_updatedCells.Any())
        {
            lock (_updatedCells)
            {
                UpdatePlayerScore();
                UpdateGameWorld();
            }
        }
    }

    private void UpdateGameWorld()
    {
        foreach (var cell in _updatedCells)
        {
            Color newColor = _empty;
            if (!string.IsNullOrWhiteSpace(cell.Player))
            {
                newColor = GetColor(cell.Player);
            }
            else if (cell.FoodValue > 0)
            {
                newColor = _food;
            }

            ColorObject(cell.Address.ToArray(), newColor);
        }
        _updatedCells.Clear();
    }

    private void UpdatePlayerScore()
    {
        foreach (var score in _playerScores)
        {
            var playerFound = GetPlayer(score.PlayerName);
            if (playerFound != null)
            {
                playerFound.Score = score.Score;
                playerFound.Snakes = score.Snakes;
            }
        }
        Players.RemoveAll(x => !_playerScores.Any(p => p.PlayerName == x.Name));    
        playerUiGameObject.UpdatePlayerScores(Players);
        _playerScores.Clear();
    }

    private Player GetPlayer(string playerName)
    {
        return Players.FirstOrDefault(player => player.Name == playerName);
    }

    private void ColorObject(int[] address, Color color)
    {
        ((GameObject)Map.GetValue(address)).GetComponent<Renderer>().material.color = color;
    }
}
