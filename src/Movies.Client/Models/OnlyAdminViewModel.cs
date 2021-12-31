using System.Collections.Generic;

namespace Movies.Client.Models
{
    public class OnlyAdminViewModel
    {
        public Dictionary<string, string> UserInfoDictionary { get; private set; }

        public OnlyAdminViewModel(Dictionary<string, string> userInfoDictionary)
        {
            UserInfoDictionary = userInfoDictionary;
        }
    }
}
