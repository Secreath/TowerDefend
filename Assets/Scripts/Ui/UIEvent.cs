using System.Collections;
using System.Collections.Generic;
using Tower;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public enum UiType
    {
        ChooseBtn,
        UpGradeBtn
    }

    public class UIEvent : Singleton<UIEvent>
    {

        private RectTransform chooseTowerPanel;
        private RectTransform settingPanel;
        private RectTransform upGradeTowerPanel;
        
        private void Start()
        {
            chooseTowerPanel = transform.Find("ChooseTower") as RectTransform;
            settingPanel = transform.Find("Setting") as RectTransform;
            upGradeTowerPanel = transform.Find("UpGradeTower") as RectTransform;
        }

        public void Init()
        {
            InitChooseTowerPanel();
        }


        private void InitUpGradePanel(TowerType type)
        {
            chooseTowerPanel.gameObject.SetActive(false);
            upGradeTowerPanel.anchoredPosition = chooseTowerPanel.anchoredPosition;
            
            StartCoroutine(SetUpGradeUI(upGradeTowerPanel, type));

        }

        private IEnumerator SetUpGradeUI(RectTransform parent, TowerType type)
        {
            if (!GameManager.Instance.HadThisTowerType(type))
                yield break;
            
            string uiType = UiType.UpGradeBtn.ToString();
            ResourceRequest uiBtn = Resources.LoadAsync<GameObject>("UI/" + uiType + "/" + type.ToString() + uiType);
            yield return uiBtn;
            
            GameObject upGradeBtn = Instantiate(uiBtn.asset as GameObject);
            GameObject tower = Instantiate(GameManager.Instance.GetTowerByType(type)); 

            Button btn = upGradeBtn.GetComponent<Button>();
            BaseTower baseTower = tower.GetComponent<BaseTower>();
            
            upGradeBtn.name = type.ToString() + "Btn";
            upGradeBtn.transform.SetParent(upGradeTowerPanel, false);
            upGradeTowerPanel.gameObject.SetActive(false);

            tower.SetActive(true);
            tower.transform.position = UiManager.Instance.curPoint.CenterPos;
            tower.GetComponent<SpriteRenderer>().sortingOrder = UiManager.Instance.curPoint.Y;
            
            btn.onClick.AddListener(()=>
            {
                baseTower.UpGrade();    
            });
            UiManager.Instance.curPoint.BuildTower(baseTower);
//            UiManager.Instance.curPoint = baseTower;
        }

        private void InitChooseTowerPanel()
        {
            StartCoroutine(InitCircle());
        }

        public IEnumerator InitCircle(float startAngle = 58.12f)
        {
            string typeName = UiType.ChooseBtn.ToString();
            List<TowerType> chooseTypes = UiManager.Instance.chooseTypes;
            int count = chooseTypes.Count;

            float radius = 50;
            List<GameObject> btnObjs = new List<GameObject>();

            for (int i = 0; i < count; i++)
            {
                ResourceRequest rr =
                    Resources.LoadAsync<GameObject>("UI/" + typeName + "/" + chooseTypes[i].ToString() + typeName);
                yield return rr;
                if (rr.asset == null)
                    continue;
                rr.asset.name = chooseTypes[i] + "Btn";
                btnObjs.Add(rr.asset as GameObject);
            }

            float deltaTheta = (2f * Mathf.PI) / count;
            float theta = startAngle; //当前角度
            for (var i = 0; i < btnObjs.Count; i++)
            {
                Vector2 pos = chooseTowerPanel.anchoredPosition +
                              new Vector2(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta));
                GameObject obj = Instantiate(btnObjs[i]);
                theta = deltaTheta + theta;

                RectTransform rect = obj.transform as RectTransform;
                rect.SetParent(chooseTowerPanel, false);
                rect.gameObject.SetActive(true);
                rect.anchoredPosition = pos;

                
                TowerType type = chooseTypes[i];
                Button btn = obj.GetComponent<Button>();
                btn.onClick.AddListener(() =>
                {
                    InitUpGradePanel(type);
                });
            }

            chooseTowerPanel.gameObject.SetActive(false);



        }
    }

}
