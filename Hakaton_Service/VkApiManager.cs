using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model.RequestParams;

namespace Hakaton_Service
{
    public class VkApiManager
    {
        private static VkApiManager _instance;
        private static readonly object Locker = new object();

        public static VkApiManager GetInstance(ulong appId, string login, string password)
        {
            if (_instance == null)
            {
                lock (Locker)
                {
                    if (_instance == null)
                    {
                        _instance = new VkApiManager(new VkApi(), appId, login, password);
                    }
                }
            }

            return _instance;
        }

        private readonly VkApi _api;

        private VkApiManager(VkApi api, ulong appId, string login, string password)
        {
            try
            {
                _api = api;
                var apiAuthParams = new ApiAuthParams
                {
                    ApplicationId = appId,
                    Login = login,
                    Password = password,
                    Settings = Settings.All
                };
                _api.Authorize(apiAuthParams);
            }
            catch
            {
                Debug.Print("Error VkApi");
            }
        }

        public long? GetUserId() => _api.UserId;
        public string GetAccessToken() => _api.Token;

        public string GetFio()
        {
            var profileInfo = _api.Account.GetProfileInfo();
            return $"{profileInfo.LastName} {profileInfo.FirstName}";
        }

        public List<(long, string)> GetFriends()
        {
            var friends = _api.Friends.Get(new FriendsGetParams
                {
                    UserId = _api.UserId
                }).Select(x => (x.Id,
                    x.FirstName + " " + x.LastName))
                .ToList();
            return friends;
        }
    }
}