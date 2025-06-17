using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using VARLab.SCORM;

public class AzureManager : MonoBehaviour
{
    public delegate void OnBlobDeletedEvent(bool success);

    public event OnBlobDeletedEvent OnBlobDeleted;


    [HideInInspector] public string DEFAULT_CONTAINER;
    [HideInInspector] public readonly string SHOP_CONTAINER = "prmshop";
    [HideInInspector] public readonly string LATHE_CONTAINER = "prmlathe";
    [HideInInspector] public readonly string SURFACEGRINDER_CONTAINER = "prmsurfacegrinder";
    [HideInInspector] public readonly string MILLINGMACHINE_CONTAINER = "prmmillingmachine";
    [HideInInspector] public string DEFAULT_CALL = "iscompleted";
    private const string ENDPOINT_URL = "https://varlabcloudsave.azurewebsites.net/core/";

    private enum ActionType
    {
        Save,
        Load,
        Delete
    }

    private enum Module
    {
        ShopSafety,
        Lathe,
        SurfaceGrinder,
        MillingMachine
    }

    [SerializeField, Tooltip("Select the module from the list")]private Module module;

    [Serializable]
    private struct Savedata
    {
        public string Content;
    }

    private struct PasswordPacket
    {
        public string Password;
    }

    /// <summary>Token used to authorize requests to the saveload API.</summary>
    private string authorizeToken;

    /// <summary>HasLoaded checks if the initial load call has been called and a result is back. The bool will truen true even if the callback resut is a protocol error </summary>
    [HideInInspector] public bool HasLoaded;

    [HideInInspector] public bool SaveFileExists;

    /// <summary>Data loaded from the saveload API.</summary>
    public string LoadedData { get; set; }

    /// <summary>Name of the student's file to load and save to.</summary>
    public string FileName { get; private set; }

    private const string INSTRUCTOR_ID = "preview";


    private void Update()
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        // Deletes the current user's save file in Azure
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            StartCoroutine(AuthorizeRequest(ActionType.Delete, $"{GetFullFileUrl(GetContainerName(), SetFileName())}"));
        }
#endif
    }

    private string SetFileName()
    {
#if UNITY_EDITOR || !UNITY_WEBGL
        return FileName = $"{SystemInfo.deviceUniqueIdentifier}.json";
#else
           return FileName = $"{(ScormIntegrator.LearnerId)}.json";
#endif
    }

    public void Save(string containername, string saveData)
    {
        var url = $"{GetFullFileUrl(containername, SetFileName())}/uploadcontent";
        var wrapper = new Savedata();
        wrapper.Content = saveData;
        saveData = JsonUtility.ToJson(wrapper);

        StartCoroutine(AuthorizeRequest(ActionType.Save, url, saveData));
    }

    public void Load(string containername)
    {
        var url = $"{GetFullFileUrl(containername, SetFileName())}";
        StartCoroutine(AuthorizeRequest(ActionType.Load, url));
    }

    private static string GetFullFileUrl(string containerName, string fileName)
    {
        return $"{ENDPOINT_URL}{containerName}/{fileName}";
    }

    #region Auth

    private IEnumerator AuthorizeRequest(ActionType actionType, string url, string saveData = null)
    {
        PasswordPacket passwordData = new PasswordPacket();
        passwordData.Password = "icecreamb23i4b2kh";
        string jsonPassword = JsonUtility.ToJson(passwordData);

        using (UnityWebRequest request = UnityWebRequest.Put(ENDPOINT_URL + "authenticate", jsonPassword))
        {
            request.method = UnityWebRequest.kHttpVerbPOST;
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
            }
            else
            {
                authorizeToken = request.downloadHandler.text.Trim('"');

                if (actionType == ActionType.Save)
                {
                    StartCoroutine(PostRequest(url, saveData));
                }
                else if (actionType == ActionType.Load)
                {
                    StartCoroutine(GetRequest(url));
                }
                else if (actionType == ActionType.Delete)
                {
                    StartCoroutine(DeleteRequest(url));
                }
            }
        }
    }

    #endregion Auth

    #region Get

    private IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            webRequest.SetRequestHeader("Authorization", "Bearer " + authorizeToken);

            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            LoadedData = "0";

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.Log(pages[page] + ": Error: " + webRequest.error);
                    SaveFileExists = false;
                    break;

                case UnityWebRequest.Result.ProtocolError:
                    Debug.Log(pages[page] + ": HTTP Error: " + webRequest.error);
                    SaveFileExists = false;
                    break;

                case UnityWebRequest.Result.Success:
                    Debug.Log("Load Successful");
                    SaveFileExists = true;
                    LoadedData = webRequest.downloadHandler.text;
                    Debug.Log(LoadedData);
                    break;
            }
            HasLoaded = true;
        }
    }

    #endregion Get

    #region Delete

    private IEnumerator DeleteRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Delete(uri))
        {
            webRequest.method = UnityWebRequest.kHttpVerbDELETE;
            webRequest.SetRequestHeader("Authorization", "Bearer " + authorizeToken);

            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(webRequest.error);
                OnBlobDeleted?.Invoke(false);
            }
            else
            {
                Debug.Log("Save File Deleted On Azure");
                SaveFileExists = false;
                OnBlobDeleted?.Invoke(true);
            }
        }
    }

    #endregion Delete

    #region Post

    private IEnumerator PostRequest(string uri, string saveData)
    {
        using (UnityWebRequest request = UnityWebRequest.Put(uri, saveData))
        {
            request.method = UnityWebRequest.kHttpVerbPOST;
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + authorizeToken);

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
            }
            else
            {
                Debug.Log("Upload Successful");
            }
        }
    }

    #endregion Post

    #region Helper
    public string GetContainerName()
    {
        switch (module)
        {
            case Module.ShopSafety:
                DEFAULT_CONTAINER = SHOP_CONTAINER;
                break;
            case Module.Lathe:
                DEFAULT_CONTAINER = LATHE_CONTAINER;
                break;
            case Module.SurfaceGrinder:
                DEFAULT_CONTAINER = SURFACEGRINDER_CONTAINER;
                break;
            case Module.MillingMachine:
                DEFAULT_CONTAINER = MILLINGMACHINE_CONTAINER;
                break;
            default:
                break;
        }

        Debug.Log(DEFAULT_CONTAINER);

        return DEFAULT_CONTAINER;
    }
    #endregion
}