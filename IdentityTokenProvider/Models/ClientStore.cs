using IdentityTokenProvider.Dto;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace IdentityTokenProvider.Models
{

    public class ClientStore
    {
        public static string AuthURL { get; set; }
        public static string AuthUserURL { get; set; }
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
              new ApiResource("all", "all")//can be stored as needed in db
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
              new IdentityResources.OpenId()
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            
            IEnumerable<ClientCredentialDto>dto = null;
            List<Client> ClientList = new List<Client>(); 
            using (var client = new HttpClient())
            {
                //client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", base64bytes);
                //List<KeyValuePair<string, string>> requestData = new List<KeyValuePair<string, string>>();
                //requestData.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));

                //FormUrlEncodedContent requestBody = new FormUrlEncodedContent(requestData);
                var request = client.GetAsync(AuthURL); //client.PostAsync(_tokenURL, requestBody);

                if (request.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var responseContent = request.Result.Content.ReadAsStringAsync();
                    //dto = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<ClientCredentialDto>>(responseContent.Result);                 
                    dto = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<ClientCredentialDto>>(responseContent.Result);                    
                }
                else
                {
                    var responseContent = request.Result.Content.ReadAsStringAsync();
                    throw new Exception("Cannot client list" + responseContent.Result);
                }
            }

            if(dto != null && dto.Count() > 0)
            {
                int id = 0;
                foreach(ClientCredentialDto cdto in dto)
                {
                    Client client = new Client();
                    client.ClientId = cdto.ClientID;                    
                    client.ClientName = cdto.ClientID;
                    client.ClientSecrets.Add(new Secret(cdto.ClientSecret.ToSha256()));
                    if (cdto.AllowedGrantTypes != "ResourceOwnerPassword".ToLower())
                    {
                        client.AllowedGrantTypes = GrantTypes.ResourceOwnerPassword;
                        client.AllowedScopes.Add(IdentityServerConstants.StandardScopes.OpenId);
                        client.AllowedScopes.Add(IdentityServerConstants.StandardScopes.OfflineAccess);
                        client.AllowOfflineAccess = true;
                        client.RefreshTokenUsage = cdto.RefreshTokenUsage;
                        client.AccessTokenLifetime = cdto.AccessTokenLifetime;
                        client.RefreshTokenExpiration = cdto.RefreshTokenExpiration;
                        client.AbsoluteRefreshTokenLifetime = cdto.AbsoluteRefreshTokenLifetime;

                        //else
                        //{
                        //    client.AllowedGrantTypes = GrantTypes.ClientCredentials;
                        //    client.AllowOfflineAccess = true;
                        //    client.AccessTokenLifetime = 360;
                        //    client.AllowedScopes.Add("all");
                        //    client.RefreshTokenUsage = cdto.RefreshTokenUsage;//identity server 4 does not allow refresh token with client credential
                        //    client.RefreshTokenExpiration = cdto.RefreshTokenExpiration;
                        //    client.AbsoluteRefreshTokenLifetime = cdto.AbsoluteRefreshTokenLifetime;
                        //    client.AllowedScopes.Add(IdentityServerConstants.StandardScopes.OfflineAccess);
                        //}*/

                        client.AllowedGrantTypes = GrantTypes.ClientCredentials;
                        client.AllowOfflineAccess = true;
                        client.AccessTokenLifetime = 360;
                        client.AllowedScopes.Add("all");
                        client.RefreshTokenUsage = cdto.RefreshTokenUsage;//identity server 4 does not allow refresh token with client credential
                        client.RefreshTokenExpiration = cdto.RefreshTokenExpiration;
                        client.AbsoluteRefreshTokenLifetime = cdto.AbsoluteRefreshTokenLifetime;
                        client.AllowedScopes.Add(IdentityServerConstants.StandardScopes.OfflineAccess);

                        ClientList.Add(client);
                    }
                }
            }

            return ClientList;



            /*return new List<Client>
            {
                new Client
                {
                    ClientName = "Resource Owner Flow",
                    ClientId = "resource_owner_flow",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets =
                    {
                    new Secret("resource_owner_flow_secret".Sha256())
                    },
                    AllowedScopes =
                    {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.OfflineAccess
                    },
                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    AccessTokenLifetime = 60,
                    RefreshTokenExpiration = TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime = 300
                },
                new Client
                {
                    ClientName = "Client Credential Flow",
                    ClientId = "client_credential_flow",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                    new Secret("client_credential_flow_secret".Sha256())
                    },
                    AllowedScopes =
                    {
                    "all",
                    IdentityServerConstants.StandardScopes.OfflineAccess
                    },
                    AllowOfflineAccess = true,
                    AccessTokenLifetime = 360
                }
            };*/
        }

        public static UserDto GetUser(string userName, string passWord)
        {           

            using (var client = new HttpClient())
            {
                UserDto userdto = new UserDto();
                userdto.UserName = userName;
                userdto.Password = passWord;


                string stringData = JsonConvert.SerializeObject(userdto);
                var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");

                var response = client.PostAsync(
                    AuthUserURL,
                    contentData).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var responseContent = response.Content.ReadAsStringAsync();
                    var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<UserDto>(responseContent.Result);
                    return obj;
                  
                }
                else
                {
                    //var resultVal = request.Result.Content.ReadAsAsync<string>().Result;
                    //throw new Exception("Cannot client list" + resultVal);
                }
            }
            return null;
        }

    }
}
