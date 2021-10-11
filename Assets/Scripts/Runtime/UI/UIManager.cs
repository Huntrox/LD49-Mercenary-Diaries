using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using System;
using HuntroxGames.Utils;
using System.Net.Http.Headers;

namespace HuntroxGames.LD49
{
	public class UIManager : MonoBehaviour
    {
        [Header("Book")]
        [SerializeField] private Transform bookUI;
        [Header("Book/Tabs")]
        [SerializeField] private Transform mercDisplyTab;
        [SerializeField] private Transform contractsTab;
        [Header("Book/Tab/Squad&Hiring")]
        [SerializeField] private TextMeshProUGUI mercName;
        [SerializeField] private TextMeshProUGUI mercDesc;
        [SerializeField] private TextMeshProUGUI mercLevel;
        [SerializeField] private TextMeshProUGUI mercRiskLevel;

        [SerializeField] private TextMeshProUGUI pageText;

        [SerializeField] private TextMeshProUGUI remainingTime;

        [SerializeField] private Image mercPortriat;

        [SerializeField] private Image mercHealthFill;
        [SerializeField] private Image mercEffiFill;
        [SerializeField] private Image mercMsFill;

        [SerializeField] private TextMeshProUGUI healthValue;
        [SerializeField] private TextMeshProUGUI msValue;
        [SerializeField] private TextMeshProUGUI effeciencyValue;

        [SerializeField] private Button prevPage;

	
		[SerializeField] private Button nextPage;
        [SerializeField] private Button hireBtn;
        [SerializeField] private TextMeshProUGUI warningText;
        [Header("Book/Animation")]
        [SerializeField] private float fillDuration = 0.2f;
        [SerializeField] private float textDuration = 0.2f;
        [SerializeField] private float fadeDuration = 0.2f;
        [SerializeField] private float uiAnimaionDuration = 0.2f;
        [SerializeField] private AnimationCurve uiAnimationCurve;

        [Header("Book/Tab/Contracts")]
        [SerializeField] private TextMeshProUGUI contTitle;
        [SerializeField] private TextMeshProUGUI contDesc;
        [SerializeField] private TextMeshProUGUI contRecLevel;
        [SerializeField] private TextMeshProUGUI contExp;
        [SerializeField] private TextMeshProUGUI contCash;
        [SerializeField] private TextMeshProUGUI contDifficulty;
        [SerializeField] private TextMeshProUGUI contTimer;
        [SerializeField] private Button acceptBtn;
        [Header("Contract Paper")]
        [SerializeField] private Transform contPaper;
        [SerializeField] private TextMeshProUGUI contPaperTitle;
        [SerializeField] private TextMeshProUGUI contPaperDesc;
        [SerializeField] private Button contPaperAcceptBtn;
        [SerializeField] private Button contPaperCancelBtn;
        [Header("Contract Paper")]
        [SerializeField] private Transform logPaper;
        [SerializeField] private TextMeshProUGUI logPaperTitle;
        [SerializeField] private TextMeshProUGUI logDesc;
        [SerializeField] private Button logPaperOkBtn;
        [SerializeField] private Button logPaperNextBtn;
        [SerializeField] private Button logPaperPrevBtn;
        [Header("InventoryUI")]
        [SerializeField] private Transform inventoryparent;
        [SerializeField] private Transform inventoryContainer;
        [SerializeField] private GameObject itemUIPrefab;

        [Header("Tooltip")]
        [SerializeField] private float tooltip_anim_duration = 0.2f;
        [SerializeField] private AnimationCurve tooltip_anim_curve;
        private Tooltip tooltip;
        private RectTransform tooltipRectTransform;
        private Tween tooltiptween;
        [Header("Popup")]
        [SerializeField] private Transform popupsContainer;
        [SerializeField] private Transform popupPrefab;
        [SerializeField] private float popupDelay = 3f;
        [Header("SummonedMercenary")]
        [SerializeField] private Transform mercContainer;
        [SerializeField] private GameObject mercPrefab;
        [SerializeField] TextMeshProUGUI currency;
        [Header("HowToPlay")]
        [SerializeField] Transform htpPaper;
        [SerializeField] TextMeshProUGUI howToPlayText;
        [SerializeField] TextMeshProUGUI howToPlayTitleText;
        [SerializeField] private Button htpPaperNextBtn;
        [SerializeField] private Button htpPaperPrevBtn;
        [SerializeField] List<HowToPlay> howToPlays;


        private int currentLog;
        private int currentHtp;
        private int currentIndex;


        private SquadManager squadManager;
        private HiringManager hiringManager;
        private ContractsManager contractsManager;
        private MercenaryTaskSelector mercenaryTaskSelector;


        private SummonedMercenary summoned;
        void Start()
        {

            squadManager = GetComponent<SquadManager>();
            hiringManager = GetComponent<HiringManager>();
            contractsManager = GetComponent<ContractsManager>();
            mercenaryTaskSelector = GetComponent<MercenaryTaskSelector>();
            tooltip = FindObjectOfType<Tooltip>();
            tooltipRectTransform = tooltip.GetComponent<RectTransform>();
            SetupInventory();
            UpdateCurrency();
            SetupHowToPlay();
        }

        void Update()
        {

        }

        public void SquadTab()
        {

            prevPage.onClick.RemoveAllListeners();
            nextPage.onClick.RemoveAllListeners();
            hireBtn.onClick.RemoveAllListeners();
            prevPage.gameObject.SetActive(false);
            nextPage.gameObject.SetActive(false);

            hireBtn.gameObject.SetActive(false);

            contractsTab.gameObject.SetActive(false);

            currentIndex = 0;

            hireBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Summon";

            if (squadManager.CurrentSquadCount == 0)
            {
                mercDisplyTab.gameObject.SetActive(false);
                warningText.gameObject.SetActive(true);
                warningText.text = "You don't have Mercenaries, Go to hiring tab and hire some";
                pageText.text = "";
            }
            else
            {

                prevPage.gameObject.SetActive(true);
                nextPage.gameObject.SetActive(true);

                prevPage.onClick.AddListener(() =>
                {
                    currentIndex = (currentIndex - 1) < 0 ? squadManager.CurrentSquadCount - 1 : (currentIndex - 1);
                    DisplayMercenaryInfo(squadManager.GetMercenary(currentIndex), squadManager.CurrentSquadCount,true);
                    hireBtn.gameObject.SetActive(!squadManager.GetMercenary(currentIndex).IsOnATask);

                });

                nextPage.onClick.AddListener(() =>
                {
                    currentIndex = (currentIndex + 1) % squadManager.CurrentSquadCount;
                    DisplayMercenaryInfo(squadManager.GetMercenary(currentIndex), squadManager.CurrentSquadCount, true);
                    hireBtn.gameObject.SetActive(!squadManager.GetMercenary(currentIndex).IsOnATask);
                });

				hireBtn.onClick.AddListener(SummonMercenary);
                DisplayMercenaryInfo(squadManager.GetMercenary(currentIndex), squadManager.CurrentSquadCount, true);
                hireBtn.gameObject.SetActive(!squadManager.GetMercenary(currentIndex).IsOnATask);
                mercDisplyTab.gameObject.SetActive(true);
                warningText.gameObject.SetActive(false);
            }
        }

		private void SummonMercenary()
		{
			var merc=  squadManager.SummonMercenary(currentIndex);
            if (merc != null)
            {
                ShowInventory();
                var go = Instantiate(mercPrefab, mercContainer);
                go.GetComponent<CanvasGroup>().DOFade(1,fadeDuration);
                summoned = go.GetComponent<SummonedMercenary>();
                summoned.Init(merc);
            }
		}
        public void DismissMercenary()
		{
            if (summoned)
            {
                summoned.Dismiss();
                summoned = null;
                HideInventory();
            }
        }

        public void HiringTab()
        {
            prevPage.onClick.RemoveAllListeners();
            nextPage.onClick.RemoveAllListeners();
            hireBtn.onClick.RemoveAllListeners();
            prevPage.gameObject.SetActive(false);
            nextPage.gameObject.SetActive(false);
            hireBtn.gameObject.SetActive(false);
            contractsTab.gameObject.SetActive(false);

            currentIndex = 0;

            if (hiringManager.MercenariesCount == 0)
            {
                mercDisplyTab.gameObject.SetActive(false);
                warningText.gameObject.SetActive(true);
                warningText.text = "No Mercenaries currently available, try again later";
                pageText.text = "";
            }
            else
            {

                prevPage.gameObject.SetActive(true);
                nextPage.gameObject.SetActive(true);

                prevPage.onClick.AddListener(() =>
                {
                    currentIndex = (currentIndex - 1) < 0 ? hiringManager.MercenariesCount - 1 : (currentIndex - 1);
                    DisplayMercenaryInfo(hiringManager.GetMercenary(currentIndex), hiringManager.MercenariesCount,false);
                    hireBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Hire for " + hiringManager.GetMercenary(currentIndex).value + "$";
                });

                nextPage.onClick.AddListener(() =>
                {
                    currentIndex = (currentIndex + 1) % hiringManager.MercenariesCount;
                    DisplayMercenaryInfo(hiringManager.GetMercenary(currentIndex), hiringManager.MercenariesCount, false);
                    hireBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Hire for " + hiringManager.GetMercenary(currentIndex).value + "$";
                });

                hireBtn.onClick.AddListener(() =>
                {
                    hiringManager.TryHireMercenary(currentIndex);
                    HiringTab();
                });
                DisplayMercenaryInfo(hiringManager.GetMercenary(currentIndex), hiringManager.MercenariesCount, false);
                hireBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Hire for " + hiringManager.GetMercenary(currentIndex).value + "$";

                hireBtn.gameObject.SetActive(true);
                mercDisplyTab.gameObject.SetActive(true);
                warningText.gameObject.SetActive(false);
            }
        }


        public void ContractsTab()
        {
            prevPage.onClick.RemoveAllListeners();
            nextPage.onClick.RemoveAllListeners();
            hireBtn.onClick.RemoveAllListeners();
            acceptBtn.onClick.RemoveAllListeners();
            prevPage.gameObject.SetActive(false);
            nextPage.gameObject.SetActive(false);
            hireBtn.gameObject.SetActive(false);
            acceptBtn.gameObject.SetActive(false);

            mercDisplyTab.gameObject.SetActive(false);

            currentIndex = 0;

            if (contractsManager.ContractsCount == 0)
            {

                contractsTab.gameObject.SetActive(false);
                warningText.gameObject.SetActive(true);
                warningText.text = "No Contracts currently available, try again later";
                pageText.text = "";
                //TODO display contracts cooldown
            }
            else
            {


                prevPage.gameObject.SetActive(true);
                nextPage.gameObject.SetActive(true);

                prevPage.onClick.AddListener(() =>
                {
                    currentIndex = (currentIndex - 1) < 0 ? contractsManager.ContractsCount - 1 : (currentIndex - 1);
                    DisplayContractInfo(contractsManager.GetContract(currentIndex), contractsManager.ContractsCount);
                    //hireBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Hire for " + hiringManager.GetMercenary(currentIndex).value + "$";
                });

                nextPage.onClick.AddListener(() =>
                {
                    currentIndex = (currentIndex + 1) % contractsManager.ContractsCount;
                    DisplayContractInfo(contractsManager.GetContract(currentIndex), contractsManager.ContractsCount);
                    //hireBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Hire for " + hiringManager.GetMercenary(currentIndex).value + "$";
                });

                acceptBtn.onClick.AddListener(() =>
                {
                    var signedContract = contractsManager.TryAcceptContract(currentIndex);
                    if (signedContract != null)
                    {
                        HideTheBook(() =>
                        {
                            mercenaryTaskSelector.SelectMercenaries(signedContract);
                            ShowContractPaper(signedContract.contract);
                        });
                    }
                    else
                    {
                        SendPopup("You don't have any mercenaries available");
                    }
                });

                DisplayContractInfo(contractsManager.GetContract(currentIndex), contractsManager.ContractsCount);

                acceptBtn.gameObject.SetActive(true);
                contractsTab.gameObject.SetActive(true);
                warningText.gameObject.SetActive(false);
            }
        }
        private void DisplayMercenaryInfo(Mercenary mercenary, int count, bool v)
        {
            if (mercenary == null)
            {
                return;
            }

            mercName.DOKill();
            mercDesc.DOKill();


            mercName.text = mercenary.mercenaryName;
            //mercName.DOText(mercenary.mercenaryName, textDuration).SetEase(Ease.InOutSine);
            mercDesc.DOText(mercenary.mercenaryDesc, textDuration).SetEase(Ease.InOutSine);
            mercPortriat.sprite = mercenary.portrait;

            healthValue.text = mercenary.currentHealth + "/" + mercenary.MaxHealth;
            msValue.text = mercenary.currMentalStability + "/" + mercenary.mentalStability;
            effeciencyValue.text = mercenary.efficiency + "/" + GlobalSettings.MAX_MECENARY_EFFECIENCE;

            mercHealthFill.DOFill(mercenary.CurrentHealthPerc, fillDuration).SetEase(Ease.InOutSine);
            mercMsFill.DOFill(mercenary.CurrentMentalStabilityPerc, fillDuration).SetEase(Ease.InOutSine);
            mercEffiFill.DOFill(mercenary.EfficiencyPerc, fillDuration).SetEase(Ease.InOutSine);
            mercLevel.text = mercenary.Level.ToString();
            mercRiskLevel.text = mercenary.instabilityRisk.ToString().ToUpper();

            remainingTime.gameObject.SetActive(mercenary.IsOnATask);

            if (count > 1)
                pageText.text = (currentIndex + 1) + "/" + count;
            else
                pageText.text = "";



            if (mercenary.IsOnATask)
            {
                remainingTime.GetComponent<Timer>().SetTimer(mercenary.ContractRemainingTimer);
            }
        }
        private void DisplayContractInfo(Contract contract, int count)
        {

            contTitle.DOKill();
            contDesc.DOKill();

            contTitle.DOText(contract.title, textDuration).SetEase(Ease.InOutSine);
            contDesc.DOText(contract.description, textDuration).SetEase(Ease.InOutSine);
            contRecLevel.text = contract.recomendedLevel.ToString();
            contCash.text = contract.cashReward + "$";
            contExp.text = contract.expReward + " Exp";
            contDifficulty.text = contract.contractDifficultyType.ToString().ToUpper();
            var ts = TimeSpan.FromSeconds(contract.time);
            contTimer.text = string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
            if (count > 1)
                pageText.text = (currentIndex + 1) + "/" + count;
            else
                pageText.text = "";
        }


        public void ShowContractPaper(Contract contract)
        {

            contPaperAcceptBtn.onClick.RemoveAllListeners();
            contPaperCancelBtn.onClick.RemoveAllListeners();
            contPaperAcceptBtn.onClick.AddListener(() => mercenaryTaskSelector.OnFinishAssigning(OnTaskAssigned));
            contPaperCancelBtn.onClick.AddListener(OnCancelTask);

            contPaperTitle.text = contract.title;
            var paperRect = contPaper.GetComponent<RectTransform>();
            paperRect.anchoredPosition = new Vector2(0, -600);
            contPaper.gameObject.SetActive(true);
            paperRect.DOAnchorPosY(60, uiAnimaionDuration).SetEase(uiAnimationCurve).OnComplete(() =>
            {
                // OnFinish?.Invoke();
            });
        }

        private void OnCancelTask()
        {
            mercenaryTaskSelector.OnCancel();
            HideContractPaper();
        }

        private void OnTaskAssigned(ActiveContract currentContract)
        {
            contractsManager.OnAcceptContract(currentContract);
            HideContractPaper();
        }

        public void HideContractPaper(Action OnFinish = null)
        {
            var paperRect = contPaper.GetComponent<RectTransform>();
            paperRect.DOKill();
            paperRect.DOAnchorPosY(-600, uiAnimaionDuration).SetEase(uiAnimationCurve).OnComplete(() =>
            {
                contPaper.gameObject.SetActive(false);
                // OnFinish?.Invoke();
            });
        }
        private void UpdateCurrency()
        {
            currency.text = squadManager.CurrentCash + "$";
        }

        #region Book

        public void ShowTheBook()
        {
            if (bookUI.gameObject.activeInHierarchy)
                HideTheBook();
            else
                ShowTheBook(null);
        }
        public void ShowTheBook(Action OnFinish = null)
        {
            var booRect = bookUI.GetComponent<RectTransform>();
            booRect.DOKill();
            booRect.anchoredPosition = new Vector2(0, -1280);
            bookUI.gameObject.SetActive(true);
            booRect.DOAnchorPosY(0, uiAnimaionDuration).SetEase(uiAnimationCurve).OnComplete(() =>
            {
                OnFinish?.Invoke();
            });
            OnCancelTask();
            HideLogPapers();
            DismissMercenary();
            HideInventory();
            HideHowToPlay();
        }
        public void HideTheBook(Action OnFinish = null)
        {
            var booRect = bookUI.GetComponent<RectTransform>();
            booRect.DOKill();
            booRect.DOAnchorPosY(-1280, uiAnimaionDuration).SetEase(uiAnimationCurve).OnComplete(() =>
            {
                bookUI.gameObject.SetActive(false);
                OnFinish?.Invoke();
            });
        }
        #endregion
        #region LogPaper

        public void ShowLogPapers()
        {
            SetupLogPapers();
            HideHowToPlay();
            DismissMercenary();
            HideTheBook(() =>
            {
                var paperRect = logPaper.GetComponent<RectTransform>();
                paperRect.DOKill();
                paperRect.anchoredPosition = new Vector2(0, -600);
                logPaper.gameObject.SetActive(true);
                paperRect.DOAnchorPosY(60, uiAnimaionDuration).SetEase(uiAnimationCurve).OnComplete(() =>
                {

                });
            });
        }
        public void HideLogPapers()
        {
            var paperRect = logPaper.GetComponent<RectTransform>();
            paperRect.DOKill();
            paperRect.DOAnchorPosY(-600, uiAnimaionDuration).SetEase(uiAnimationCurve).OnComplete(() =>
            {
                logPaper.gameObject.SetActive(false);
                // OnFinish?.Invoke();
            });
        }
        private void SetupLogPapers()
        {
            currentLog = 0;
            logPaperNextBtn.onClick.RemoveAllListeners();
            logPaperPrevBtn.onClick.RemoveAllListeners();
            logPaperOkBtn.onClick.RemoveAllListeners();

            //contractsManager.RemoveFinishedContract(1);


            logPaperOkBtn.onClick.AddListener(() =>
            {
                contractsManager.RemoveFinishedContract(currentLog);
                if (contractsManager.FinishedContractsCount == 0)
                    HideLogPapers();
                else
                    SetupLogPapers();
            });

            logPaperPrevBtn.onClick.AddListener(() =>
            {
                currentLog = (currentLog - 1) < 0 ? contractsManager.FinishedContractsCount - 1 : (currentLog - 1);
                RefrechLogPaper();

            });

            logPaperNextBtn.onClick.AddListener(() =>
            {
                currentLog = (currentLog + 1) % contractsManager.FinishedContractsCount;
                RefrechLogPaper();
            });

            RefrechLogPaper();
        }

        private void RefrechLogPaper()
        {
            var cont = contractsManager.GetFinishedContract(currentLog);
            if (cont == null)
            {
                HideLogPapers();
            }
            else
            {
                logPaperTitle.text = cont.contract.title;
                logDesc.text = cont.log;
            }

            logPaperPrevBtn.gameObject.SetActive(contractsManager.FinishedContractsCount > 1);
            logPaperNextBtn.gameObject.SetActive(contractsManager.FinishedContractsCount > 1);

        }
        #endregion
        #region Inventory

        public void ShowOrHideInventory()
		{
            if (inventoryparent.gameObject.activeInHierarchy)
                HideInventory();
            else
                ShowInventory();
        }
        public void ShowInventory()
        {
            HideTheBook();
            OnCancelTask();
            HideLogPapers();
            var inventoryparentRect = inventoryparent.GetComponent<CanvasGroup>();
            inventoryparentRect.DOKill();
            inventoryparent.gameObject.SetActive(true);
            inventoryparentRect.DOFade(1, fadeDuration).SetEase(Ease.InOutSine);
        }
        public void HideInventory()
		{
            var inventoryparentRect = inventoryparent.GetComponent<CanvasGroup>();
            inventoryparentRect.DOKill();
            inventoryparentRect.DOFade(0, fadeDuration).SetEase(Ease.InOutSine).OnComplete(()
                => inventoryparent.gameObject.SetActive(false));
        }
        public void SetupInventory()
        {
            for (int i = 0; i < squadManager.Items.Count; i++)
            {
                int index = i;
                var go = Instantiate(itemUIPrefab, inventoryContainer);
                go.GetorAddComponent<ItemUI>().Setup(squadManager.Items[i], index);
            }
        }
		#endregion
		#region How To Play

        public void ShowHowToPlay()
		{
            HideTheBook();
            OnCancelTask();
            HideLogPapers();
            var paperRect = htpPaper.GetComponent<RectTransform>();
            paperRect.DOKill();
            paperRect.anchoredPosition = new Vector2(0, -600);
            htpPaper.gameObject.SetActive(true);
            paperRect.DOAnchorPosY(60, uiAnimaionDuration).SetEase(uiAnimationCurve).OnComplete(() =>
            {

            });

        }
        public void HideHowToPlay()
        {
            var paperRect = htpPaper.GetComponent<RectTransform>();
            paperRect.DOKill();
            paperRect.DOAnchorPosY(-600, uiAnimaionDuration).SetEase(uiAnimationCurve).OnComplete(() =>
            {
                htpPaper.gameObject.SetActive(false);
                // OnFinish?.Invoke();
            });
        }
        private void SetupHowToPlay()
		{
            htpPaperNextBtn.onClick.AddListener(() =>
            {
                currentHtp = (currentHtp + 1) % howToPlays.Count;
                howToPlayTitleText.DOKill();
                howToPlayText.DOKill();
                howToPlayTitleText.DOText(howToPlays[currentHtp].title, textDuration);
                howToPlayText.DOText(howToPlays[currentHtp].description, textDuration);

            });
            htpPaperPrevBtn.onClick.AddListener(() =>
            {
                currentHtp = (currentHtp - 1) < 0 ? howToPlays.Count - 1 : (currentHtp - 1);
                howToPlayTitleText.DOKill();
                howToPlayText.DOKill();
                howToPlayTitleText.DOText(howToPlays[currentHtp].title, textDuration);
                howToPlayText.DOText(howToPlays[currentHtp].description, textDuration);
            });
            howToPlayTitleText.DOText(howToPlays[currentHtp].title, textDuration);
            howToPlayText.DOText(howToPlays[currentHtp].description, textDuration);
        }

        #endregion
        #region Tooltip
        public void ShowTooltip(string header, string content,string price, float delay )
        {
            if (string.IsNullOrEmpty(header) && string.IsNullOrEmpty(content)) return;

            if (tooltiptween != null)
                tooltiptween.Complete();
            tooltipRectTransform.DOComplete();

            tooltipRectTransform.gameObject.SetActive(true);
            tooltiptween = tooltipRectTransform.DOScale(1, tooltip_anim_duration).SetDelay(delay).SetEase(tooltip_anim_curve);
            tooltip.Show(header, content, price);
        }
        public void HideTooltip()
        {
            tooltipRectTransform.DOComplete();
            if (tooltiptween != null)
                tooltiptween.Complete();
            tooltipRectTransform.DOScale(0, tooltip_anim_duration).SetEase(tooltip_anim_curve).OnComplete(() =>
            tooltipRectTransform.gameObject.SetActive(false));
        }
		#endregion
		#region Popups
        public void SendPopup(string text)
		{
            var popup = Instantiate(popupPrefab, popupsContainer);
            popup.GetComponentInChildren<TextMeshProUGUI>().text = text;
            popup.GetComponent<CanvasGroup>().DOFade(0, fadeDuration)
                .SetDelay(popupDelay)
                .SetEase(Ease.InOutSine)
                .OnComplete(() =>Destroy(popup.gameObject));

        }
        public static void GlobalPopup(string text)
		{
            GameObject.FindObjectOfType<UIManager>().SendPopup(text);
		}
        public static void NotifiyCurrencyChanged()
        {
            GameObject.FindObjectOfType<UIManager>().UpdateCurrency();
        }


		#endregion
	}
    [System.Serializable]
    public struct HowToPlay
    {
        public string title;
        [TextArea(0,2)]public string description;
    }
}