using Newtonsoft.Json;

namespace EduMetricsApi.Application.DTO;

public class UserCredentialsDto
{
    [JsonProperty("username")]
    public string UserName { get; set; }

    [JsonProperty("userpassword")]
    public string UserPassword { get; set; }

    public ComputerInformationsDto ComputerInformations { get; set; }
}