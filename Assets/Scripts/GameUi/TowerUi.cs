using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameUi
{
    public class TowerUi : MonoBehaviour
    {
        public List<GameObject> towerGroups;

        private Transform _towerBtnGroup;
        private Transform _basement;
        private Button _makeTower;

        void Start()
        {
            _basement = transform.parent;
            _towerBtnGroup = transform.Find("TowerGroup");
            _makeTower = transform.Find("MakeTower").GetComponent<Button>();
            _makeTower.onClick.AddListener(() =>
            {
                bool active = !_towerBtnGroup.gameObject.activeSelf;
                _towerBtnGroup.gameObject.SetActive(active);
            });
            TowerGroupEvent();
        }

        private void TowerGroupEvent()
        {
            for (int i = 0; i < _towerBtnGroup.childCount && i<towerGroups.Count; i++)
            {
                GameObject buildTower = _towerBtnGroup.transform.GetChild(i).gameObject;
                buildTower.SetActive(true);
                int tempI = i;
                buildTower.GetComponent<Button>().onClick.AddListener(delegate
                {
                    GameObject tower = Instantiate(towerGroups[tempI],_basement);
                    tower.transform.localPosition = Vector3.zero;
                    _makeTower.gameObject.SetActive(false);
                    _towerBtnGroup.gameObject.SetActive(false);
                });
                
            }
        }
    }
    
}
