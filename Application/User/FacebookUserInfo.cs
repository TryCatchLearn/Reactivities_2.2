using Newtonsoft.Json;

namespace Application.User
{
    public class FacebookUserInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        public string Username { get; set; }
        public FacebookPictureData Picture { get; set; }
    }

    public class FacebookPictureData
    {
        public FacebookPicture Data { get; set; }
    }

    public class FacebookPicture
    {
        public string Url { get; set; }
    }
}