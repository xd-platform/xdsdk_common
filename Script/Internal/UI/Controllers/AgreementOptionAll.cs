using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#if UNITY_EDITOR || UNITY_STANDALONE

namespace XD.SDK.Common.PC.Internal
{
    public class AgreementOptionAll : MonoBehaviour
    {
        public Button toggleButton;
        public HyperlinkText label;
        
        private void Awake() 
        {
            toggleButton = transform.Find("OptionButton").GetComponent<Button>();
            label = transform.Find("OptionLabel").GetComponent<HyperlinkText>();
        }

        private void Start()
        {
            
        }

        // 设置 click 回调
        public void AddListener(UnityAction action)
        {
            toggleButton.onClick.AddListener(action);
        }
        
        // 设置 label 文案
        public void SetLabel(string text)
        {
            label.text = text;
        }
    }
}
#endif