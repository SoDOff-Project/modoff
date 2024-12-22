using Microsoft.Extensions.Options;
using modoff.Model;
using modoff.Runtime;
using modoff.Services;
using modoff.Schema;
using modoff.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using modoff.Attributes;
using Microsoft.EntityFrameworkCore.Metadata;

namespace modoff.Controllers {
    public class ProfileController : Controller {

        private readonly DBContext ctx;
        private AchievementService achievementService;
        private ProfileService profileService;

        public ProfileController(DBContext ctx, AchievementService achievementService, ProfileService profileService) {
            this.ctx = ctx;
            this.achievementService = achievementService;
            this.profileService = profileService;
        }

        [Route("ProfileWebService.asmx/GetUserProfileByUserID")]
        public IActionResult GetUserProfileByUserID(Guid userId, string apiKey) {
            // NOTE: this is public info (for mmo) - no session check

            Viking? viking = ctx.Vikings.FirstOrDefault(e => e.Uid == userId);
            if (viking is null) {
                return Ok(new UserProfileData());
                // NOTE: do not return `Conflict("Viking not found")` due to client side error handling
                //       (not Ok response cause soft-lock client - can't close error message)
            }

            return Ok(GetProfileDataFromViking(viking, apiKey));
        }

        [Route("ProfileWebService.asmx/GetUserProfile")]
        [VikingSession(UseLock = false)]
        public IActionResult GetUserProfile(Viking viking, string apiKey) {
            return Ok(GetProfileDataFromViking(viking, apiKey));
        }

        [Route("ProfileWebService.asmx/GetDetailedChildList")]
        [VikingSession(Mode = VikingSession.Modes.USER, ApiToken = "parentApiToken", UseLock = false)]
        public IActionResult GetDetailedChildList(User user, string apiKey) {
            if (user.Vikings.Count <= 0)
                return Ok(""); // FIXME

            UserProfileData[] profiles = user.Vikings.Select(v => GetProfileDataFromViking(v, apiKey)).ToArray();
            return Ok(new UserProfileDataList {
                UserProfiles = profiles
            });
        }

        [PlainText]
        [Route("ProfileWebService.asmx/GetQuestions")]
        public IActionResult GetQuestions() {
            return Ok(XmlUtil.ReadResourceXmlString("questiondata"));
        }

        [Route("ProfileWebService.asmx/SetUserProfileAnswers")]
        [VikingSession]
        public IActionResult SetUserProfileAnswers(Viking viking, int profileAnswerIDs) {
            if (viking is null)
                return Ok("");
            ProfileQuestion questionFromaId = profileService.GetQuestionFromAnswerId(profileAnswerIDs);
            return Ok(profileService.SetAnswer(viking, questionFromaId.ID, profileAnswerIDs));
        }

        [PlainText]
        [Route("ProfileWebService.asmx/GetProfileTagAll")] // used by Magic & Mythies
        public IActionResult GetProfileTagAll() {
            return Ok(XmlUtil.ReadResourceXmlString("profiletags"));
        }

        private UserProfileData GetProfileDataFromViking(Viking viking, string apiKey) {
            // Get the avatar data
            AvatarData avatarData = null;
            Gender? gender = null;
            if (viking.AvatarSerialized is not null) {
                avatarData = XmlUtil.DeserializeXml<AvatarData>(viking.AvatarSerialized);
                avatarData.Id = viking.Id;
                if (gender is null)
                    gender = avatarData.GenderType;
            }
            if (gender is null)
                gender = Gender.Unknown;

            if (avatarData != null && ClientVersion.GetVersion(apiKey) == 0xa3a12a0a) { // TODO adjust version number: we don't know for which versions it is required (for 3.12 it is, for 3.19 and 3.0 it's not)
                if (avatarData.Part.FirstOrDefault(e => e.PartType == "Sword") is null) {
                    var extraParts = new AvatarDataPart[] {
                    new AvatarDataPart {
                        PartType = "Sword",
                        Geometries = new string[] {"NULL"},
                        Textures = new string[] {"__EMPTY__"},
                        UserInventoryId = null,
                    }
                };
                    avatarData.Part = extraParts.Concat(avatarData.Part).ToArray();
                }
            }

            // Build the AvatarDisplayData
            AvatarDisplayData avatar = new AvatarDisplayData {
                AvatarData = avatarData,
                UserInfo = new UserInfo {
                    MembershipID = "ef84db9-59c6-4950-b8ea-bbc1521f899b", // placeholder
                    UserID = viking.Uid.ToString(),
                    ParentUserID = viking.UserId.ToString(),
                    Username = viking.Name,
                    FirstName = viking.Name,
                    MultiplayerEnabled = false,
                    Locale = "en-US", // placeholder
                    GenderID = gender,
                    OpenChatEnabled = true,
                    IsApproved = true,
                    RegistrationDate = viking.CreationDate,
                    CreationDate = viking.CreationDate,
                    FacebookUserID = 0,
                    BirthDate = viking.BirthDate
                },
                UserSubscriptionInfo = new UserSubscriptionInfo {
                    UserID = viking.UserId.ToString(),
                    MembershipID = 130687131, // placeholder
                    SubscriptionTypeID = 1, // placeholder
                    SubscriptionDisplayName = "Member", // placeholder
                    SubscriptionPlanID = 41, // placeholder
                    SubscriptionID = -3, // placeholder
                    IsActive = true, // placeholder
                },
                RankID = 0, // placeholder
                AchievementInfo = null, // placeholder
                Achievements = new UserAchievementInfo[] {
                achievementService.CreateUserAchievementInfo(viking, AchievementPointTypes.PlayerXP),
                achievementService.CreateUserAchievementInfo(viking, AchievementPointTypes.PlayerFarmingXP),
                achievementService.CreateUserAchievementInfo(viking, AchievementPointTypes.PlayerFishingXP),
                achievementService.CreateUserAchievementInfo(viking, AchievementPointTypes.UDTPoints),
            }
            };

            UserGameCurrency currency = achievementService.GetUserCurrency(viking);

            return new UserProfileData {
                ID = viking.Uid.ToString(),
                AvatarInfo = avatar,
                AchievementCount = 0,
                MythieCount = 0,
                AnswerData = new UserAnswerData { UserID = viking.Uid.ToString(), Answers = profileService.GetUserAnswers(viking) },
                GameCurrency = currency.GameCurrency,
                CashCurrency = currency.CashCurrency,
                ActivityCount = 0,
                BuddyCount = 0,
                UserGradeData = new UserGrade { UserGradeID = 0 },
                UserProfileTag = new UserProfileTag() {
                    CreateDate = new DateTime(DateTime.Now.Ticks),
                    ProductGroupID = 1,
                    ProfileTags = new List<ProfileTag>(),
                    UserID = viking.Uid,
                    UserProfileTagID = 1
                }
            };
        }
    }
}
