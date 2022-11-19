using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Unity.Services.Samples.SeasonalEvents
{
    public class SeasonalEventsSceneManager : MonoBehaviour
    {
        public SeasonalEventsSampleView sceneView;
        public CountdownManager countdownManager;

        bool m_Updating = false;

        AsyncOperationHandle<IList<Sprite>> m_BackgroundImageHandle;
        AsyncOperationHandle<IList<GameObject>> m_PlayButtonPrefabHandle;
        AsyncOperationHandle<IList<GameObject>> m_PlayChallengeButtonPrefabHandle;


        async void Start()
        {
            try
            {
                await InitializeServices();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        async Task InitializeServices()
        {
            UpdateStarted();

            try
            {
                await UnityServices.InitializeAsync();

                // Check that scene has not been unloaded while processing async wait to prevent throw.
                if (this == null) return;
                    
                Debug.Log("Services Initialized.");

                if (!AuthenticationService.Instance.IsSignedIn)
                {
                    Debug.Log("Signing in...");
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();
                    if (this == null) return;
                }
                Debug.Log($"Player id: {AuthenticationService.Instance.PlayerId}");

                // Economy configuration should be refreshed every time the app initializes.
                // Doing so updates the cached configuration data and initializes for this player any items or
                // currencies that were recently published.
                // 
                // It's important to do this update before making any other calls to the Economy or Remote Config
                // APIs as both use the cached data list. (Though it wouldn't be necessary to do if only using Remote
                // Config in your project and not Economy.)
                await EconomyManager.instance.RefreshEconomyConfiguration();
                if (this == null) return;

                // Set server time offset. Needed throughout initialization and while event is running to
                // determine what time it is on the server so we can time seasons locally to match.
                await UpdateServerEpochTime();
                if (this == null) return;

                await Task.WhenAll(
                    EconomyManager.instance.RefreshCurrencyBalances(),
                    GetRemoteConfigUpdates(),
                    CloudSaveManager.instance.LoadAndCacheData());
                if (this == null) return;

                sceneView.sceneInitialized = true;
            }
            finally
            {
                // The finally statement will attempt to execute no matter what happens in the try block,
                // so our check for whether the scene has been unloaded while processing the last async wait
                // of the try block has to happen in the finally. Since we can't exit a finally block early
                // we will only call UpdateFinished if the scene hasn't been unloaded.
                if (this != null)
                {
                    UpdateFinished();
                }
            }
        }

        async Task UpdateServerEpochTime()
        {
            // Call simple server script to determine the current server epoch time.
            double serverEpochTime = await CloudCodeManager.instance.CallGetServerEpochTimeEndpoint();

            ServerTimeHelper.SetServerEpochTime(serverEpochTime);
        }

        void UpdateStarted()
        {
            m_Updating = true;
            sceneView.SetInteractable(false);
        }

        void UpdateFinished()
        {
            m_Updating = false;
            UpdateStateAndEnableSceneView();
        }

        void UpdateStateAndEnableSceneView()
        {
            // The Cloud Code script GrantEventReward will only distribute rewards once per season in a given cycle.
            // So that players don't click the PlayChallenge button thinking they'll get rewards when they won't, we
            // set playChallengeAllowed to false, which disables the button.
            sceneView.playChallengeAllowed = IsPlayingChallengeAllowed();
            sceneView.SetInteractable();
        }

        private bool IsPlayingChallengeAllowed()
        {
            // If you'd like to leave the PlayChallenge button enabled, in order to test the Cloud Code script's 
            // distribution protections, uncomment the next line:
            // return true;

            if (IsLastCompletedEventKeyDifferentFromActiveEvent())
            {
                return true;
            }

            // Because the seasonal events cycle, and do not have unique keys for each cycle, we need to check that
            // the last time the event challenge was completed, is outside of the possible timespan for the current
            // event.
            return IsLastCompletedEventTimestampOld();
        }

        private bool IsLastCompletedEventKeyDifferentFromActiveEvent()
        {
            return !string.Equals(
                CloudSaveManager.instance.GetLastCompletedActiveEvent(),
                RemoteConfigManager.instance.activeEventKey);
        }

        private bool IsLastCompletedEventTimestampOld()
        {
            // Determine the approximate utc time on the server.
            // Note: We need to use the server time to ensure we are showing/claiming the correct season in case the 
            //       client's clock is off for any reason.
            var currentTime = ServerTimeHelper.UtcNow;
            var eventDuration = new TimeSpan(0, RemoteConfigManager.instance.activeEventDurationMinutes, 0);
            var earliestPotentialStartForActiveEvent = currentTime - eventDuration;

            return CloudSaveManager.instance.GetLastCompletedEventTimestamp() < earliestPotentialStartForActiveEvent;
        }

        async Task GetRemoteConfigUpdates()
        {
            await RemoteConfigManager.instance.FetchConfigs();
            if (this == null) return;
            await UpdateSeasonOnView();
        }

        async Task UpdateSeasonOnView()
        {
            sceneView.UpdateRewardView();
            countdownManager.StartCountdownFromNow();
            await UpdateSeasonalAddressables();
        }

        async Task UpdateSeasonalAddressables()
        {
            // This method is only called when the season has changed. Since we're done with the last season's
            // assets, we'll release the Async handles to them before loading next season's assets.
            ReleaseHandlesIfValid();
            await LoadSeasonalAddressables();
        }

        void ReleaseHandlesIfValid()
        {
            if (m_BackgroundImageHandle.IsValid())
            {
                Addressables.Release(m_BackgroundImageHandle);
            }

            if (m_PlayButtonPrefabHandle.IsValid())
            {
                Addressables.Release(m_PlayButtonPrefabHandle);
            }

            if (m_PlayChallengeButtonPrefabHandle.IsValid())
            {
                Addressables.Release(m_PlayChallengeButtonPrefabHandle);
            }
        }

        async Task LoadSeasonalAddressables()
        {
            m_BackgroundImageHandle = Addressables.LoadAssetsAsync<Sprite>(
                new List<string>{ RemoteConfigManager.instance.activeEventKey, "Sprites/BackgroundImage" },
                LoadSeasonalBackgroundImage,
                Addressables.MergeMode.Intersection
            );
            m_PlayButtonPrefabHandle = Addressables.LoadAssetsAsync<GameObject>(
                new List<string>{ RemoteConfigManager.instance.activeEventKey, "Prefabs/PlayButton" },
                LoadSeasonalPlayButton,
                Addressables.MergeMode.Intersection
            );
            m_PlayChallengeButtonPrefabHandle = Addressables.LoadAssetsAsync<GameObject>(
                new List<string>{ RemoteConfigManager.instance.activeEventKey, "Prefabs/PlayChallengeButton" },
                LoadSeasonalPlayChallengeButton,
                Addressables.MergeMode.Intersection
            );

            await Task.WhenAll(m_BackgroundImageHandle.Task, 
                    m_PlayButtonPrefabHandle.Task, 
                    m_PlayChallengeButtonPrefabHandle.Task);
        }

        void LoadSeasonalBackgroundImage(Sprite backgroundImage)
        {
            sceneView.UpdateBackgroundImage(backgroundImage);
        }

        void LoadSeasonalPlayButton(GameObject playButtonPrefab)
        {
            sceneView.UpdatePlayButton(playButtonPrefab);
        }

        void LoadSeasonalPlayChallengeButton(GameObject playChallengeButtonPrefab)
        {
            sceneView.UpdatePlayChallengeButton(playChallengeButtonPrefab);
            sceneView.playChallengeButton.onClick.AddListener(OnPlayChallengeButtonPressed);
        }

        async void LateUpdate()
        {
            try
            {
                if (!m_Updating)
                {
                    await UpdateSeason();
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        async Task UpdateSeason()
        {
            // Because our events are time-based and change so rapidly (every 2 - 3 minutes), we will check each
            // update if it's time to refresh Remote Config's local data, and refresh it if the current
            // last digit of the minutes equals the start of the next game override's time (See more info in the
            // comments in GetUserAttributes). More typically you would probably fetch new configs at app launch
            // and under other less frequent circumstances.
            // Note: We use the approximate server time here to ensure we are showing/claiming the correct season
            //       in case the client's clock is off for any reason.
            var currentMinuteLastDigit = ServerTimeHelper.UtcNow.Minute % 10;

            if (currentMinuteLastDigit > RemoteConfigManager.instance.activeEventEndTime ||
                (currentMinuteLastDigit == 0 && RemoteConfigManager.instance.activeEventEndTime == 9))
            {
                try
                {
                    UpdateStarted();
                    Debug.Log(
                        "Getting next seasonal event from Remote Config and refreshing Cloud Save data...");

                    await Task.WhenAll(
                        RemoteConfigManager.instance.FetchConfigs(),
                        CloudSaveManager.instance.LoadAndCacheData());
                    if (this == null) return;

                    await UpdateSeasonOnView();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
                finally
                {
                    if (this != null)
                    {
                        UpdateFinished();
                    }
                }
            }
        }

        public void OnPlayChallengeButtonPressed()
        {
            sceneView.ShowRewardPopup(RemoteConfigManager.instance.challengeRewards);
            sceneView.SetInteractable(false);
        }

        public async void OnCloseRewardPopupPressed()
        {
            try
            {
                await CloudCodeManager.instance.CallGrantEventRewardEndpoint();
                if (this == null) return;

                await CloudSaveManager.instance.LoadAndCacheData();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            finally
            {
                // The finally statement will attempt to execute no matter what happens in the try block,
                // so our check for whether the scene has been unloaded while processing the last async wait
                // of the try block has to happen in the finally. Since we can't exit a finally block early
                // we will only call Close if the scene hasn't been unloaded.
                if (this != null)
                {
                    sceneView.CloseRewardPopup();
                    UpdateStateAndEnableSceneView();
                }
            }
        }
    }
}
