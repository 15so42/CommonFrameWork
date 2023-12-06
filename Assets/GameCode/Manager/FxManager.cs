using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

[System.Serializable]
public class FxConfig
{
    public string fxName;
    public GameObject fxPrefab;
    public float destroyTime;
}

public class FxManager : NetworkBehaviour
{
    public static FxManager Instance;

    public List<FxConfig> fxConfigs;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
       
    }

    [ServerRpc(RequireOwnership = false,RunLocally = true)]
    public void PlayFxByServer(string fxName, Vector3 position, Quaternion rotation)
    {
        PlayFxByObserver(fxName,position,rotation);
    }

    [ObserversRpc(RunLocally = true,ExcludeOwner = true)]
    public void PlayFxByObserver(string fxName, Vector3 position, Quaternion rotation)
    {
        PlayFx(fxName, position, rotation);
    }

   
    public GameObject PlayFx(string fxName, Vector3 position, Quaternion rotation)
    {
        FxConfig fxConfig = fxConfigs.Find(x => x.fxName == fxName);
        if (fxConfig != null)
        {
            GameObject fxPrefab = fxConfig.fxPrefab;
            GameObject fxInstance = Instantiate(fxPrefab, position, rotation);
            if(fxConfig.destroyTime>=0)
                Destroy(fxInstance, fxConfig.destroyTime);
            return fxInstance;
        }
        else
        {
            Debug.LogWarning("FxManager: Could not find fx with name " + fxName);
        }

        return null;
    }
}