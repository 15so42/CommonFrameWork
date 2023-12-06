using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUiPanel
{
    void EnablePanel();
    void DisablePanel();
    void ClosePanel();
    public string GetObjName();

    public GameObject GetGameObject();
}
