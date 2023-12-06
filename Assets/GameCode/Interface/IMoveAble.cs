using System.Numerics;

namespace Interface
{
    public interface IMoveAble
    {
        public float MoveSpeed { get; set; }

        public void SetTargetPos(UnityEngine.Vector3 newPos);
    }
}
