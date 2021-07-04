using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterModel
{
    public List<Monster> monsterList;

    public Dictionary<int, Monster> monsterDic;

    public MonsterModel()
    {
        monsterList = new List<Monster>();
        monsterDic = new Dictionary<int, Monster>();   
    }
    // Start is called before the first frame update
    public void InitMonster(string text)
    {
        AllMonster allMonster = JsonUtility.FromJson<AllMonster>(text);
        
        if(allMonster == null)
            Debug.LogError("AllMonsterNull");
        foreach (Monster monster in allMonster.Monsters)
        {
            monsterList.Add(monster);
            monsterDic.Add(monster.Id,monster);
            Debug.Log(monster.GroundAir);
        }

    }
}

[Serializable]
public class AllMonster
{
    public List<Monster> Monsters;
}
[Serializable]
public class Monster
{
    public int Id;
    public int Hp;
    public int Atk;
    public int Def;
    public int Spd;
    public int Type;
    public int GroundAir;
    public int Aoe;
    public int SkillId;
}
