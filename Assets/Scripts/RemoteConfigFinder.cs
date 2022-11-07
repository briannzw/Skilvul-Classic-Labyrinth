using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.RemoteConfig;
using UnityEngine;

public struct userAttributes
{
    public int characterLevel;
}

public struct appAttributes
{

}

public class RemoteConfigFinder : MonoBehaviour
{
    public string environmentName;
    public int characterLevel;
    public bool fetch;
    public float gravity;
    public PhoneGravity phoneGravity;

    private async void Awake()
    {
        var options = new InitializationOptions();
        options.SetEnvironmentName(environmentName);
        await UnityServices.InitializeAsync(options);

        Debug.Log("UGS Initialized");

        if(!AuthenticationService.Instance.IsSignedIn)
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

        Debug.Log("Player signed in");

        RemoteConfigService.Instance.FetchCompleted += OnFetchConfig;

        Debug.Log("Fetch Config");
        RemoteConfigService.Instance.FetchConfigs(
            new userAttributes() { characterLevel = this.characterLevel },
            new appAttributes());
    }

    private void OnDestroy()
    {
        RemoteConfigService.Instance.FetchCompleted -= OnFetchConfig;
    }

    private void OnFetchConfig(ConfigResponse response)
    {
        Debug.Log(response.requestOrigin);
        Debug.Log(response.body);

        switch (response.requestOrigin)
        {
            case ConfigOrigin.Default:
                Debug.Log("Default");
                break;
            case ConfigOrigin.Cached:
                Debug.Log("Cached");
                break;
            case ConfigOrigin.Remote:
                Debug.Log("Remote");
                gravity = RemoteConfigService.Instance.appConfig.GetFloat("gravity");
                phoneGravity.gravityMagnitude = gravity;
                break;
        }
    }

    private void Update()
    {
        if (fetch)
        {
            fetch = false;
            Debug.Log("Fetch Config");
            RemoteConfigService.Instance.FetchConfigs(
                new userAttributes() { characterLevel = this.characterLevel },
                new appAttributes());
        }
    }
}
