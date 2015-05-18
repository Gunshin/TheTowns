using UnityEngine;
using System.Collections.Generic;

using pathPlanner;

public class GameInterface : MonoBehaviour
{
    [SerializeField]
    Town townPrefab;

    [SerializeField]
    Player neutralPrefab;

    [SerializeField]
    Player playerPrefab;

    [SerializeField]
    Player aiSimplePrefab;

    public static GameInterface instance;

    Player humanPlayer;
    List<Player> players;

    Dictionary<Player, List<Town>> towns = new Dictionary<Player, List<Town>>();
    Dictionary<Player, List<Army>> armies = new Dictionary<Player, List<Army>>();

    public string mapFilePath;
    public pathPlanner.GraphGridMap pathplannerMap;
    public pathPlanner.IPathfinder pathfinder;

    public enum Events : int{
        Attack,
        Reinforce,
        MaxPopulation,
        MaxDefenders,
        OverPopulated,
        AttackerPopulationIncreased
    };

    void Awake()
    {
        instance = this;

        EventManager.instance = new EventManager();

        pathplannerMap = LoadMap(mapFilePath);

        pathfinder = new pathPlanner.AStar((x, y) =>
        {
            return Mathf.Sqrt(Mathf.Pow((float)(x.GetX() - y.GetX()), 2) + Mathf.Pow((float)(x.GetY() - y.GetY()), 2));
        });
    }

    void Start()
    {
        players = new List<Player>();

        Player neutralPlayer = (Player)Instantiate(neutralPrefab);
        players.Add(neutralPlayer);
        humanPlayer = (Player)Instantiate(playerPrefab);
        players.Add(humanPlayer);
        Player aiPlayer = (Player)Instantiate(aiSimplePrefab);
        players.Add(aiPlayer);
        

        humanPlayer.SetColour(Color.green);
        aiPlayer.SetColour(Color.red);

        neutralPlayer.SetColour(Color.grey);

        {
            Town town = (Town)Instantiate(townPrefab, new Vector3(100, 0, 10), Quaternion.identity);
            town.SetPlayer(humanPlayer);
            town.SetTownSize(3);
        }

        {
            Town town = (Town)Instantiate(townPrefab, new Vector3(100, 0, 41), Quaternion.identity);
            town.SetPlayer(neutralPlayer);
            town.SetTownSize(UnityEngine.Random.Range(1, 5));
        }

        {
            Town town = (Town)Instantiate(townPrefab, new Vector3(80, 0, 58), Quaternion.identity);
            town.SetPlayer(neutralPlayer);
            town.SetTownSize(UnityEngine.Random.Range(1, 5));
        }

        {
            Town town = (Town)Instantiate(townPrefab, new Vector3(65, 0, 118), Quaternion.identity);
            town.SetPlayer(neutralPlayer);
            town.SetTownSize(UnityEngine.Random.Range(1, 5));
        }

        {
            Town town = (Town)Instantiate(townPrefab, new Vector3(5, 0, 105), Quaternion.identity);
            town.SetPlayer(neutralPlayer);
            town.SetTownSize(UnityEngine.Random.Range(1, 5));
        }

        {
            Town town = (Town)Instantiate(townPrefab, new Vector3(112, 0, 168), Quaternion.identity);
            town.SetPlayer(neutralPlayer);
            town.SetTownSize(UnityEngine.Random.Range(1, 5));
        }

        {
            Town town = (Town)Instantiate(townPrefab, new Vector3(160, 0, 168), Quaternion.identity);
            town.SetPlayer(neutralPlayer);
            town.SetTownSize(UnityEngine.Random.Range(1, 5));
        }

        {
            Town town = (Town)Instantiate(townPrefab, new Vector3(178, 0, 104), Quaternion.identity);
            town.SetPlayer(neutralPlayer);
            town.SetTownSize(UnityEngine.Random.Range(1, 5));
        }

        {
            Town town = (Town)Instantiate(townPrefab, new Vector3(113, 0, 137), Quaternion.identity);
            town.SetPlayer(neutralPlayer);
            town.SetTownSize(UnityEngine.Random.Range(1, 5));
        }

        {
            Town town = (Town)Instantiate(townPrefab, new Vector3(146, 0, 136), Quaternion.identity);
            town.SetPlayer(neutralPlayer);
            town.SetTownSize(UnityEngine.Random.Range(1, 5));
        }

        {
            Town town = (Town)Instantiate(townPrefab, new Vector3(113, 0, 73), Quaternion.identity);
            town.SetPlayer(neutralPlayer);
            town.SetTownSize(UnityEngine.Random.Range(1, 5));
        }

        {
            Town town = (Town)Instantiate(townPrefab, new Vector3(145, 0, 72), Quaternion.identity);
            town.SetPlayer(neutralPlayer);
            town.SetTownSize(UnityEngine.Random.Range(1, 5));
        }

        {
            Town town = (Town)Instantiate(townPrefab, new Vector3(240, 0, 73), Quaternion.identity);
            town.SetPlayer(neutralPlayer);
            town.SetTownSize(UnityEngine.Random.Range(1, 5));
        }

        {
            Town town = (Town)Instantiate(townPrefab, new Vector3(273, 0, 41), Quaternion.identity);
            town.SetPlayer(neutralPlayer);
            town.SetTownSize(UnityEngine.Random.Range(1, 5));
        }

        {
            Town town = (Town)Instantiate(townPrefab, new Vector3(225, 0, 153), Quaternion.identity);
            town.SetPlayer(neutralPlayer);
            town.SetTownSize(UnityEngine.Random.Range(1, 5));
        }

        {
            Town town = (Town)Instantiate(townPrefab, new Vector3(272, 0, 200), Quaternion.identity);
            town.SetPlayer(aiPlayer);
            town.SetTownSize(3);
        }

        {
            Town town = (Town)Instantiate(townPrefab, new Vector3(273, 0, 152), Quaternion.identity);
            town.SetPlayer(neutralPlayer);
            town.SetTownSize(UnityEngine.Random.Range(1, 5));
        }

        {
            Town town = (Town)Instantiate(townPrefab, new Vector3(162, 0, 41), Quaternion.identity);
            town.SetPlayer(neutralPlayer);
            town.SetTownSize(UnityEngine.Random.Range(1, 5));
        }

        {
            Town town = (Town)Instantiate(townPrefab, new Vector3(193, 0, 71), Quaternion.identity);
            town.SetPlayer(neutralPlayer);
            town.SetTownSize(UnityEngine.Random.Range(1, 5));
        }

    }

    void Update()
    {

    }

    public void RegisterTown(Town town_)
    {
        GetPlayerTowns(town_.GetPlayer()).Add(town_);
    }

    public bool UnregisterTown(Town town_)
    {
        if(town_.GetPlayer() == null)
        {
            return true;
        }

        return GetPlayerTowns(town_.GetPlayer()).Remove(town_);
    }

    public void RegisterArmy(Army army_)
    {
        GetPlayerArmies(army_.GetPlayer()).Add(army_);
    }

    public bool UnregisterArmy(Army army_)
    {
        if (army_.GetPlayer() == null)
        {
            return true;
        }

        return GetPlayerArmies(army_.GetPlayer()).Remove(army_);
    }

    public List<Army> GetPlayerArmies(Player player_)
    {

        List<Army> playerArmies;
        if (!armies.TryGetValue(player_, out playerArmies))
        {
            playerArmies = new List<Army>();
            armies.Add(player_, playerArmies);
        }

        return playerArmies;
    }

    public List<Town> GetPlayerTowns(Player player_)
    {
        List<Town> playerTowns;
        if (!towns.TryGetValue(player_, out playerTowns))
        {
            playerTowns = new List<Town>();
            towns.Add(player_, playerTowns);
        }

        return playerTowns;
    }

    public Player GetHumanPlayer()
    {
        return humanPlayer;
    }

    public List<Player> GetAllPlayers()
    {
        return players;
    }

    public List<Vector3> GetPath(Vector2 start_, Vector2 goal_)
    {
        pathPlanner.PathplannerParameter param = new pathPlanner.PathplannerParameter();
        param.startNode = pathplannerMap.GetNodeByIndex((int)start_.x, (int)start_.y);
        param.goalNode = pathplannerMap.GetNodeByIndex((int)goal_.x, (int)goal_.y);

        Array<object> nodes = pathfinder.FindPath(param);
        List<Vector3> path = new List<Vector3>();
        for(int i = 0; i < nodes.length; ++i)
        {
            Position pos = ((Node)nodes[i]).GetPosition();
            path.Add(new Vector3(pos.GetX(), 0, pos.GetY()));// y has no effect
        }

        return path;

    }

    public static GameInterface GetInstance()
    {
        return instance;
    }

    string[] ReadLines(string filePath_)
    {
        System.IO.File.SetAttributes(Application.dataPath + filePath_, System.IO.File.GetAttributes(Application.dataPath + filePath_) & ~System.IO.FileAttributes.ReadOnly);

        List<string> lines = new List<string>();
        System.IO.StreamReader sr = new System.IO.StreamReader(Application.dataPath + filePath_);

        while (!sr.EndOfStream)
        {
            lines.Add(sr.ReadLine());
        }

        return lines.ToArray();
    }

    public pathPlanner.GraphGridMap LoadMap(string filePath_)
    {
        string[] fin = ReadLines(filePath_);

        int height = int.Parse(fin[1].Split(' ')[1]);
        int width = int.Parse(fin[2].Split(' ')[1]);

        pathPlanner.GraphGridMap map = new pathPlanner.GraphGridMap(width, height);

        for (int y = 4; y < fin.Length; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                char value = fin[y][x];
                switch (value)
                {
                    case '.':
                    case 'G':
                    case 'S':
                        map.GetNodeByIndex(x, y - 4).SetTraversable(true);
                        break;

                    case '@':
                    case 'O':
                    case 'W':
                    case 'T':
                        map.GetNodeByIndex(x, y - 4).SetTraversable(false);
                        break;

                    default:
                        Debug.Log("something went wrong in loading map: " + filePath_ + " : " + value);
                        break;

                }

            }
        }

        return map;
    }
}
