#if UNITY_EDITOR || UNITY_STANDALONE
using UnityEngine;
using UnityEngine.UI;

namespace XD.SDK.Common.PC.Internal {
    public class OptionAgreementAlert : GeneralAgreementAlert {
        protected Toggle optionToggle;

        protected override void Awake() {
            base.Awake();
            optionToggle = GameObject.Find("OptionToggle").GetComponent<Toggle>();
        }

        protected override void Start() {
            Text label = optionToggle.transform.Find("Label").GetComponent<Text>();
            string text = Extra["option"] as string;

            // 计算字符宽度
            TextGenerator generator = label.cachedTextGenerator;
            TextGenerationSettings settings = label.GetGenerationSettings(Vector2.zero);
            // Windows 下有误差
            float width = generator.GetPreferredWidth(text, settings) / label.pixelsPerUnit + 40;
            float height = 20;

            RectTransform panelRectTransform = gameObject.GetComponent<RectTransform>();
            float maxWidth = panelRectTransform.rect.width - 30;

            RectTransform toggleRectTransform = optionToggle.GetComponent<RectTransform>();

            if (width > maxWidth) {
                width = maxWidth;
                height = 40;
            }

            toggleRectTransform.sizeDelta = new Vector2(width, height);

            label.text = text;

            LayoutRebuilder.ForceRebuildLayoutImmediate(toggleRectTransform);

            base.Start();
        }
    }
}
#endif
