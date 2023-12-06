using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class UiStackManager : Singleton<UiStackManager>
{
    [Header("ui堆栈")] public Stack<IUiPanel> uiStack = new Stack<IUiPanel>();

    [FormerlySerializedAs("RemoveAllToNone")] [Header("从有到无移除掉所有UiPanel")]
    public UnityEvent PopAllToNone;
    
    #region Ui堆栈
    /// <summary>
    /// 有其他类型的ui
    /// </summary>
    /// <returns></returns>
    public bool HasAnyOtherUi(params Type[] allowedTypes)
    {
        return uiStack.Any(panel => !allowedTypes.Contains(panel.GetType()));
    }


    public bool HasAnyUi()
    {
        return uiStack.Count > 0;
    }
    
    public void RemoveFromStack<T>(Stack<T> stack, T itemToRemove)
    {
        Stack<T> tempStack = new Stack<T>();
        bool found = false;

        while (stack.Count > 0)
        {
            T item = stack.Pop();
            if (!item.Equals(itemToRemove) || found)  // if item is found more than once, remove only the first occurrence
            {
                tempStack.Push(item);
            }
            else
            {
                found = true;
            }
        }

        while (tempStack.Count > 0)
        {
            stack.Push(tempStack.Pop());
        }
    }

    public bool ContainsPanelOfType<T>()
    {
        return uiStack.Any(panel => panel is T);
    }

    public void EscTopUi(InputAction.CallbackContext context)
    {

        if (uiStack.Count == 0)
        {
            PopAllToNone?.Invoke();
            return;
        }
        
        if (uiStack.Count > 0)
        {
            uiStack.Peek().ClosePanel();//使用peek而不是pop,避免重复调用PopUi,因为panel基本在Disable的时候会自动pop
        }
        
        
        
    }

    /// <summary>
    /// 重要面板使用，比如玩家死亡。
    /// </summary>
    public void CloseAllUi()
    {
        while (uiStack.Count > 0)
        {
            EscTopUi(default);
        }
        
    }

    public string GetUiStackStr()
    {
        return string.Join(", ", uiStack.Select(panel => panel.GetType()));
    }
    public void PushUI(IUiPanel panel)
    {
       
        uiStack.Push(panel);
        //Debug.Log("UI堆栈入栈："+panel+",当前："+GetUiStackStr());
        
        AudioManager.instance.PlayAudio2D("Ui入栈");
        Cursor.lockState = CursorLockMode.None;
        EventCenter.Broadcast(EnumEventType.OnDialogOpen);
       
    }

    
    public void PopUI()
    {

       
        if (uiStack.Count > 0)
        {
            var toPop = uiStack.Pop();
            //Debug.Log("UI堆栈出栈："+toPop +",剩下："+GetUiStackStr());
         
            AudioManager.instance.PlayAudio2D("Ui出栈");
        }

        if (uiStack.Count == 0)//ui清空了
        {
            Cursor.lockState = CursorLockMode.Locked;
            EventCenter.Broadcast(EnumEventType.OnDialogClose);
        }
    }

    public IUiPanel GetTopPanel()
    {
        if(uiStack.Count>0)
            return uiStack.Peek();
        return null;
    }
    

    #endregion
}
