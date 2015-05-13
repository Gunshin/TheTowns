using UnityEngine;

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
        transform.position = Vector3.MoveTowards(transform.position, townToAttack.transform.position, speed * Time.fixedDeltaTime);
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
    }

    public int GetPopulation()
    {
        return armyPopulation;
    }

    public void SetPlayer(Player player_)
    {
        player = player_;
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
    }

    public int ReducePopulation(int populationCount_)
    {
        int remainderAfterAttackers = Mathf.Clamp(populationCount_ - armyPopulation, 0, populationCount_);
        armyPopulation = Mathf.Clamp(armyPopulation - populationCount_, 0, populationCount_);

        return remainderAfterAttackers;
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
    }
}
