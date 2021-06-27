using System;
using System.Collections;
using System.Collections.Generic;
using tower;
using ui;
using UnityEngine;

public class PlayerPackage : Singleton<PlayerPackage>
{
    private int coinNums;
    private int woodNums;
    private int waterNums;
    private int steelNums;
    private int goldenNums;

    public int CoinNums
    {
        get { return coinNums; }
        set
        {
            coinNums = value;
            ChangePackageUi(ResType.Coin,coinNums);
        }
    }
    public int WoodNums
    {
        get { return woodNums; }
        set
        {
            woodNums = value;
            ChangePackageUi(ResType.Wood,woodNums);
        }
    }
    
    public int WaterNums
    {
        get { return waterNums; }
        set
        {
            waterNums = value;
            ChangePackageUi(ResType.Water,waterNums);
        }
    }
    
    public int SteelNums
    {
        get { return steelNums; }
        set
        {
            steelNums = value;
            ChangePackageUi(ResType.Steel,steelNums);
        }
    }
    
    public int GoldenNums
    {
        get { return goldenNums; }
        set
        {
            goldenNums = value;
            ChangePackageUi(ResType.Golden,goldenNums);
        }
    }
    
    private void Start()
    {
        EventCenter.GetInstance().AddEventListener("GameManagerInit",InitGame);
    }

    private void InitGame()
    {
        CoinNums = 500;
        WoodNums = 0;
        WaterNums = 0;
        SteelNums = 0;
        GoldenNums = 0;
    }

    public void AddResNums(ResType res, int num)
    {
        switch (res)
        {
            case ResType.Coin:
                CoinNums += num;
                break;
            case ResType.Wood:
                WoodNums += num;
                break;
            case ResType.Water:
                WaterNums += num;
                break;
            case ResType.Steel:
                SteelNums += num;
                break;
            case ResType.Golden:
                GoldenNums += num;
                break;
        }
    }
    private void ChangePackageUi(ResType res,int num)
    {
        UiManager.Instance.ChangePackageUi(res,num);
    }
}
