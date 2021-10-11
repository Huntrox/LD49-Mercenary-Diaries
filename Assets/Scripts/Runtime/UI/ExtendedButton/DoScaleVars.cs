
using UnityEngine;
using DG.Tweening;

namespace HuntroxGames.Utilities
{
    [System.Serializable]
    public class DoScaleVars
    {
        public AnimationCurve ease = AnimationCurve.Linear(0, 1, 1, 0);
      //  public Ease ease;
     //   public Vector3 Scale;
        public float EndScale;
        public float Duration;
    }
}
