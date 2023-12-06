using UnityEngine;

namespace GameCode.Tools
{
    /// <summary>
    /// 变量计数器，声明此对象，之后使用Add方法增加计数
    /// </summary>
    public class VariableCounter
    {
        private int count=0;

        public void Add(int value)
        {
            count += value;
        }

        public int Value()
        {
            return count;
        }
        
    }
}
