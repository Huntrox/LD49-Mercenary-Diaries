using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace HuntroxGames.LD49
{
    public class Tooltip : MonoBehaviour
    {

        [SerializeField] private int contentwrapLimit = 45;
        [SerializeField] private float offset = 12;
        private TextMeshProUGUI headerText;
        private TextMeshProUGUI contentText;
        private TextMeshProUGUI priceText;
        private LayoutElement layoutElement;
        private RectTransform rectTransform;
        private RectTransform bgRectTransform;
        private RectTransform canvas;
        private Transform header_trans;

        private Image img;

        private void Start()
        {
            canvas = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
            rectTransform = GetComponent<RectTransform>();
            bgRectTransform = transform.Find("Tooltip").GetComponent<RectTransform>();
            headerText = transform.Find("Tooltip/header").Find("HeaderText").GetComponent<TextMeshProUGUI>();
            header_trans = transform.Find("Tooltip/header");
            contentText = transform.Find("Tooltip").Find("ContentText").GetComponent<TextMeshProUGUI>();
            priceText = transform.Find("Tooltip").Find("priceText").GetComponent<TextMeshProUGUI>();
            layoutElement = transform.Find("Tooltip").GetComponent<LayoutElement>();
        }

        public void Show(string header, string content, string price)
        {

            header_trans.gameObject.SetActive(!string.IsNullOrEmpty(header));
            headerText.gameObject.SetActive(!string.IsNullOrEmpty(header));
            contentText.gameObject.SetActive(!string.IsNullOrEmpty(content));
            priceText.gameObject.SetActive(!string.IsNullOrEmpty(price));


            headerText.text = header;
            contentText.text = content;
            priceText.text = "Cost: "+price+"$";
            int headerLength = string.IsNullOrEmpty(content) ? 0 : headerText.text.Length;
            int contentLength = string.IsNullOrEmpty(content) ? 0 : contentText.text.Length;

            layoutElement.enabled = headerLength > contentwrapLimit || contentLength > contentwrapLimit;
        }
        void Update()
        {
            Vector2 anchPos;
            Vector2 _offset = (Vector2.one * offset);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, Input.mousePosition, null, out anchPos);
            Vector2 pivot = new Vector2(0, 0);
            if (anchPos.x + bgRectTransform.rect.width > (canvas.rect.width / 2))
            {
                pivot.x = 1;
                _offset.x = -offset;
            }
            if (anchPos.y + bgRectTransform.rect.height > (canvas.rect.height / 2))
            {
                pivot.y = 1;
                _offset.y = -offset;
            }
            rectTransform.pivot = pivot;
            bgRectTransform.pivot = pivot;
            bgRectTransform.localPosition = Vector2.zero;
            rectTransform.anchoredPosition = anchPos + _offset;
        }
    }
}