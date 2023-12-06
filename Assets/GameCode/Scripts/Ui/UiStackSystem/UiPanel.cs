using UnityEngine;

namespace _MyGame.Scripts.UiStackSystem
{
    public class UiPanel : MonoBehaviour, IUiPanel
    {
        


        public void EnablePanel()
        {
            gameObject.SetActive(true);
        }

        public void DisablePanel()
        {
            gameObject.SetActive(false);
        }

        public virtual void ClosePanel()
        {
       
      
        }

        public string GetObjName()
        {
            return gameObject.name;
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        
    }
}