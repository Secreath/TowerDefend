using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShineImage : MonoBehaviour
{
    public Transform player;
    
    public float minAlpha;
    public float maxAlpha;
    public float loopTime;
    public bool isLoop;
    
    
    private SpriteRenderer sp;
    void Start()
    {
//        player = transform.parent;
        sp = transform.GetComponent<SpriteRenderer>();

        StartCoroutine(StartBreath());
    }
 
    void Update()
    {
        Vector3 pointPos = VTool.ToTilePos(player.position);
        transform.position = pointPos;
        
    }

    private IEnumerator StartBreath()
    {
        float eachTime = loopTime;
        float alpha = minAlpha;
        Color color = sp.color;
        
        sp.color = ColorTool.ChangeAlpha(color, minAlpha);
        do
        {
            alpha += 0.001f;
            
            sp.color = ColorTool.ChangeAlpha(color, alpha);
            yield return new WaitForSeconds(eachTime);
        } while (alpha>=maxAlpha);

        alpha = maxAlpha;
        
        sp.color = ColorTool.ChangeAlpha(color, maxAlpha);
        do
        {
            alpha -= 0.02f;
            sp.color = ColorTool.ChangeAlpha(color, alpha);
            yield return new WaitForSeconds(eachTime);
        } while (alpha <= minAlpha);
        
        sp.color = ColorTool.ChangeAlpha(color, minAlpha);

        if (isLoop)
            StartCoroutine(StartBreath());
    }
}
