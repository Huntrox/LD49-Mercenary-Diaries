using HuntroxGames.Utils.Audio;
using JetBrains.Annotations;
using System;

using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HuntroxGames.LD49
{
    public class ContractsManager : MonoBehaviour
    {
        [SerializeField] private List<Contract> contracts = new List<Contract>();
        [SerializeField] private List<ActiveContract> completedContracts = new List<ActiveContract>();

        [SerializeField] private Transform logPile;
        [SerializeField] private GameObject[] logPrefabs;
		[SerializeField] private SoundClip sfx;

		private List<GameObject> logs = new List<GameObject>();
        private SquadManager squadManager;
        private Dictionary<ContractDifficultyType, ActiveContract> activeContracts;
        private HiringManager hiringManager;
        public int FinishedContractsCount { get => completedContracts.Count; }
        public int ContractsCount { get => contracts.Count; }
        private void Start()
        {
            squadManager = GetComponent<SquadManager>();
            hiringManager = GetComponent<HiringManager>();
            contracts.Add(Utils.Utils.GetContract(ContractDifficultyType.Normal, squadManager.AvrageLevel));
            contracts.Add(Utils.Utils.GetContract(ContractDifficultyType.Hard, squadManager.AvrageLevel));
            contracts.Add(Utils.Utils.GetContract(ContractDifficultyType.MissionImpossible, squadManager.AvrageLevel));

            activeContracts = new Dictionary<ContractDifficultyType, ActiveContract>
            {
                {ContractDifficultyType.Normal, null },
                {ContractDifficultyType.Hard, null },
                {ContractDifficultyType.MissionImpossible, null },
            };
        }
        private void Update()
        {
            DOUpdate(activeContracts[ContractDifficultyType.Normal], Time.deltaTime);
            DOUpdate(activeContracts[ContractDifficultyType.Hard], Time.deltaTime);
            DOUpdate(activeContracts[ContractDifficultyType.MissionImpossible], Time.deltaTime);
        }
        void DOUpdate(ActiveContract contract, float deltaTime)
        {
            if (contract != null)
                contract.Update(deltaTime);
        }
        public Contract GetContract(int index)
        {
            if (contracts.IsNullOrEmpty() && index >= contracts.Count)
                return null;
            return contracts[index];
        }
        public ActiveContract GetFinishedContract(int index)
        {
            if (completedContracts.IsNullOrEmpty() && index >= completedContracts.Count)
                return null;
            return completedContracts[index];
        }
        public ActiveContract TryAcceptContract(int index)
        {
            if (squadManager.AvailableMercenaries.Count < 1)
                return null;
            var cont = contracts[index];
            var activeCont = new ActiveContract
            {
                contract = cont,
                remainingTime = cont.time,
                OnContractFinished = OnContractFinished
            };
            return activeCont;
        }

        public void OnAcceptContract(ActiveContract contract)
        {
            contract.CaculateEffeciency();
            activeContracts[contract.contract.contractDifficultyType] = contract;
            contracts.Remove(contract.contract);
        }

        public void OnContractFinished(ActiveContract contract)
        {

            //TODO STUFF
            Utils.Utils.BattleSim(contract.mercenaries, contract.contract);
            string battleResualtLog = GetBattleLog(contract);
            contract.log = battleResualtLog;
            //ContractDifficulty calculateDifMod =Utils.Utils.CalcualteDifficulty(contract)

            contract.mercenaries.ForEach(m =>
            {
                if (m.IsDead)
                    squadManager.RemoveFromSquad(m);
                else
                    m.level.AddExp(contract.contract.expReward);
            });
            squadManager.AddCash(contract.contract.cashReward);
            completedContracts.Add(contract);
            contracts.Add(Utils.Utils.GetContract(contract.contract.contractDifficultyType, squadManager.AvrageLevel));
            UIManager.GlobalPopup("New Mission has been added");
            AddNewLogToLogPile();
            hiringManager.AddaMercenary(squadManager.AvrageLevel);
            UIManager.GlobalPopup("New mercenary available to hire");
            UIManager.NotifiyCurrencyChanged();
            AudioController.PlaySoundClip(sfx);
            activeContracts[contract.contract.contractDifficultyType] = null;
        }

        public void AddNewLogToLogPile()
        {
            logPile.gameObject.SetActive(true);

            Vector2 offset = new Vector2(UnityEngine.Random.Range(-10, 10), 10 * completedContracts.Count);
            var go = Instantiate(logPrefabs.RandomElement(), logPile);
            go.GetComponent<RectTransform>().anchoredPosition = offset;
            logs.Add(go);
        }
        private string GetBattleLog(ActiveContract contract)
        {
            string actions = "<b>Log:</b>";
            foreach (var mercs in contract.mercenaries)
            {
                if (!mercs.IsDead)
                {
                    var mercAction = mercs.CheckMS(contract.mercenaries);
                    switch (mercAction)
                    {
                        case InstabilityActions.None:
                            break;
                        case InstabilityActions.Suicid:
                            mercs.currentHealth = 0;
                            actions += "\n-" + mercs.mercenaryName + " committed suicide";
                            break;
                        case InstabilityActions.Kill:
                            var alive = contract.mercenaries.Where(m => !m.IsDead && m != mercs).ToList();
                            if (!alive.IsNullOrEmpty())
                            {
                                var randMer = alive.RandomElement();
                                actions += "\n-" + randMer.mercenaryName + " was killed by " + mercs.mercenaryName;
                                randMer.currentHealth = 0;
                            }
                            break;
                        case InstabilityActions.Attack:
                            alive = contract.mercenaries.Where(m => !m.IsDead && m != mercs).ToList();
                            if (!alive.IsNullOrEmpty())
                            {
                                var randMer = alive.RandomElement();
                                actions += "\n-" + mercs.mercenaryName + " was Attacked by " + randMer.mercenaryName;
                                randMer.CurrentHealthPerc =
                                    randMer.currentHealth - GlobalSettings.AttackModifier(mercs.instabilityRisk);
                            }
                            break;
                        case InstabilityActions.RunAway:
                            actions += "\n-" + mercs.mercenaryName + " Stole the money and Ran Away";
                            contract.contract.cashReward = 0;
                            squadManager.RemoveFromSquad(mercs);
                            break;
                        default:
                            break;
                    }
                }
			}

            string deathLog = " <b>Death:</b>";
            var deaths = contract.mercenaries.Where(m => m.IsDead).ToList();
            if (!deaths.IsNullOrEmpty())
            {
                foreach (var mercs in deaths)
                {
                    deathLog += "\n-"+mercs.mercenaryName + ".";
                }
			}
			else
			{
                deathLog = "<b>Death:</b> \nNone";
            }
            var final = string.Format("{0}\n{1}\nEarned: <b>{2}$</b>", deathLog,actions,contract.contract.cashReward);
            return final;
        }

        public void RemoveFinishedContract(int index)
        {
            if (completedContracts.Count >= 1)
            {
                var go = logs.Last();
                logs.Remove(go);
                Destroy(go);
            }
            if (!completedContracts.IsNullOrEmpty())
                completedContracts.RemoveAt(index);
            logPile.gameObject.SetActive(logs.Count <= 1);
        }
    }
    public class ActiveContract
    {
        public Contract contract;
        public List<Mercenary> mercenaries;
        public float remainingTime;
        public Action<ActiveContract> OnContractFinished;
        public string log;
        public void Update(float deltaTime)
        {
            remainingTime -= deltaTime;
            if (remainingTime <= 0)
            {
                mercenaries.ForEach(m => m.IsOnATask = false);
                remainingTime = 0;
                OnContractFinished(this);
                return;
            }
            foreach (var merc in mercenaries)
            {
                merc.ContractRemainingTimer = remainingTime;
                merc.IsOnATask = true;
            }
        }

		public void CaculateEffeciency()
		{
            var effeSum = mercenaries.Select(x => x.efficiency).Sum();
            var offPers = (remainingTime * effeSum) / 100;
            remainingTime -= offPers;
        }
	}
}
