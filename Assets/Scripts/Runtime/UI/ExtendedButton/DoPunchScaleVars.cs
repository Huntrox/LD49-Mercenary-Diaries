
using UnityEngine;
using DG.Tweening;

namespace HuntroxGames.Utilities
{
    [System.Serializable]
    public class DoPunchScaleVars
    {
        public AnimationCurve ease = AnimationCurve.Linear(1, 1, 1,1);
       // public Ease ease;
        public Vector3 punchScale;
        public  int Vibrato;
        public  float Elasticity;
        public  float Duration;
    }
}