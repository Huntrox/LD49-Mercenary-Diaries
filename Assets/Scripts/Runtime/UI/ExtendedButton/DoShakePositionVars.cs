using UnityEngine;
using DG.Tweening;

namespace HuntroxGames.Utilities
{
    [System.Serializable]
    public class DoShakePositionVars
    {


        public Ease ease;
        public Vector3 strength;
        public float duration;
        public int Vibrato;
        public float randomness;
        public  bool snapping;
        public  bool fadeOut;

    }
}