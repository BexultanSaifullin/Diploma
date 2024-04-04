using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Boss
{
    public int Attack, Defense, Range;

    public Boss(int attack, int defense, int range)
    {
        Attack = attack;
        Defense = defense;
        Range = range;
    }
    public bool IsAlive
    {
        get
        {
            return Defense > 0;
        }
    }
    public void GetDamage(int dmg)
    {
        Defense -= dmg;
    }
}
public static class BossManagerList
{
    public static List<Boss> AllBoss = new List<Boss>();

}
public class BossManager : MonoBehaviour
{


    public void Awake()
    {
        BossManagerList.AllBoss.Add(new Boss(1, 10, 1));
        BossManagerList.AllBoss.Add(new Boss(0, 10, 0));
    }


}