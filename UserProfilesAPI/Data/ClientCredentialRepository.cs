using IdentityServer4.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using UserProfilesAPI.Dto;
using UserProfilesAPI.Model;

namespace UserProfilesAPI.Data
{
    public class ClientCredentialRepository: ICredentialRepository
    {
        AuthDbContext _context;
        private IConfiguration _configuration = null;
        public ClientCredentialRepository(AuthDbContext context,IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<ClientCredentialDto> Register(ClientCredentialDto credential)
        {
            //The following code works with SQLLite but entity framework has issues with SQL Server
            var clientCred = await _context.ClientCredential.FirstOrDefaultAsync(x => x.ClientID == credential.ClientID);
            if(clientCred == null)
            {
                ClientCredentials cred = new ClientCredentials();
                cred.AbsoluteRefreshTokenLifetime = credential.AbsoluteRefreshTokenLifetime;
                cred.AccessTokenLifetime = credential.AccessTokenLifetime;
                cred.AllowedGrantTypes = credential.AllowedGrantTypes;

                //if(credential.AllowOfflineAccess == null)
                //    cred.AllowOfflineAccess = 0;
                //else
                //    cred.AllowOfflineAccess = credential.AllowOfflineAccess;

                cred.AllowOfflineAccess = credential.AllowOfflineAccess;
                cred.ClientID = credential.ClientID;
                cred.RefreshTokenExpiration = credential.RefreshTokenExpiration;
                cred.RefreshTokenUsage = credential.RefreshTokenUsage;
                cred.AbsoluteRefreshTokenLifetime = credential.AbsoluteRefreshTokenLifetime;

                //byte[] passwordHash, passwordSalt;
                //AuthUserRepository.CreatePasswordHash(credential.ClientSecret, out passwordHash, out passwordSalt);
                //cred.ClientSecretHash = passwordHash;
                //cred.ClientSecretSalt = passwordSalt;
                cred.ClientSecretHash = Helper.EncryptDecrypt.EncryptString(credential.ClientSecret);
                _context.Database.SetCommandTimeout(30000);
                await _context.ClientCredential.AddAsync(cred);
                await _context.SaveChangesAsync();
                credential.Id = cred.Id;
                return credential;                
            }
            else
                return null;



            /*using (Microsoft.Data.SqlClient.SqlConnection cn = new Microsoft.Data.SqlClient.SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                await cn.OpenAsync();
               

                using (Microsoft.Data.SqlClient.SqlCommand command = new Microsoft.Data.SqlClient.SqlCommand())
                {
                    command.Connection = cn;
                    command.CommandTimeout = 300000;
                    command.CommandText = "INSERT INTO [dbo].[ClientCredential]([ClientID],[ClientSecretHash]" +
                                          ",[AllowedGrantTypes],[AllowOfflineAccess],[RefreshTokenUsage]" +
                                          ",[AccessTokenLifetime],[RefreshTokenExpiration],[AbsoluteRefreshTokenLifetime])" +
                                          "VALUES(@ClientID,@ClientSecretHash,@AllowedGrantTypes" +
                                          ",@AllowOfflineAccess,@RefreshTokenUsage,@AccessTokenLifeTime" +
                                          ",@RefreshTokenExpiration,@AbsoluteRefreshTokenLifetime);" +
                                          "SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY]; ";

                    //throw new Exception(command.CommandText);
                                          
                    command.Parameters.Add("@ClientID", System.Data.SqlDbType.NVarChar);
                    command.Parameters["@ClientID"].Value = credential.ClientID;

                   
                   
                    command.Parameters.Add("@ClientSecretHash", System.Data.SqlDbType.NVarChar);
                    string encryptedPassword = Helper.EncryptDecrypt.EncryptString(credential.ClientSecret);
                    
                    command.Parameters["@ClientSecretHash"].Value = encryptedPassword.Replace("'", "''");

                    command.Parameters.Add("@AllowedGrantTypes", System.Data.SqlDbType.NVarChar);
                    command.Parameters["@AllowedGrantTypes"].Value = credential.AllowedGrantTypes;
                    

                    command.Parameters.Add("@AllowOfflineAccess", System.Data.SqlDbType.Int);

                    if (credential.AllowOfflineAccess == null)
                        command.Parameters["@AllowOfflineAccess"].Value = DBNull.Value;
                    else
                        command.Parameters["@AllowOfflineAccess"].Value = credential.AllowOfflineAccess;


                    command.Parameters.Add("@RefreshTokenUsage", System.Data.SqlDbType.Int);
                    command.Parameters["@RefreshTokenUsage"].Value = credential.RefreshTokenUsage;

                    command.Parameters.Add("@AccessTokenLifeTime", System.Data.SqlDbType.Int);
                    command.Parameters["@AccessTokenLifeTime"].Value = credential.AccessTokenLifetime;


                    command.Parameters.Add("@RefreshTokenExpiration", System.Data.SqlDbType.Int);
                    command.Parameters["@RefreshTokenExpiration"].Value = credential.RefreshTokenExpiration;

                    command.Parameters.Add("@AbsoluteRefreshTokenLifetime", System.Data.SqlDbType.Int);
                    command.Parameters["@AbsoluteRefreshTokenLifetime"].Value = credential.AbsoluteRefreshTokenLifetime;                 
                                      
                    
                    try
                    {
                        var obj = await command.ExecuteNonQueryAsync();
                        //credential.Id = id;
                      
                    }
                    catch
                    {
                        throw;
                    }
                                       
                    return credential;

                   
                   

                }
            }*/


        }
        public async Task<IEnumerable<ClientCredentialDto>> GetAllClientCredentials()
        {            
            var credentialList = await _context.ClientCredential.ToListAsync();
            List<ClientCredentialDto> ccList = new List<ClientCredentialDto>();
            if (credentialList != null && credentialList.Count > 0)
            {
                foreach(var cc in credentialList)
                {
                    ClientCredentialDto cred = new ClientCredentialDto();
                    cred.AbsoluteRefreshTokenLifetime = cc.AbsoluteRefreshTokenLifetime;
                    cred.AccessTokenLifetime = cc.AccessTokenLifetime;
                    cred.AllowedGrantTypes = cc.AllowedGrantTypes;
                    cred.AllowOfflineAccess = cc.AllowOfflineAccess;
                    cred.ClientID = cc.ClientID;
                    cred.ClientSecret = Helper.EncryptDecrypt.DecryptString(cc.ClientSecretHash);
                    cred.RefreshTokenExpiration = cc.RefreshTokenExpiration;
                    cred.RefreshTokenUsage = cc.RefreshTokenUsage;
                    cred.AbsoluteRefreshTokenLifetime = cc.AbsoluteRefreshTokenLifetime;
                    cred.Id = cc.Id;
                    ccList.Add(cred);
                }
            }
            return ccList;
        }

        public async Task<bool> ClientIdExists(string clientID)
        {
            var clientCred = await _context.ClientCredential.FirstOrDefaultAsync(x => x.ClientID == clientID);
            if (clientCred == null)
                return false;
            else
                return true;
        }

    }
}
