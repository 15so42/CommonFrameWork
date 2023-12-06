using UnityEngine;

namespace Interface
{
   public interface IStateAI<T>
   {
      public AIState<T> CurState { get; set; }

      public void SetState(AIState<T> nextState);
      public GameObject GetObj();
   }
}
