using UnityEngine;

public class Army : MonoBehaviour
{
    [SerializeField]
    float speed;

    [SerializeField]
    int playerID;

    Town townToAttack;

    int armyPopulation;
    
    void Start()
    {

    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, townToAttack.transform.position, speed);
    }

    void OnCollisionEnter(Collision collision_)
    {
        Town town = collision_.gameObject.GetComponent<Town>();
        if(town != null)
        {
            Town.AttackTown(town, this);
            return;
        }

        Army army = collision_.gameObject.GetComponent<Army>();
        if(army != null)
        {
            Army.AttackArmy(army, this);

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
    }

    public int GetPopulation()
    {
        return armyPopulation;
    }

    public void SetPlayer(int playerID_)
    {
        playerID = playerID_;
    }

    public int GetPlayerID()
    {
        return playerID;
    }

    public void SetTownToAttack(Town town_)
    {
        townToAttack = town_;
    }

    public int ReducePopulation(int populationCount_)
    {
        int remainderAfterAttackers = Mathf.Clamp(populationCount_ - armyPopulation, 0, populationCount_);
        armyPopulation = Mathf.Clamp(armyPopulation - populationCount_, 0, populationCount_);

        return remainderAfterAttackers;
    }

    static void AttackArmy(Army armyOne_, Army armyTwo_)
    {
        int remainder = armyOne_.ReducePopulation(armyTwo_.GetPopulation());
        armyTwo_.SetPopulation(remainder);
    }
}
