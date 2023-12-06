using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WarningDialogContext : DialogContext
{
    
    public string title;
    public string content;
    public Action action;
    public string buttonStr;
    public bool useUiStack;
    public bool hideCloseBtn;
    public string secondButtonText;
    public Action secondButtonAction;
    
    public WarningDialogContext(string title,Action action,string content, string buttonStr, bool useUiStack,bool hideCloseBtn, string secondButtonText="",Action secondButtonAction=null) 
    {
        this.action = action;
        this.title = title;
        this.content = content;
        this.buttonStr = buttonStr;
        this.useUiStack = useUiStack;
        this.hideCloseBtn = hideCloseBtn;
        this.secondButtonText = secondButtonText;
        this.secondButtonAction = secondButtonAction;
    }
}

public class WarningDialog : Dialog<WarningDialogContext>
{
    public Text titleText;
    public Text contentText;
    public Button confirmBtn;
    public TMP_Text buttonText;
    public Button hideCloseBtn;
    public Transform buttonGroupTrans;//记得RebuildLayout
    public Button secondButton;
    
    /// <summary>
    /// 生成警告Ui，当传入secondButtonText后会自动构造第二个按钮,点击第一个按钮和第二个按钮后都会自动关闭警告ui，第二个按钮Action可以传null，传null的时候表示点击的时候什么都不做直接关闭，
    /// 可用于取消操作。
    /// </summary>
    /// <param name="title"></param>
    /// <param name="action"></param>
    /// <param name="buttonStr"></param>
    /// <param name="content"></param>
    /// <param name="useUiStack"></param>
    /// <param name="hideCloseBtn"></param>
    /// <param name="secondButtonText"></param>
    /// <param name="secondButtonAction"></param>
    /// <param name="onClose"></param>
    public static void ShowDialog(string title,Action action,string buttonStr, string content,bool useUiStack,bool hideCloseBtn=false, string secondButtonText="",Action secondButtonAction=null, Action onClose=null)
    {
        //warningDialog一般只使用一次，所以不需要使用EnableDialog之类的方法，也就不需要额外持有引用
        DialogUtil.ShowDialogWithContext(nameof(WarningDialog), new WarningDialogContext(title, action, content,buttonStr, useUiStack,hideCloseBtn,secondButtonText,secondButtonAction),null,onClose);
    
    }
    

    public override void Show()
    {
        base.Show();
        titleText.text = dialogContext.title;
        contentText.text = dialogContext.content;
        
        //第二个按钮
        if (dialogContext.secondButtonText != "")
        {
            secondButton.gameObject.SetActive(true);
            secondButton.GetComponentInChildren<TMP_Text>().text = dialogContext.secondButtonText;
            var buttonComp = secondButton;

            buttonComp.onClick.AddListener(()=>
            {
                this.dialogContext.secondButtonAction?.Invoke();
                
                Close();
            });
        }
        else
        {
            secondButton.gameObject.SetActive(false);
        }
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(buttonGroupTrans.transform as RectTransform);
        
        
        confirmBtn.onClick.AddListener(()=>
        {
            dialogContext.action?.Invoke();
            Close();
        });
        buttonText.text = dialogContext.buttonStr;
        
       

        if (dialogContext.hideCloseBtn)
        {
            hideCloseBtn.gameObject.SetActive(false);
        }
        
       
    }

    public override void ClosePanel()
    {
        base.ClosePanel();
        Close();
    }
}
