using UnityEngine;
using System.Collections.Generic;

public class Army : MonoBehaviour
{
    [SerializeField]
    float speed;

    Player player;

    Town townFrom;
    Town townToAttack;

    [SerializeField]
    int armyPopulation;

    static int sid = 0;
    int id = 0;

    List<Vector3> path;

    void Awake()
    {
        id = sid++;
    }

    void Start()
    {
        
        GameInterface.GetInstance().RegisterArmy(this);
    }

    void FixedUpdate()
    {
        if(path != null && path.Count > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, path[0], speed * Time.fixedDeltaTime);
            if(Vector3.Distance(transform.position, path[0]) < 0.1)
            {
                path.RemoveAt(0);
            }
        }
    }

    void OnTriggerEnter(Collider collider_)
    {
        Town town = collider_.gameObject.GetComponent<Town>();

        // we may have no population if we collided with another army or if we collided with the town we left, do nothing
        if(GetPopulation() == 0 || town == townFrom)
        {
            return;
        }

        if(town != null)
        {
            Town.AttackTown(town, this);
            return;
        }

        Army army = collider_.gameObject.GetComponent<Army>();
        if(army != null)
        {
            Army.AttackArmy(this, army);

            if(GetPopulation() == 0)
            {
                Destroy(this.gameObject);
            }

            if(army.GetPopulation() == 0)
            {
                Destroy(army.gameObject);
            }
        }
    }

    public void SetPopulation(int population_)
    {
        armyPopulation = population_;
        UpdateSize();
    }

    public int GetPopulation()
    {
        return armyPopulation;
    }

    public void SetPlayer(Player player_)
    {
        player = player_;
        GetComponent<MeshRenderer>().material.color = player.GetColour();
    }

    public Player GetPlayer()
    {
        return player;
    }

    public void SetTownFrom(Town town_)
    {
        townFrom = town_;
    }

    public void SetTownToAttack(Town town_)
    {
        townToAttack = town_;
        Vector2 start = new Vector2(this.transform.position.x, this.transform.position.z); // y is up, so has now effect on position
        Vector2 end = new Vector2(townToAttack.transform.position.x, townToAttack.transform.position.z);
        path = GameInterface.instance.GetPath(start, end);

    }

    public int ReducePopulation(int populationCount_)
    {
        int remainderAfterAttackers = Mathf.Clamp(populationCount_ - armyPopulation, 0, populationCount_);
        armyPopulation = Mathf.Clamp(armyPopulation - populationCount_, 0, populationCount_);

        return remainderAfterAttackers;
    }

    public void OnDestroy()
    {
        GameInterface.GetInstance().UnregisterArmy(this);
    }

    public void UpdateSize()
    {
        int size = Mathf.Clamp(armyPopulation / 10, 1, 5);
        transform.localScale = new Vector3(size, size, size);
    }

    static void AttackArmy(Army armyOne_, Army armyTwo_)
    {
        if (armyOne_.GetPlayer() != armyTwo_.GetPlayer())
        {
            int remainder = armyOne_.ReducePopulation(armyTwo_.GetPopulation());
            armyTwo_.SetPopulation(remainder);
        }
        else
        {
            armyOne_.SetPopulation(armyOne_.GetPopulation() + armyTwo_.GetPopulation());
            armyTwo_.SetPopulation(0);
        }

        armyOne_.UpdateSize();
        armyTwo_.UpdateSize();
    }
}
