using System;
using NFTConnect;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Economy;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unity.Services.Samples.LootBoxes
{
    public class SelectLEvelManager : MonoBehaviour
    {
        public int indexSelect = 1;
        async void Start()
        {
           
            try
            {
                await UnityServices.InitializeAsync();

                // Check that scene has not been unloaded while processing async wait to prevent throw.
                if (this == null) return;

                if (!AuthenticationService.Instance.IsSignedIn)
                {
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();
                    if (this == null) return;
                }

                Debug.Log($"Player id:{AuthenticationService.Instance.PlayerId}");

                // Economy configuration should be refreshed every time the app initializes.
                // Doing so updates the cached configuration data and initializes for this player any items or
                // currencies that were recently published.
                // 
                // It's important to do this update before making any other calls to the Economy or Remote Config
                // APIs as both use the cached data list. (Though it wouldn't be necessary to do if only using Remote
                // Config in your project and not Economy.)
                await EconomyManager.instance.RefreshEconomyConfiguration();
                if (this == null) return;

                await EconomyManager.instance.RefreshCurrencyBalances();
                if (this == null) return;

                //sceneView.SetInteractable();

                Debug.Log("Initialization and signin complete.");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public async void EnterGame()
        {
            try
            {
                await EconomyService.Instance.PlayerBalances.DecrementBalanceAsync("ELIXIR", 5);
                await EconomyService.Instance.PlayerBalances.DecrementBalanceAsync("PILL", 5);
                Debug.Log("use currency to Entergame");
                SceneManager.LoadScene("Level" +indexSelect.ToString());

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public void ChoseLevel(int i)
        {
            indexSelect = i;
        }

    }
}