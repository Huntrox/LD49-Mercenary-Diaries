using UnityEngine;
using UnityEditor;
using HuntroxGames.Utils.Audio;
using System;
using System.Collections.Generic;
using UnityEngine.Audio;

namespace HuntroxGames.Utils.EditorUtils
{
    public class AudioManagerEditorWindow : EditorWindow
    {

        [MenuItem("HuntroxUtils/AudioManager Editor Window", false, 0)]
        public static void ShowWindow()
        {
            GetWindow<AudioManagerEditorWindow>("AudioManager Window");
            EditorWindow window = GetWindow<AudioManagerEditorWindow>(true, "AudioManager Window", true);
            //window.maxSize = new Vector2(638, 430);
            //window.minSize = window.maxSize;
            window.minSize = new Vector2(638, 430);
            ((EditorWindow)window).ShowUtility();
            Init();

        }



        public static Texture playicon;
        public static Texture stopicon;

        public static AudioDatabase soundClipsList;
        public static AudioMixer audioMixer;
        public static string audioMixerPath;
        public static string soundClipsListPath;

        private string searchInput;
        public static int headerTaps = 0;

        public int audioMixerGroupsNamesTaps = 0;

        public string[] tapsName = new string[] { "Setup","SFX","BGM"};
        public string[] audioMixerGroupsNames = new string[] { };


        // Sound Clip fields
        public string clipName = string.Empty;
        public AudioClip clip;
        public float delay = 0f;
        public float pitch = 1f;
        public float volume = 1f;
        public bool loop = false;
        public AudioMixerGroup group;



        public bool RandomPitch = false;
        public bool RandomVolume = false;
        public floatMinMax minMaxPitch = new floatMinMax(0.5f, 1);
        public floatMinMax minMaxVolume = new floatMinMax(0.5f, 1);
        /// /////
        bool showAudioMixer;
        bool showSoundClipsListData;
        bool showSettings;
        Vector2 scrollPosition;

        GUIContent playbtn_content = new GUIContent(playicon);
        GUIContent stopbtn_content = new GUIContent(stopicon);

        public static void Init()
        {
            soundClipsList = AudioDataHandler.LoadOrCreateSoundsDataAsset();
            audioMixer = AudioDataHandler.LoadAudioMixer();
            currentBGM = soundClipsList.SoundTracks();

            string playPath = EditorGUIUtility.isProSkin ? "Assets/AudioManager/Resources/texture/play_btn_dark.png" :
                "Assets/AudioManager/Resources/texture/play_btn.png";
            string stopPath = EditorGUIUtility.isProSkin ? "Assets/AudioManager/Resources/texture/stop_btn_dark.png" :
                "Assets/AudioManager/Resources/texture/stop_btn.png";
            playicon = (Texture)AssetDatabase.LoadAssetAtPath(playPath, typeof(Texture));
            stopicon = (Texture)AssetDatabase.LoadAssetAtPath(stopPath, typeof(Texture));

            if (audioMixer == null || soundClipsList == null)
                headerTaps = 0;
            else headerTaps= 1;


        }


        private void OnFocus()
        {

            soundClipsList = AudioDataHandler.LoadOrCreateSoundsDataAsset();
            audioMixer = AudioDataHandler.LoadAudioMixer();
            currentBGM = soundClipsList.SoundTracks();
            string playPath= EditorGUIUtility.isProSkin?"Assets/AudioManager/Resources/texture/play_btn_dark.png":
                "Assets/AudioManager/Resources/texture/play_btn.png";
            string stopPath= EditorGUIUtility.isProSkin ? "Assets/AudioManager/Resources/texture/stop_btn_dark.png" :
                "Assets/AudioManager/Resources/texture/stop_btn.png";
            playicon = (Texture)AssetDatabase.LoadAssetAtPath(playPath, typeof(Texture));
            stopicon = (Texture)AssetDatabase.LoadAssetAtPath(stopPath, typeof(Texture));

        }
        void OnGUI()
        {




            EditorGUILayout.BeginVertical();
            Header();
            GUILayout.Space(5);
			switch (headerTaps)
			{
                case 0:
                    SetupMenu();
                    break;
                case 1:
                    GUILayout.BeginHorizontal();
                    SCLDisplayBox();
                  //  GUILayout.Space(5);
                    GUILayout.FlexibleSpace();
                    SCEBox();

                    GUILayout.EndHorizontal();
                    break;
                case 2:
                    BGM_Window();
                    break;
			}

 
            EditorGUILayout.EndVertical();



        }
        void SetupMenu()
        {
            showAudioMixer = EditorGUILayout.Foldout(showAudioMixer, "Audio Mixer");
            if (showAudioMixer)
            {
            EditorGUILayout.BeginVertical("Box");
               audioMixer = (AudioMixer)EditorGUILayout.ObjectField("Audio Mixer", audioMixer, typeof(AudioMixer), false
                   , GUILayout.Width(position.width - 18));

            if(audioMixer != null)
                {
                    audioMixerPath = AssetDatabase.GetAssetPath(audioMixer);
                    EditorPrefs.SetString("audioMixerPath", audioMixerPath);
                    //if (soundClipsList != null&& audioMixer != null)
                    //    soundClipsList.audioMixer = audioMixer;
                }

                EditorGUILayout.EndVertical();

            }
            showSoundClipsListData = EditorGUILayout.Foldout(showSoundClipsListData, "Sound Clips Data");
       
            if (showSoundClipsListData)
            {
            EditorGUILayout.BeginVertical("Box");
                soundClipsList = (AudioDatabase)EditorGUILayout.ObjectField("ScriptableObject", soundClipsList,
                    typeof(AudioDatabase), false
                   , GUILayout.Width(position.width - 18));

            if(soundClipsList != null)
                {
                    soundClipsListPath = AssetDatabase.GetAssetPath(soundClipsList);
                    EditorPrefs.SetString("soundClipsListPath", soundClipsListPath);
                }

                //if (GUILayout.Button("Create Sound Clips ScriptableObject "))
                //{

                //    string mainpath = Application.dataPath;
                //    string path = EditorUtility.OpenFolderPanel("Path", mainpath, "");
                //    if (!string.IsNullOrEmpty(path))
                //    {
                ////pathhh
                //if (absolutepath.StartsWith(Application.dataPath))
                //{
                //    relativepath = "Assets" + absolutepath.Substring(Application.dataPath.Length);
                //}

                //        Debug.Log(path);
                //        SoundClipsList list = SoundClipsList.CreateInstance<SoundClipsList>();
                //        AssetDatabase.CreateAsset(list, path+"/SoundClipsData.asset");
                //        //EditorPrefs.SetString("soundClipsListPath", "Assets/AudioManager/Resources/SoundClipsData.asset");
                //        AssetDatabase.SaveAssets();

                //    }
                //}




                EditorGUILayout.EndVertical();

            }

            showSettings = EditorGUILayout.Foldout(showSettings, "Settings");

            if (showSettings)
            {
                EditorGUILayout.BeginVertical("Box", GUILayout.Width(position.width-15));

				soundClipsList.maxSoundPlayCountPerFrame = EditorGUILayout.IntField("Max Simultaneous Audio", soundClipsList.maxSoundPlayCountPerFrame);
				soundClipsList.maxSameSoundPlayCountPerFrame = EditorGUILayout.IntField("Max Simultaneous(same audioclip) ", soundClipsList.maxSameSoundPlayCountPerFrame);

				EditorGUILayout.EndVertical();
            }


        }

        void Header()
        {
            EditorGUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.Space(5);
            GUILayout.FlexibleSpace();
            EditorGUI.indentLevel++;
            headerTaps = GUILayout.Toolbar(headerTaps, tapsName, GUILayout.Width(position.width - 5));
            EditorGUI.indentLevel--;
            GUILayout.EndHorizontal();
        }

        void SCLDisplayBox()
        {
            EditorGUILayout.BeginVertical(GUILayout.Height(position.height - 42));
            GUIStyle style = new GUIStyle();
          
           // style.fixedHeight = position.height - 84;
            style.fixedWidth = 152;
            EditorGUILayout.BeginVertical(style);
            GUILayout.Label("Sound clips List", EditorStyles.boldLabel);
            GUILayout.Space(5);
            searchInput = GUILayout.TextArea(searchInput);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(position.width - 138));
            //GUILayout.Width(position.width - 84);
 

            if (soundClipsList.SoundList.Count != 0)
            {
                for (int i = 0; i < soundClipsList.SoundList.Count; i++)
                {

                    if (string.IsNullOrEmpty(searchInput) || !string.IsNullOrEmpty(searchInput) && soundClipsList.SoundList[i].clipname.ToLower().Contains(searchInput.ToLower()))
                    {

                        if (GUILayout.Button(soundClipsList.SoundList[i].clipname, GUILayout.Width(138),
                            GUILayout.Height(25)))
                        {
                            clipName = soundClipsList.SoundList[i].clipname;
                            clip = soundClipsList.SoundList[i].clip;
                            delay = soundClipsList.SoundList[i].delay;
                            volume = soundClipsList.SoundList[i].volume;
                            pitch = soundClipsList.SoundList[i].pitch;
                            loop = soundClipsList.SoundList[i].loop;
                            RandomPitch = soundClipsList.SoundList[i].RandomPitch;
                            RandomVolume = soundClipsList.SoundList[i].RandomVolume;
                            minMaxPitch = soundClipsList.SoundList[i].minMaxPitch;
                            minMaxVolume = soundClipsList.SoundList[i].minMaxVolume;


                            for (int t = 0; t < audioMixerGroupsNames.Length; t++)
                            {
                                if (audioMixerGroupsNames[t] == soundClipsList.SoundList[i].group.name)
                                    audioMixerGroupsNamesTaps = t;
                            }
                            GUI.FocusControl(null);
                        }
                        GUILayout.Space(2);
                    }
                }


            }
            GUILayout.FlexibleSpace();
            GUILayout.EndScrollView();
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            EditorGUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Clear All"))
            {
                if (EditorUtility.DisplayDialog("Warrning",
                    " this will clear all the sound clips data , Are you sure???", "Yes !","No"))
                {
                    updateSoundList();
                    soundClipsList.SoundList.Clear();
                    updateSoundList();
                    GUI.FocusControl(null);
                }


            }
            EditorGUILayout.EndVertical();
        }




        void SCEBox()
        {

            EditorGUILayout.BeginVertical("Box", GUILayout.Height(position.height - 42));
          
            GUILayout.Label("Sound clips Edit Menu", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            GUILayout.Space(15);
          
            clipName = EditorGUILayout.TextField("Sound Name", clipName, GUILayout.Width(position.width - 170));
            GUILayout.Space(10);
            clip = (AudioClip)EditorGUILayout.ObjectField("Audio Clip",clip, typeof(AudioClip),false,GUILayout.Width(position.width - 170));

            GUILayout.BeginHorizontal();
            playbtn_content = new GUIContent(playicon);
            stopbtn_content = new GUIContent(stopicon);
            loop = EditorGUILayout.Toggle("Loop", loop);
            //GUILayout.Space(220);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(playbtn_content,GUILayout.Width(20), GUILayout.Height(20)))
            {
                if (clip != null)
                {
                    SoundClip t_clip = new SoundClip();
                    t_clip.clipname = clipName;
                    t_clip.clip = clip;
                    t_clip.volume = volume;
                    t_clip.pitch = pitch;
                    t_clip.delay = delay;
                    t_clip.minMaxPitch = minMaxPitch;
                    t_clip.minMaxVolume = minMaxVolume;
                    t_clip.RandomPitch = RandomPitch;
                    t_clip.RandomVolume = RandomVolume;
                    t_clip.loop = loop;
                    AudioController.PlaySoundEditMode(t_clip);
                    GUI.FocusControl(null);
                }
                //     soundClipName

            }
            if (GUILayout.Button(stopbtn_content, GUILayout.Width(20), GUILayout.Height(20)))
            {
                AudioDataHandler.StopAllClips();
                AudioController.StopAudioSource();
            }
            GUILayout.EndHorizontal();
            EditorGUI.indentLevel--;
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            EditorGUILayout.LabelField("AudioGroup: ");
            EditorGUI.indentLevel++;
            if (audioMixer != null)
            {
                GUILayout.Space(10);
                audioMixerGroupsNames = new string[audioMixer.FindMatchingGroups("Master").Length];

                for (int i = 0; i < audioMixer.FindMatchingGroups("Master").Length; i++)
                {
                    audioMixerGroupsNames[i] = audioMixer.FindMatchingGroups("Master")[i].name;

                }
                audioMixerGroupsNamesTaps = GUILayout.Toolbar(audioMixerGroupsNamesTaps, audioMixerGroupsNames);
                group = audioMixer.FindMatchingGroups("Master")[audioMixerGroupsNamesTaps];

            }
            else
            {
                EditorGUILayout.HelpBox("There is no Audio Mixer Selected Please Setup your \n  Audio Mixer on Setup Tap Menu !!", MessageType.Warning, true);
 
            }
            GUILayout.Space(5);
            EditorGUILayout.Separator();
            EditorGUI.indentLevel--;
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            EditorGUI.indentLevel++;
            delay = EditorGUILayout.Slider("Delay",delay,0,5f, GUILayout.Width(position.width - 170));
            GUILayout.Space(5);
            if (RandomPitch)
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.MinMaxSlider("Pitch", ref minMaxPitch._MinValue, ref minMaxPitch._MaxValue, 0, 3, GUILayout.Width(position.width - 275));
                minMaxPitch.MinValue = EditorGUILayout.FloatField(minMaxPitch.MinValue, GUILayout.Width(48));
                minMaxPitch._MaxValue = EditorGUILayout.FloatField(minMaxPitch.MaxValue, GUILayout.Width(48));
                GUILayout.EndHorizontal();
            }
            else
            pitch = EditorGUILayout.Slider("Pitch", pitch,0f,3f,GUILayout.Width(position.width - 170));

            GUILayout.Space(5);
            if (RandomVolume)
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.MinMaxSlider("Volume", ref minMaxVolume._MinValue, ref minMaxVolume._MaxValue, 0, 1, GUILayout.Width(position.width - 275));
                minMaxVolume.MinValue = EditorGUILayout.FloatField(minMaxVolume.MinValue, GUILayout.Width(48));
                minMaxVolume.MaxValue = EditorGUILayout.FloatField(minMaxVolume.MaxValue, GUILayout.Width(48));
                GUILayout.EndHorizontal();
            }
            else
                volume = EditorGUILayout.Slider("Volume", volume, 0, 1f, GUILayout.Width(position.width - 170));

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.EndHorizontal();

            RandomPitch = EditorGUILayout.Toggle("Randomized  Pitch", RandomPitch);
            RandomVolume = EditorGUILayout.Toggle("Randomized  Volume", RandomVolume);
            GUILayout.Space(5);
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            //GUILayout.Space(257);
            GUILayout.FlexibleSpace();
            string add_btn = (!soundClipsList.CheckItemInList(clipName) ? "Add" : "Save");
            if (GUILayout.Button(add_btn))
            {
                if(!string.IsNullOrEmpty(clipName)&& clip != null)
                {
                    SoundClip t_clip = new SoundClip();
                    t_clip.clipname = clipName;
                    t_clip.clip = clip;
                    t_clip.pitch = pitch;
                    t_clip.volume = volume;
                    t_clip.loop = loop;
                    t_clip.delay = delay;
                    t_clip.group = group;
                    t_clip.minMaxVolume = minMaxVolume;
                    t_clip.minMaxPitch = minMaxPitch;
                    t_clip.RandomPitch = RandomPitch;
                    t_clip.RandomVolume = RandomVolume;


                    if (!soundClipsList.CheckItemInList(t_clip.clipname))
                    {
                        soundClipsList.UpdateSoundClip(t_clip);
                        updateSoundList();
                    }
                    else
                    {
                        if (EditorUtility.DisplayDialog("Warrning", clipName + " Already Exist Do you want to overwrite it ??", "Yes", "No"))
                        {
                            soundClipsList.UpdateSoundClip(t_clip);
                            updateSoundList();
                            return;
                        }
                    }

                }
                GUI.FocusControl(null);
            }
            if (GUILayout.Button("Remove"))
            {
                if (string.IsNullOrEmpty(clipName) && clip == null) return;
                if (!soundClipsList.CheckItemInList(clipName)) return;
                updateSoundList();

                if (EditorUtility.DisplayDialog("Delete "+ clipName,$" [{clipName}] This will be remove from  soundclips list data , are you sure?", "Delete","Dont"))
                {

                  
                    soundClipsList.SoundList.Remove(soundClipsList.GetSoundClip(clipName));
                    updateSoundList();

                    GUI.FocusControl(null);
                }
            }
            EditorGUI.indentLevel--;

            GUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

        }

        public int currentSceneIndex = 0;
        public int currentBgmIndex = 0;
        public int bgmToolbarIndex = 0;
        public int audioClipsCount = 0;
        public static string[] currentBGM = new string[] { };
        public static string[] bgm_windows = new string[] {"Create","Edit"};
        public SoundTrack mt = new SoundTrack();
        public List<AudioClip> audioClips = new List<AudioClip>();
        public List<AudioClip> audioClipsEdit = new List<AudioClip>();
        public int audioClipsEditCount = 0;
        public Vector2 AudioClipsScroll;
        public Vector2 AudioClipsScrollEdit;
 
        void BGM_Window()
        {
            GUILayout.Space(6);
            EditorGUILayout.BeginVertical("Box");
            bgmToolbarIndex = GUILayout.Toolbar(bgmToolbarIndex, bgm_windows, GUILayout.Width(position.width - 18));
            EditorGUILayout.EndVertical();

            if (bgmToolbarIndex == 0)
                BGM_CreateWindow();
            else
                BGM_EditWindow();
            GUILayout.Space(6);
        }


        void BGM_EditWindow()
        {
            GUILayout.Space(6);
            EditorGUILayout.BeginVertical("Box", GUILayout.Width(position.width-18));

            GUIStyle sltc_langStyl = new GUIStyle(EditorStyles.popup);
            sltc_langStyl.fontStyle = FontStyle.Bold;
            sltc_langStyl.alignment = TextAnchor.MiddleCenter;
            if (currentBGM.IsNullOrEmpty())
            {
                EditorGUILayout.HelpBox("There is no Music tracks", MessageType.Info,true);
                EditorGUILayout.EndVertical();
                return; 
            }
            currentBgmIndex = EditorGUILayout.Popup("Selected Level", currentBgmIndex, currentBGM, sltc_langStyl, GUILayout.Width(position.width - 18));

            GUILayout.Space(6);
            soundClipsList.MusicTracksList[currentBgmIndex].group = (AudioMixerGroup)EditorGUILayout.ObjectField("Audio Group", soundClipsList.MusicTracksList[currentBgmIndex].group, typeof(AudioMixerGroup), false, GUILayout.Width(position.width - 20));

            audioClipsEditCount = ((audioClipsEdit == null) ? 0 : soundClipsList.MusicTracksList[currentBgmIndex].tracks.Length);

            EditorGUILayout.LabelField("Music tracks", AudioManagerUtils.LabelStyle(12, true));
            AudioClipsScrollEdit = GUILayout.BeginScrollView(AudioClipsScrollEdit);
           
            for (int i = 0; i < audioClipsEditCount; i++)
            {
                GUILayout.BeginHorizontal();
               
                soundClipsList.MusicTracksList[currentBgmIndex].tracks[i] = (AudioClip)EditorGUILayout.ObjectField("AudioClip: " + (i + 1), soundClipsList.MusicTracksList[currentBgmIndex].tracks[i], typeof(AudioClip), false, GUILayout.Width(position.width - 50));
                if (GUILayout.Button("X", GUILayout.Width(20), GUILayout.Height(20)))
                {
                    if (soundClipsList.MusicTracksList[currentBgmIndex].tracks[i] == null)
                    {
                       AudioManagerUtils.RemoveAt(ref soundClipsList.MusicTracksList[currentBgmIndex].tracks, i);
                        updateBGM();
                        GUI.FocusControl(null);
                    }
                    else
                    {
                        if (EditorUtility.DisplayDialog("Delete " + soundClipsList.MusicTracksList[currentBgmIndex].tracks[i].name, $" [{   soundClipsList.MusicTracksList[currentBgmIndex].tracks[i].name}] This will be remove from  Music Tracks list, are you sure?", "Do it", "Dont"))
                        {
                            AudioManagerUtils.RemoveAt(ref soundClipsList.MusicTracksList[currentBgmIndex].tracks, i);
                            updateBGM();
                            GUI.FocusControl(null);
                        }
                    }
                }
                GUILayout.EndHorizontal();
            }
            if (GUILayout.Button("Add Audio Clip"))
            {
                audioClipsEditCount++;
                Array.Resize(ref soundClipsList.MusicTracksList[currentBgmIndex].tracks, audioClipsEditCount);
                updateBGM();
            }

            GUILayout.EndScrollView();
            soundClipsList.MusicTracksList[currentBgmIndex].volume = EditorGUILayout.Slider("Volume", soundClipsList.MusicTracksList[currentBgmIndex].volume, 0, 1f);
            soundClipsList.MusicTracksList[currentBgmIndex].loopLastElement = EditorGUILayout.Toggle("Loop Last Element", soundClipsList.MusicTracksList[currentBgmIndex].loopLastElement);
            soundClipsList.MusicTracksList[currentBgmIndex].fadeIn = EditorGUILayout.Toggle("FadeIn", soundClipsList.MusicTracksList[currentBgmIndex].fadeIn);
            soundClipsList.MusicTracksList[currentBgmIndex].fadeOut = EditorGUILayout.Toggle("FadeOut", soundClipsList.MusicTracksList[currentBgmIndex].fadeOut);


            if (GUILayout.Button("Save"))
            {
 
                updateBGM();
            }
            if (GUILayout.Button("Delete"))
            {

                if (EditorUtility.DisplayDialog("Delete " + currentBGM[currentBgmIndex], $" [{ currentBGM[currentBgmIndex]}] This will be remove from  Music Traks list, are you sure?","Do it", "Dont"))
                {
                    updateBGM();
                    soundClipsList.MusicTracksList.RemoveAt(currentBgmIndex);
                    updateBGM();
                    GUI.FocusControl(null);
                }
            }
            
            EditorGUILayout.EndVertical();
            GUILayout.Space(6);
        }
        void BGM_CreateWindow()
        {
            GUILayout.Space(6);
            EditorGUILayout.BeginVertical("Box");
            GUILayout.Space(15);

            mt.name = EditorGUILayout.TextField("Track Name", mt.name, GUILayout.Width(position.width - 20));
            EditorGUILayout.EndVertical();

            GUILayout.Space(10);
            EditorGUILayout.BeginVertical("Box", GUILayout.Height(90), GUILayout.Width(position.width));
            EditorGUILayout.LabelField("Tracks", AudioManagerUtils.LabelStyle());
            audioClipsCount = ((audioClips.IsNullOrEmpty())?0: audioClips.Count);

            AudioClipsScroll = GUILayout.BeginScrollView(AudioClipsScroll);
            for (int i = 0; i < audioClipsCount; i++)
            {
                audioClips[i] = (AudioClip)EditorGUILayout.ObjectField("Audio Clip: "+ (i+1), audioClips[i], typeof(AudioClip), false, GUILayout.Width(position.width - 20));
            }
     
            GUILayout.EndScrollView();
     
            EditorGUILayout.EndVertical();
            if (GUILayout.Button("Add Audio Clip",GUILayout.Width(position.width - 20)))
            {
                audioClipsCount++;
                audioClips.Resize(audioClipsCount);
            }
            if (GUILayout.Button("Remove Audio Clip",GUILayout.Width(position.width - 20)))
            {
                audioClipsCount--;
                audioClips.Resize(audioClipsCount);
            }

            mt.tracks = audioClips.ToArray();
            mt.group = (AudioMixerGroup)EditorGUILayout.ObjectField("Audio Group", mt.group, typeof(AudioMixerGroup), false, GUILayout.Width(position.width - 20));
            mt.musicTrackEvent = (SoundtrackEvent)EditorGUILayout.EnumPopup("TriggerEvent: ",mt.musicTrackEvent, GUILayout.Width(position.width - 20));
            if (mt.musicTrackEvent == SoundtrackEvent.OnSceneLoad)
            {
                currentSceneIndex = EditorGUILayout.Popup("Scene", currentSceneIndex,AudioManagerUtils.GetCurrentScenesName(), GUILayout.Width(position.width - 18));
                mt.sceneName = AudioManagerUtils.GetCurrentScenesName()[currentSceneIndex];
            }
            GUILayout.BeginHorizontal();
            mt.loopLastElement = EditorGUILayout.Toggle("LoopLast", mt.loopLastElement);
            mt.fadeIn = EditorGUILayout.Toggle("FadeIn", mt.fadeIn);
            mt.fadeOut = EditorGUILayout.Toggle("FadeOut", mt.fadeOut);
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Create", GUILayout.Width(position.width - 20)))
            {
                if (soundClipsList.check_Duplication(mt.name))
                {
                    soundClipsList.MusicTracksList.Add(mt);
                    mt = new SoundTrack();
                    audioClips = new List<AudioClip>();
                    updateBGM();
                }
            }

            GUILayout.Space(6);

        }



        void updateBGM()
        {
            AssetDatabase.Refresh();
            currentBGM = soundClipsList.SoundTracks();
            EditorUtility.SetDirty(soundClipsList);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        void updateSoundList()
        {

            EditorUtility.SetDirty(soundClipsList);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

        }
    }
}
