using System.Collections;
using System.Collections.Generic;
using GameUi;
using tower;
using UnityEngine;

public class EnemyUiManager : MonoBehaviour
{
    private Camera UiCamera;
    
    private Transform charUiBars;

    private GameObject charUiObj;
    private Stack<CharacterUiMgr> charUiStack;
    
    // Start is called before the first frame update
    void Start()
    {
        charUiBars = transform.Find("CharUiBars");
        charUiObj = transform.Find("OriCharUiObj").gameObject;
        charUiStack = new Stack<CharacterUiMgr>();       
    }

    public void SetUiCamera(Camera uiCamera)
    {
        UiCamera = uiCamera;
    }
    public CharacterUiMgr PopCharUiBar()
    {
        Debug.Log("GetUi");
        CharacterUiMgr charUi;
        if (charUiStack.Count > 1)
        {
            charUi = charUiStack.Pop();
            charUi.SetActive(true);
            return charUi;
            
        }

        GameObject NewCharUi = Instantiate(charUiObj, charUiBars);
        NewCharUi.SetActive(true);
        charUi = NewCharUi.GetComponent<CharacterUiMgr>();
        charUi.SetUiCamera(UiCamera);
        return charUi;
    }
    
    public void PushCharUiBar(CharacterUiMgr charUi)
    {
        charUi.ReSetUi();   
        charUi.SetActive(false);
        charUiStack.Push(charUi);
    }
}
