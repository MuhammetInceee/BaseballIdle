using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Baseball Idle/Player/Player Attribute Settings"), fileName = ("PlayerAttributeSettings"))]
public class UpgradableAttributeSettings : ScriptableObject
{
    [Header("Player Attribute Upgrades: "), Space]
    public List<float> playerMovementSpeedList = new List<float>();
    public List<float> playerCarryCapacityList = new List<float>();
    public List<float> incomeList = new List<float>();

    [Header("Upgrade Table Costs: "), Space]
    public List<int> speedCost;
    public List<int> capacityCost;
    public List<int> incomeCost;
}
