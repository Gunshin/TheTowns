using UnityEngine;
using System.Collections.Generic;

public class Town : MonoBehaviour
{
    [SerializeField]
    Army prefabArmy;

    GameObject selectionTorus;

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

    [SerializeField]
    TextMesh details;

    Player player;

    float testingMultiplier = 1;

    void Start()
    {
        selectionTorus = transform.FindChild("SelectionTorus").gameObject;



        populationGrowth = (float)townSize / 5 * testingMultiplier;
        attackerMax = townSize * 10;
        defenderMax = townSize * 5;
        defenderPopulation = defenderMax;

        transform.localScale = new Vector3(townSize, townSize, townSize);
    }

    void FixedUpdate()
    {
        float populationGrown = populationGrowth * Time.fixedDeltaTime;

        if (attackerPopulation < attackerMax)
        {
            IncreasePopulation(populationGrown);
        }

        if(GetTotalPopulation() >= attackerMax + defenderMax)
        {
            OnOverpopulated();
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
        GameInterface.GetInstance().UnregisterTown(this);
        player = player_;
        GameInterface.GetInstance().RegisterTown(this);

        GetComponent<MeshRenderer>().material.color = player.GetColour();

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

    public int GetMaxPopulation()
    {
        return (int)(attackerMax + defenderMax);
    }

    public void IncreasePopulation(float populationCount_)
    {
        bool hadMaxDefenders = GetDefenderPopulation() == defenderMax;

        float defenderOverflow = Mathf.Clamp(defenderPopulation + populationCount_ - defenderMax, 0, populationCount_);
        defenderPopulation = Mathf.Clamp(defenderPopulation + populationCount_, 0, defenderMax);

        if (GetDefenderPopulation() == defenderMax && !hadMaxDefenders)
        {
            OnMaxDefenders();
        }

        bool hadMaxAttackers = GetAttackerPopulation() == attackerMax;
        int aBefore = Mathf.FloorToInt(attackerPopulation);

        attackerPopulation = attackerPopulation + defenderOverflow; // we only care about attackerMax when regenerating population over time

        int aAfter = Mathf.FloorToInt(attackerPopulation);

        if(aAfter - aBefore >= 1)
        {
            OnAttackerPopulationIncreased();
        }

        if(GetAttackerPopulation() == attackerMax && !hadMaxAttackers) // max attackers means max population
        {
            OnMaxPopulation();
        }

        details.text = "Offensive: " + ((int)attackerPopulation) + "\nDefensive: " + ((int)defenderPopulation);
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
        if(townToAttack_ == this)
        {
            return false;
        }

        Army army = RaiseAttackers();

        if (army != null)
        {
            army.SetTownToAttack(townToAttack_);
            if (townToAttack_.GetPlayer() == this.GetPlayer())
            {
                OnReinforce(townToAttack_, army);
            }
            else
            {
                OnAttack(townToAttack_, army);
            }
            return true;
        }

        return false;
    }

    public static void AttackTown(Town town_, Army army_)
    {
        if (town_.GetPlayer() == army_.GetPlayer()) // reinforce
        {
            town_.IncreasePopulation(army_.GetPopulation());
            Destroy(army_.gameObject);
        }
        else // attack
        {
            float remainder = town_.ReducePopulation(army_.GetPopulation());

            if (remainder > 0)
            {
                town_.SetPlayer(army_.GetPlayer());
                town_.IncreasePopulation(remainder);
            }

            Destroy(army_.gameObject);
        }
    }

    public void Select()
    {
        selectionTorus.SetActive(true);
    }

    public void Deselect()
    {
        selectionTorus.SetActive(false);
    }

    void OnAttack(Town target_, Army army_)
    {
        EventManager.instance.Activate((int)GameInterface.Events.Attack, new List<object>
        {
            this,
            target_,
            army_
        });
    }

    void OnMaxDefenders()
    {
        EventManager.instance.Activate((int)GameInterface.Events.MaxDefenders, new List<object>
        {
            this,
            GetDefenderPopulation()
        });
    }

    void OnMaxPopulation()
    {
        EventManager.instance.Activate((int)GameInterface.Events.MaxPopulation, new List<object>
        {
            this,
            GetTotalPopulation()
        });
    }

    void OnReinforce(Town target_, Army army_)
    {
        EventManager.instance.Activate((int)GameInterface.Events.Reinforce, new List<object>
        {
            this,
            target_,
            army_
        });
    }

    void OnOverpopulated()
    {
        EventManager.instance.Activate((int)GameInterface.Events.OverPopulated, new List<object>
        {
            this,
            GetTotalPopulation()
        });
    }

    void OnAttackerPopulationIncreased()
    {
        EventManager.instance.Activate((int)GameInterface.Events.AttackerPopulationIncreased, new List<object>
        {
            this,
            GetTotalPopulation()
        });
    }
}
