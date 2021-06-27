using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using tower;
using UnityEngine;

public class ResourcesPoint : MonoBehaviour
{
    public ResType resType;
    public float digRange;
    public int resCount;
    
    private BuildRes _buildRes;
    private bool _canDig;
    private bool _onDig;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = VTool.ToTilePos(transform.position);
        EventCenter.GetInstance().AddEventListener("GameManagerInit",InitResPoint);
    }

    void InitResPoint()
    {
        _buildRes = AssestMgr.Instance.LoadBuildRes(resType);
        StartCoroutine(DigRes());
    }
    void FixedUpdate()
    {
        CheckAround();
        if (!_onDig && _canDig)
            StartCoroutine(DigRes());
    }

    void CheckAround()
    {
        Collider2D player = Physics2D.OverlapBox(transform.position, new Vector2(digRange,digRange),0, LayerMask.GetMask("Player"));
        _canDig = player != null;
    }

    IEnumerator DigRes()
    {
        _onDig = true;
        while (_canDig && resCount > 0)
        {
            resCount--;
            PlayerPackage.Instance.AddResNums(resType, 1);
            yield return new WaitForSeconds(_buildRes.digTime);
        }
        _onDig = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(digRange,digRange,0));
    }
}
