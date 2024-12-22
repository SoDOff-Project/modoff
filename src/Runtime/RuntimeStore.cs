using System.Collections.Generic;
using modoff.Model;
using modoff.Controllers;
using modoff.Services;

namespace modoff.Runtime {
    public static class RuntimeStore {
        public static DBContext ctx;
        public static Dispatcher dispatcher;

        // FIXME: Bad idea, but will do for now
        private static AchievementService achievementService;
        private static AchievementStoreSingleton achievementStoreSingleton;
        private static DisplayNamesService displayNamesService;
        private static GameDataService gameDataService;
        private static InventoryService inventoryService;
        private static ItemService itemService;
        private static KeyValueService keyValueService;
        private static MissionService missionService;
        private static MissionStoreSingleton missionStoreSingleton;
        private static NeighborhoodService neighborhoodService;
        private static ProfileService profileService;
        private static RoomService roomService;
        private static StoreService storeService;

        public static void Init() {
            ctx = new DBContext();
            ctx.Database.EnsureCreated();

            itemService = new ItemService();
            achievementStoreSingleton = new AchievementStoreSingleton();
            achievementService = new AchievementService(achievementStoreSingleton, inventoryService, ctx);
            displayNamesService = new DisplayNamesService(itemService);
            gameDataService = new GameDataService(ctx);
            inventoryService = new InventoryService(ctx, itemService);
            keyValueService = new KeyValueService(ctx);
            missionStoreSingleton = new MissionStoreSingleton();
            missionService = new MissionService(ctx, missionStoreSingleton, achievementService);
            neighborhoodService = new NeighborhoodService(ctx);
            profileService = new ProfileService(ctx);
            roomService = new RoomService(ctx, itemService, achievementService);
            storeService = new StoreService(itemService);

            var controllers = new List<Controller> {
                new AuthenticationController(ctx),
                new MembershipController(ctx),
                new ProfileController(ctx, achievementService, profileService),
                new RegistrationController(ctx, missionService, roomService, keyValueService)
            };

            dispatcher = new Dispatcher(controllers);
        }
    }
}
