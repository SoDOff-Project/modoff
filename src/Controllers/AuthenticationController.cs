using Microsoft.Extensions.Options;
using modoff.Attributes;
using modoff.Model;
using modoff.Runtime;
using modoff.Schema;
using modoff.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Networking.Match;

namespace modoff.Controllers {
    public class AuthenticationController : Controller {

        private readonly DBContext ctx;

        public AuthenticationController(DBContext ctx) {
            this.ctx = ctx;
        }

        [Route("v3/AuthenticationWebService.asmx/GetRules")]
        [EncryptResponse]
        public IActionResult GetRules() {
            GetProductRulesResponse response = new GetProductRulesResponse {
                GlobalSecretKey = "11A0CC5A-C4DF-4A0E-931C-09A44C9966AE"
            };

            return Ok(response);
        }

        [Route("v3/AuthenticationWebService.asmx/LoginParent")]
        [DecryptRequest("parentLoginData")]
        [EncryptResponse]
        public IActionResult LoginParent(string apiKey, string parentLoginData) {
            ParentLoginData data = XmlUtil.DeserializeXml<ParentLoginData>(parentLoginData);

            // Authenticate the user
            User? user = null;
            uint gameVersion = ClientVersion.GetVersion(apiKey);
            if (gameVersion <= ClientVersion.Max_OldJS) {
                user = ctx.Users.FirstOrDefault(e => e.Email == data.UserName);
            } else {
                user = ctx.Users.FirstOrDefault(e => e.Username == data.UserName);
            }

            if (user is null || !PasswordHasher.VerifyPassword(data.Password, user.Password)) {
                return Ok(new ParentLoginInfo { Status = MembershipUserStatus.InvalidPassword });
            }

            // Create session
            Session session = new Session {
                User = user,
                ApiToken = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow
            };

            ctx.Sessions.Add(session);
            ctx.SaveChanges();

            var childList = new List<UserLoginInfo>();
            if (user.Vikings != null)
                foreach (var viking in user.Vikings) {
                    childList.Add(new UserLoginInfo { UserName = viking.Name, UserID = viking.Uid.ToString() });
                }

            var response = new ParentLoginInfo {
                UserName = user.Username,
                //Email = user.Email, /* disabled to avoid put email in client debug logs */
                ApiToken = session.ApiToken.ToString(),
                UserID = user.Id.ToString(),
                Status = MembershipUserStatus.Success,
                SendActivationReminder = false,
                UnAuthorized = false,
                ChildList = childList.ToArray()
            };

            return Ok(response);
        }

        [Route("v3/AuthenticationWebService.asmx/AuthenticateUser")]
        [DecryptRequest("username")]
        [DecryptRequest("password")]
        public bool AuthenticateUser(string username, string password) {

            // Authenticate the user
            User? user = ctx.Users.FirstOrDefault(e => e.Username == username);
            if (user is null || !PasswordHasher.VerifyPassword(password, user.Password)) {
                return false;
            }

            return true;
        }

        [Route("AuthenticationWebService.asmx/GetUserInfoByApiToken")]
        public IActionResult GetUserInfoByApiToken(Guid apiToken, string apiKey) {
            // First check if this is a user session
            User? user = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.User;
            if (user is not null) {
                return Ok(new UserInfo {
                    UserID = user.Id.ToString(),
                    Username = user.Username,
                    MembershipID = "ef84db9-59c6-4950-b8ea-bbc1521f899b", // placeholder
                    FacebookUserID = 0,
                    MultiplayerEnabled = false,
                    IsApproved = true,
                    Age = 24,
                    OpenChatEnabled = true
                });
            }

            // Then check if this is a viking session
            Viking? viking = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.Viking;
            if (viking is not null) {
                return Ok(new UserInfo {
                    UserID = viking.Uid.ToString(),
                    Username = viking.Name,
                    FacebookUserID = 0,
                    MultiplayerEnabled = false,
                    IsApproved = true,
                    Age = 24,
                    OpenChatEnabled = true
                });
            }

            // Otherwise, this is a bad session, return empty UserInfo
            return Ok(new UserInfo { });
        }

        [Route("AuthenticationWebService.asmx/IsValidApiToken")] // used by World Of Jumpstart (FutureLand)
        public IActionResult IsValidApiToken_V1(Guid apiToken) {
            if (apiToken == null)
                return Ok(false);
            User? user = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.User;
            Viking? viking = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.Viking;
            if (user is null && viking is null)
                return Ok(false);
            return Ok(true);
        }

        [Route("AuthenticationWebService.asmx/IsValidApiToken_V2")]
        public IActionResult IsValidApiToken(Guid apiToken) {
            if (apiToken == null)
                return Ok(ApiTokenStatus.TokenNotFound);
            User? user = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.User;
            Viking? viking = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.Viking;
            if (user is null && viking is null)
                return Ok(ApiTokenStatus.TokenNotFound);
            return Ok(ApiTokenStatus.TokenValid);
        }

        // This is more of a "create session for viking", rather than "login child"
        [PlainText]
        [Route("AuthenticationWebService.asmx/LoginChild")]
        [DecryptRequest("childUserID")]
        [EncryptResponse]
        public IActionResult LoginChild(Guid parentApiToken, string? childUserID) {
            User? user = ctx.Sessions.FirstOrDefault(e => e.ApiToken == parentApiToken)?.User;
            if (user is null) {
                return Ok(""); // FIXME
            }

            // Find the viking
            Viking? viking = ctx.Vikings.FirstOrDefault(e => e.Uid == Guid.Parse(childUserID));
            if (viking is null) {
                return Ok(""); // FIXME
            }

            // Check if user is viking parent
            if (user != viking.User) {
                return Ok(""); // FIXME
            }

            // Create session
            Session session = new Session {
                Viking = viking,
                ApiToken = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow
            };
            ctx.Sessions.Add(session);
            ctx.SaveChanges();

            // Return back the api token
            return Ok(session.ApiToken.ToString());
        }

        [Route("AuthenticationWebService.asmx/DeleteAccountNotification")]
        public IActionResult DeleteAccountNotification(Guid apiToken) {
            User? user = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.User;
            if (user is null)
                return Ok(MembershipUserStatus.ValidationError);

            ctx.Users.Remove(user);
            ctx.SaveChanges();

            return Ok(MembershipUserStatus.Success);
        }

        [Route("Authentication/MMOAuthentication")]
        public IActionResult MMOAuthentication(Guid token) {
            AuthenticationInfo info = new();
            info.Authenticated = false;
            var session = ctx.Sessions.FirstOrDefault(x => x.ApiToken == token);
            if (session != null) {
                info.Authenticated = true;
                info.DisplayName = session.Viking.Name;
            }
            return Ok(info);
        }
    }
}
