using UnityEngine;

public class Town : MonoBehaviour
{
    [SerializeField]
    Army prefabArmy;

    [SerializeField]
    int townSize;
    [SerializeField]
    float populationGrowth;

    [SerializeField]
    float attackerPopulation;
    [SerializeField]
    float attackerMax;

    [SerializeField]
    float defenderPopulation;
    [SerializeField]
    float defenderMax;

    Player player;


    void Start()
    {
        GameInterface.GetInstance().RegisterTown(this);

        populationGrowth = (float)townSize / 5;
        attackerMax = townSize * 10;
        defenderMax = townSize * 5;
        defenderPopulation = defenderMax;
    }

    void FixedUpdate()
    {
        float populationGrown = populationGrowth * Time.fixedDeltaTime;

        if (attackerPopulation < attackerMax)
        {
            IncreasePopulation(populationGrown);
        }
    }

    public int GetTownSize()
    {
        return townSize;
    }

    public void SetTownSize(int townSize_)
    {
        townSize = townSize_;
        transform.localScale = new Vector3(1 + (townSize / 10), 1 + (townSize / 10), 1 + (townSize / 10));
    }

    public void SetPlayer(Player player_)
    {
        player = player_;
    }

    public Player GetPlayer()
    {
        return player;
    }

    public int GetAttackerPopulation()
    {
        return (int)attackerPopulation;
    }

    public int GetDefenderPopulation()
    {
        return (int)defenderPopulation;
    }

    public int GetTotalPopulation()
    {
        return GetDefenderPopulation() + GetAttackerPopulation();
    }

    public void IncreasePopulation(float populationCount_)
    {
        float defenderOverflow = Mathf.Clamp(defenderPopulation + populationCount_ - defenderMax, 0, populationCount_);
        defenderPopulation = Mathf.Clamp(defenderPopulation + populationCount_, 0, defenderMax);

        attackerPopulation = attackerPopulation + defenderOverflow; // we only care about attackerMax when regenerating population over time
    }

    public float ReducePopulation(float populationCount_)
    {
        float remainderAfterAttackers = Mathf.Clamp(populationCount_ - attackerPopulation, 0, populationCount_);
        attackerPopulation = Mathf.Clamp(attackerPopulation - populationCount_, 0, attackerMax);

        float remainderAfterDefenders = Mathf.Clamp(remainderAfterAttackers - defenderPopulation, 0, populationCount_);
        defenderPopulation = Mathf.Clamp(defenderPopulation - remainderAfterAttackers, 0, attackerMax);

        return remainderAfterDefenders;
    }

    public Army RaiseAttackers()
    {
        if (GetAttackerPopulation() > 0)
        {
            Army army = Instantiate(prefabArmy, transform.position, Quaternion.identity) as Army;
            army.SetPopulation(GetAttackerPopulation());
            attackerPopulation -= GetAttackerPopulation();

            army.SetPlayer(player);
            army.SetTownFrom(this);

            return army;
        }
        return null;
    }

    public bool SendAttack(Town townToAttack_)
    {
        Army army = RaiseAttackers();

        if (army != null)
        {
            army.SetTownToAttack(townToAttack_);
            return true;
        }

        return false;
    }

    public static void AttackTown(Town town_, Army army_)
    {
        if(town_.GetPlayer() == army_.GetPlayer()) // reinforce
        {
            town_.IncreasePopulation(army_.GetPopulation());
            Destroy(army_.gameObject);
        }
        else // attack
        {
            float remainder = town_.ReducePopulation(army_.GetPopulation());

            if(remainder > 0)
            {
                town_.SetPlayer(army_.GetPlayer());
                town_.IncreasePopulation(remainder);
            }

            Destroy(army_.gameObject);
        }
    }
}
