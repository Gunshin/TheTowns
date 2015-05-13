using UnityEngine;
using System.Collections.Generic;

public class GameInterface : MonoBehaviour
{
    [SerializeField]
    Town townPrefab;

    [SerializeField]
    Player playerPrefab;

    static GameInterface instance;

    Player humanPlayer;
    List<Player> players;

    Dictionary<Player, List<Town>> towns = new Dictionary<Player, List<Town>>();
    Dictionary<Player, List<Army>> armies = new Dictionary<Player, List<Army>>();

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        players = new List<Player>();

        humanPlayer = (Player)Instantiate(playerPrefab);
        players.Add(humanPlayer);
        Player aiPlayer = (Player)Instantiate(playerPrefab);
        players.Add(aiPlayer);

        Town townA = (Town)Instantiate(townPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        townA.SetPlayer(humanPlayer);
        townA.SetTownSize(5);

        Town townB = (Town)Instantiate(townPrefab, new Vector3(5, 0, 0), Quaternion.identity);
        townB.SetPlayer(aiPlayer);
        townB.SetTownSize(1);
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
        return GetPlayerTowns(town_.GetPlayer()).Remove(town_);
    }

    public void RegisterArmy(Army army_)
    {
        GetPlayerArmies(army_.GetPlayer()).Add(army_);
    }

    public bool UnregisterArmy(Army army_)
    {
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

    public static GameInterface GetInstance()
    {
        return instance;
    }
}
