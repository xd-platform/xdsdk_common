using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#if UNITY_EDITOR || UNITY_STANDALONE

namespace XD.SDK.Common.PC.Internal
{
    public class AgreementOptionNormal : MonoBehaviour
    {
        public string optionName;
        public Toggle toggle;
        public HyperlinkText label;
        
        private void Awake() 
        {
            toggle = transform.Find("OptionToggle").GetComponent<Toggle>();
            label = transform.Find("OptionLabel").GetComponent<HyperlinkText>();
        }

        // 设置 click 回调
        public void AddListener(UnityAction<bool> action)
        {
            toggle.onValueChanged.AddListener(action);
        }
        
        // 设置 label 文案
        public void SetLabel(string text)
        {
            label.text = text;
        }
    }
}
#endif