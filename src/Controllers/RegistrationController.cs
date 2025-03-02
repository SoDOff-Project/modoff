﻿using modoff.Attributes;
using modoff.Model;
using modoff.Util;
using System.Collections.Generic;
using System.Linq;
using System;
using modoff.Runtime;
using modoff.Services;

namespace modoff.Controllers {

    public class RegistrationController : Controller {

        private readonly DBContext ctx;
        private MissionService missionService;
        private RoomService roomService;
        private KeyValueService keyValueService;

        public RegistrationController(DBContext ctx, MissionService missionService, RoomService roomService, KeyValueService keyValueService) {
            this.ctx = ctx;
            this.missionService = missionService;
            this.roomService = roomService;
            this.keyValueService = keyValueService;
        }

        [Route("v3/RegistrationWebService.asmx/DeleteProfile")]
        public IActionResult DeleteProfile(Guid apiToken, Guid userID) {
            User user = ctx.Sessions.FirstOrDefault(e => e.ApiToken == apiToken)?.User;
            if (user is null) {
                return Ok(DeleteProfileStatus.OWNER_ID_NOT_FOUND);
            }

            Viking viking = ctx.Vikings.FirstOrDefault(e => e.Uid == userID);
            if (viking is null) {
                return Ok(DeleteProfileStatus.PROFILE_NOT_FOUND);
            }

            if (user != viking.User) {
                return Ok(DeleteProfileStatus.PROFILE_NOT_OWNED_BY_THIS_OWNER);
            }

            ctx.Vikings.Remove(viking);
            ctx.SaveChanges();

            return Ok(DeleteProfileStatus.SUCCESS);
        }

        [Route("v3/RegistrationWebService.asmx/RegisterParent")]
        [DecryptRequest("parentRegistrationData")]
        [EncryptResponse]
        public IActionResult RegisterParent( string apiKey, string parentRegistrationData) {
            ParentRegistrationData data = XmlUtil.DeserializeXml<ParentRegistrationData>(parentRegistrationData);
            User u = new User {
                Id = Guid.NewGuid(),
                Username = data.ChildList[0].ChildName,
                Password = PasswordHasher.HashPassword(data.Password),
                Email = data.Email
            };

            // Check if user exists
            uint gameVersion = ClientVersion.GetVersion(apiKey);
            if (gameVersion <= ClientVersion.Max_OldJS) {
                if (ctx.Users.Count(e => e.Email == u.Email) > 0) {
                    return Ok(new RegistrationResult { Status = MembershipUserStatus.DuplicateEmail });
                }
            }
            if (ctx.Users.Count(e => e.Username == u.Username) > 0) {
                return Ok(new RegistrationResult { Status = MembershipUserStatus.DuplicateUserName });
            }

            ctx.Users.Add(u);
            ctx.SaveChanges();

            if (gameVersion <= ClientVersion.Max_OldJS) {
                CreateViking(u, data.ChildList[0], gameVersion);
            }

            ParentLoginInfo pli = new ParentLoginInfo {
                UserName = u.Username,
                ApiToken = Guid.NewGuid().ToString(),
                UserID = u.Id.ToString(),
                Status = MembershipUserStatus.Success,
                UnAuthorized = false
            };

            var response = new RegistrationResult {
                ParentLoginInfo = XmlUtil.SerializeXml<ParentLoginInfo>(pli),
                UserID = u.Id.ToString(),
                Status = MembershipUserStatus.Success,
                ApiToken = Guid.NewGuid().ToString()
            };

            return Ok(response);
        }

        [Route("V3/RegistrationWebService.asmx/RegisterChild")] // used by Magic & Mythies
        [Route("V4/RegistrationWebService.asmx/RegisterChild")]
        [DecryptRequest("childRegistrationData")]
        [EncryptResponse]
        public IActionResult RegisterChild(/*[FromForm]*/ Guid parentApiToken, string apiKey, string childRegistrationData) {
            User user = ctx.Sessions.FirstOrDefault(e => e.ApiToken == parentApiToken)?.User;
            if (user is null) {
                return Ok(new RegistrationResult {
                    Status = MembershipUserStatus.InvalidApiToken
                });
            }

            // Check if name populated
            ChildRegistrationData data = XmlUtil.DeserializeXml<ChildRegistrationData>(childRegistrationData);
            if (String.IsNullOrWhiteSpace(data.ChildName)) {
                return Ok(new RegistrationResult { Status = MembershipUserStatus.ValidationError });
            }

            // Check if viking exists
            if (ctx.Vikings.Count(e => e.Name == data.ChildName) > 0) {
                return Ok(new RegistrationResult { Status = MembershipUserStatus.DuplicateUserName });
            }

            Viking v = CreateViking(user, data, ClientVersion.GetVersion(apiKey));

            return Ok(new RegistrationResult {
                UserID = v.Uid.ToString(),
                Status = MembershipUserStatus.Success
            });
        }

        private Viking CreateViking(User user, ChildRegistrationData data, uint gameVersion) {
            List<InventoryItem> items = new List<InventoryItem>();
            if (gameVersion >= ClientVersion.Min_SoD) {
                items.Add(new InventoryItem { ItemId = 8977, Quantity = 1 }); // DragonStableINTDO - Dragons Dragon Stable
            }

            Viking v = new Viking {
                Uid = Guid.NewGuid(),
                Name = data.ChildName,
                User = user,
                InventoryItems = items,
                AchievementPoints = new List<AchievementPoints>(),
                Rooms = new List<Room>(),
                CreationDate = DateTime.UtcNow,
                BirthDate = data.BirthDate
            };

            missionService.SetUpMissions(v, gameVersion);

            if (data.Gender == "Boy") v.Gender = Gender.Male;
            else if (data.Gender == "Girl") v.Gender = Gender.Female;

            ctx.Vikings.Add(v);
            ctx.SaveChanges();

            if (gameVersion >= ClientVersion.MaM && gameVersion < 0xa2a09a0a) {
                keyValueService.SetPairData(null, v, null, 2017, new PairData {
                    Pairs = new Pair[]{
                    new Pair {
                        // avoid showing change viking name dialog
                        PairKey = "AvatarNameCustomizationDone",
                        PairValue = "1"
                    },
                }
                });
            }

            roomService.CreateRoom(v, "MyRoomINT");

            return v;
        }
    }
}