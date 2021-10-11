using DG.Tweening;
using HuntroxGames.LD49;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

namespace HuntroxGames
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject pressAnyButtonText;
        private bool gameStarted;
        void Start()
        {
            pressAnyButtonText.transform.DOScale(1.09f, 3f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }

        void Update()
        {
            if (!gameStarted)
            {
                var gamepadButtonPressed = Input.anyKeyDown;
                if (gamepadButtonPressed)
                {
                    gameStarted = true;
                    StartCoroutine(OnStartPressed());
                }
            }
        }

        IEnumerator OnStartPressed()
        {
            pressAnyButtonText.GetComponent<TextMeshProUGUI>().DOFade(0, 0.7f).SetEase(Ease.InOutSine);
            yield return gameObject.GetComponent<CanvasGroup>().DOFade(0, 1).SetEase(Ease.InOutSine).WaitForCompletion();
            pressAnyButtonText.transform.DOKill();
            pressAnyButtonText.SetActive(false);
            gameObject.SetActive(false);
            FindObjectOfType<HiringManager>().StartHiring();
            yield return null;
        }
    }
}