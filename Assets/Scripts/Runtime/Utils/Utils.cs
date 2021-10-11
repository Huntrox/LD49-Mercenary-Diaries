using HuntroxGames.LD49;
using System.Collections.Generic;
using UnityEngine;

namespace HuntroxGames.Utils
{
    public static class Utils
    {
        public static Texture2D TextureFromSprite(this Sprite sprite)
        {
            if (sprite == null || sprite.texture == null) return null;
            if (sprite.rect.width != sprite.texture.width)
            {
                try
                {
                    if (sprite.rect.width != sprite.texture.width)
                    {
                        Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
                        newText.filterMode = sprite.texture.filterMode;
                        Color[] colors = newText.GetPixels();
                        Color[] newColors = sprite.texture.GetPixels((int)Mathf.CeilToInt(sprite.rect.x),
                                                                     (int)Mathf.CeilToInt(sprite.rect.y),
                                                                     (int)Mathf.CeilToInt(sprite.rect.width),
                                                                     (int)Mathf.CeilToInt(sprite.rect.height));
                        newText.SetPixels(newColors);
                        newText.Apply();
                        return newText;
                    }
                }
                catch
                {
                    return sprite.texture;
                }
            }
            return sprite.texture;
        }

		internal static Mercenary CreateNewMercenary(InstabilityRiskActions instbRisk, int recLevel)
		{
           
            recLevel = Mathf.Clamp(Random.Range(recLevel-2, recLevel+2), 1, int.MaxValue);
            int riskModif = (int)instbRisk * 25;
            int cashPerLevl = 220 * recLevel;
            int rang = (cashPerLevl * 25) / 100;
            int riskDiscount = (cashPerLevl * riskModif) / 100;
            int cashAfterDsicount = cashPerLevl - riskDiscount;
            int final = Random.Range(cashAfterDsicount - rang, cashAfterDsicount + rang);

            float health =5 + recLevel * GlobalSettings.HEALTH_PER_LEVEL_MODIFIER;
            float ms =(2 + (int)instbRisk)+ recLevel * GlobalSettings.MS_PER_LEVEL_MODIFIER;
            float effeciency = 1 + (int)instbRisk + (recLevel * GlobalSettings.EFECIENCY_PER_LEVEL_MODIFIER);

            var merc = new Mercenary
            {
                MaxHealth = health,
                currentHealth = health,
                mentalStability = ms,
                currMentalStability = ms,
                efficiency = effeciency,
                mercenaryName = GetRandomMercenaryName(),
                mercenaryDesc = GetRandomMercenaryDescription(),
                portrait =  GetRandomMercenaryPortrait(),
                value = final,

            };
            merc.level = new EntityLevel(recLevel, merc.OnLevelUp);
            return merc;
        }

        public static Sprite GetRandomMercenaryPortrait()
		{
            //throw new System.NotImplementedException();
            return Resources.LoadAll<Sprite>("Portraits").RandomElement();
		}

        public static string GetRandomMercenaryName()
		{
            var name = Resources.Load<ItemsDatabase>("Database").mercenaryNames.RandomElement();
            return name;
        }

        public static string GetRandomMercenaryDescription()
		{
            //throw new System.NotImplementedException();
            return "";
		}

        public static string GetMissionDescription()
		{
            var database = Resources.Load<ItemsDatabase>("Database");
            var name = database.names.RandomElement();
            var building = database.buildings.RandomElement();
            var location = database.locations.RandomElement();
            var preset = database.presets.RandomElement();

            preset = preset.Replace("#name#", name);
            preset = preset.Replace("#building#", building);
            preset = preset.Replace("#location#", location);
            return preset;
        }
		public static Contract GetContract(ContractDifficultyType difficultyType, int avrageLevel)
        {
            var temp_Contract = new Contract
            {
                contractDifficultyType = difficultyType,
                cashReward = Random.Range(200, 800),
                expReward = Random.Range(200, 500),
                recomendedLevel = Random.Range(1, 10),
                time = Random.Range(10, 20)
                //time = Random.Range(60, 120)
            };
            switch (difficultyType)
            {
                case ContractDifficultyType.Normal:
                    return new Contract
                    {
                        contractDifficultyType = difficultyType,
                        cashReward = Random.Range(150, 300),
                        expReward = Random.Range(120, 200),
                        recomendedLevel = GetDecomendedLevelBasedOnAvrageLev(avrageLevel, difficultyType),
                        description = GetMissionDescription(),
                        time = 30
                    };
                case ContractDifficultyType.Hard:
                    return new Contract
                    {
                        contractDifficultyType = difficultyType,
                        cashReward = Random.Range(350, 450),
                        expReward = Random.Range(240, 320),
                        recomendedLevel = GetDecomendedLevelBasedOnAvrageLev(avrageLevel, difficultyType),
                        description = GetMissionDescription(),
                        time = 60
                    };
                case ContractDifficultyType.MissionImpossible:
                    return new Contract
                    {
                        contractDifficultyType = difficultyType,
                        cashReward = Random.Range(500, 600),
                        expReward = Random.Range(340, 420),
                        recomendedLevel = GetDecomendedLevelBasedOnAvrageLev(avrageLevel, difficultyType),
                        description = GetMissionDescription(),
                        time = 90
                    };
                default:
                    return temp_Contract;
            }
            //TODO return a contract from the database based on the Difficulty;
        }

        private static int GetDecomendedLevelBasedOnAvrageLev(int avrageLevel, ContractDifficultyType difficultyType)
        {
            int recmmLevl = 1;
			switch (difficultyType)
			{
				case ContractDifficultyType.Normal:
                    recmmLevl = Random.Range((avrageLevel - 3), (recmmLevl + 2));
                    break;
				case ContractDifficultyType.Hard:
                    recmmLevl = Random.Range((avrageLevel - 2), (recmmLevl + 3));
                    break;
				case ContractDifficultyType.MissionImpossible:
                    recmmLevl = Random.Range((avrageLevel - 1), (recmmLevl + 5));
                    break;
				default:
					break;
			}

            return Mathf.Clamp(recmmLevl,1,int.MaxValue);

        }
        public static bool IsNotOverGameObject()
        {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                return false;
            }
            return true;
        }

		public static void BattleSim(List<Mercenary> mercenaries, Contract contract)
		{
            var RecommLev = contract.recomendedLevel;
            var diffic = contract.contractDifficultyType;

            foreach (var mers in mercenaries)
			{
                var diffModifire = GlobalSettings.GetDiffModifier(diffic);
                var health_modif = Mathf.Clamp((RecommLev - mers.Level), 0, int.MaxValue) * GlobalSettings.HEALTH_LOSE_PER_LEVEL_MODIFIER + diffModifire;
                var ms_modif = Mathf.Clamp((RecommLev - mers.Level), 0, int.MaxValue) * GlobalSettings.MS_LOSE_PER_LEVEL_MODIFIER + diffModifire;
                var contractDifficulty = new ContractDifficulty
                {
                    healthModifier =health_modif,
                    mentalStabilityModifier = ms_modif,
                };
                mers.OnCompletingAcontract(contractDifficulty);
            }
		}



		public static bool IsValid(string input, string pattern)
        {
            if (string.IsNullOrEmpty(input))
                return false;
            return System.Text.RegularExpressions.Regex.IsMatch(input, pattern);
        }
        public static int ToInt(this string str, int defaultValue = default(int))
        {
            if (string.IsNullOrEmpty(str))
                return defaultValue;
            if (!int.TryParse(str, out int s))
                return s;
            else
                return defaultValue;
        }
        public static float ToFloat(this string str, float defaultValue = default(float))
        {
            if (string.IsNullOrEmpty(str))
                return defaultValue;
            if (!float.TryParse(str, out float s))
                return s;
            else
                return defaultValue;
        }



        public static Vector2 WorldToCanvas(Vector3 worldPostion)=>WorldToCanvas(worldPostion,null,Camera.main);
        public static Vector2 WorldToCanvas(Vector3 worldPostion,RectTransform canvas)=> WorldToCanvas(worldPostion,canvas,Camera.main);
        public static Vector2 WorldToCanvas(Vector3 worldPostion, RectTransform canvas, Camera camera)
        {
            if (canvas == null)
                canvas = GameObject.Find("Canvas").GetComponent<RectTransform>();
            if (canvas == null)
                Debug.LogError("No Canvas was  found.");
            if (camera == null)
                camera = Camera.main;

            Vector2 offset = new Vector2((float)canvas.sizeDelta.x / 2f, (float)canvas.sizeDelta.y / 2f);
            Vector2 viewportPosition = camera.WorldToViewportPoint(worldPostion);
            Vector2 screenPosition = new Vector2(
                (viewportPosition.x * canvas.sizeDelta.x),
                (viewportPosition.y * canvas.sizeDelta.y)
                );
            return screenPosition - offset;
        }


        public static Vector2 GetRandomWolrdPositionOnScreen() => GetRandomWolrdPositionOnScreen(Vector2.zero, Camera.main);
        public static Vector2 GetRandomWolrdPositionOnScreen(Camera cam) => GetRandomWolrdPositionOnScreen(Vector2.zero, cam);
        public static Vector2 GetRandomWolrdPositionOnScreen(Vector2 offset) => GetRandomWolrdPositionOnScreen(offset, Camera.main);
        public static Vector2 GetRandomWolrdPositionOnScreen(Vector2 offset, Camera cam) => new Vector2(
                Random.Range(cam.ScreenToWorldPoint(new Vector2(0, 0)).x + offset.x,
                 cam.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x - offset.x),
                Random.Range(cam.ScreenToWorldPoint(new Vector2(0, 0)).y + offset.y,
                 cam.ScreenToWorldPoint(new Vector2(0, Screen.height)).y - offset.y)
            );

        public static Vector3 RandomDirection()
             => new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), 0).normalized;
        public static Vector3 RandomDirection3DForward() =>
            new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, UnityEngine.Random.Range(-1f, 1f)).normalized;

        public static Vector3 RandomDirection(float minDis = 0.4f)
        {
            Vector3 direction = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), 0).normalized;
            direction *= UnityEngine.Random.Range(minDis, 1);
            return direction;
        }

        public static Vector2 GetVectorFromAngle(int angle)
        {
            float angleRad = angle * (Mathf.PI / 180f);
            return new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        }

        public static float GetAngleFromVector3(Vector3 dir)
        {
            dir = dir.normalized;
            float n = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
            if (n < 0) n += 360;
            return n;
        }
        public static float GetAngleFromVector(Vector2 dir)
        {
            dir = dir.normalized;
            float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (n < 0) n += 360;
            return n;
        }
        public static float GetAngle(Vector3 sender, Vector3 target)
        {
            Vector2 diff = (target - sender).normalized;
            return -Mathf.Rad2Deg * Mathf.Atan2(diff.x, diff.y);
        }
        public static Vector2 V3ToV2(Vector3 vec3) => new Vector2(vec3.x, vec3.z);
        public static Vector3 V2ToV3(Vector2 vec2, float y = 1f) => new Vector3(vec2.x, 0, vec2.y);
        public static void SetAudioGropVolume(UnityEngine.Audio.AudioMixer target, float value, string volumeParameter)
        {
            float volume = Mathf.Clamp01(value);
            if (volume > 0f)
                volume = 45 * Mathf.Log10(volume);
            else
                volume = -144f;
            target.SetFloat(volumeParameter, volume);
        }
    }
}