using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace plugin.ugui.view
{
    public class TimeCountDown : MonoBehaviour
    {
        public bool useSimple = false;
        private Text _timeText;
        private Image _image;
        private UnityAction _endAction;
        private int _ms;
        

        private void Awake()
        {
            _timeText = transform.Find("Text").GetComponent<Text>();
            _image = transform.Find("Image").GetComponent<Image>();
            _timeText.resizeTextForBestFit = true;
        }

        private void OnEnable()
        {
            StartRefreshTime();
        }
        
        public void SetRefreshTime(float second,UnityAction endAction)
        {
            _ms = (int) (second * 1000f);
            gameObject.SetActive(true);
            _endAction = endAction;
        }
        
        private void StartRefreshTime()
        {
            _timeText.text = TimeTool.ToTimeStr(_ms);
            
            StartCoroutine(UpdateTimeText(_ms));
        }
        
        private  IEnumerator UpdateTimeText(int time)
        {
            time -= 10;
            if(time >=0)
            {
                if(useSimple)
                    _timeText.text = TimeTool.MsToTimeStrSimple(time);
                else
                    _timeText.text = TimeTool.MsToTimeStr(time);
                
                _image.fillAmount = (float) time / _ms;
                yield return new WaitForSeconds(0.01f);
                StartCoroutine(UpdateTimeText(time));                
            }
            else
            {
                TimeCountOver();
            }
        }

        private void TimeCountOver()
        {
            if (_endAction != null)
                _endAction();
            gameObject.SetActive(false);
            
        }
    }
    
}
