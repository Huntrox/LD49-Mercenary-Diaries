using DG.Tweening;
using HuntroxGames.Utils;
using UnityEngine;


//[System.Serializable]
//[RequireComponent(typeof(ExtendedButton))]
//public class ExtendedButtonTweening  : MonoBehaviour
//{

//#pragma warning disable 649
//    [SerializeField] private DoScaleVars OnEnterAnimation;
//    [SerializeField] private DoScaleVars OnExitAnimation;
//    [SerializeField] private DoPunchScaleVars OnClickAnimation;
//    private Vector3 currentSize;
//#pragma warning restore 649

//	private void OnEnable()
//	{
//        GetComponent<ExtendedButton>().onClick.AddListener(OnPointerDown);
//        GetComponent<ExtendedButton>().onEnter.AddListener(OnPointerEnter);
//        GetComponent<ExtendedButton>().onExit.AddListener(OnPointerExit);
//    }
//	private void OnDisable()
//	{
//        GetComponent<ExtendedButton>().onClick.RemoveListener(OnPointerDown);
//        GetComponent<ExtendedButton>().onEnter.RemoveListener(OnPointerEnter);
//        GetComponent<ExtendedButton>().onExit.RemoveListener(OnPointerExit);
//    }
//	public void OnPointerDown()
//    {
//        transform.DOComplete();
//        transform.DOPunchScale(OnClickAnimation.punchScale,
//            OnClickAnimation.Duration,
//            OnClickAnimation.Vibrato,
//            OnClickAnimation.Elasticity).
//            SetEase(OnClickAnimation.ease).OnComplete(() => transform.localScale = currentSize);
//    }
//    public void OnPointerEnter()
//    {
//        transform.DOScale(OnEnterAnimation.EndScale, OnEnterAnimation.Duration).SetEase(OnEnterAnimation.ease);
//        currentSize = Vector3.one * OnEnterAnimation.EndScale;
//    }
//    public void OnPointerExit()
//    {
//        transform.DOScale(OnExitAnimation.EndScale, OnExitAnimation.Duration).SetEase(OnExitAnimation.ease);
//        currentSize = Vector3.one * OnExitAnimation.EndScale;
//    }
//}