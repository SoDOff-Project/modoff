using System;
using System.Linq;
using System.Collections.Generic;
using modoff.Model;

namespace modoff.Services;
public class KeyValueService {

    private readonly DBContext ctx;

    public KeyValueService(DBContext ctx) {
        this.ctx = ctx;
    }

    public modoff.Model.PairData? GetPairData(User? user, Viking? viking, string? userId, int pairId) {
        Dragon? dragon = null;
        return CheckOwnerAndGetPairData(ref user, ref viking, ref dragon, userId, pairId);
    }

    public bool SetPairData(User? user, Viking? viking, string? userId, int pairId, PairData schemaData) {
        // Get the pair
        Dragon? dragon = null;
        modoff.Model.PairData? pair = CheckOwnerAndGetPairData(ref user, ref viking, ref dragon, userId, pairId);

        // Create the pair if it doesn't exist
        bool exists = true;
        if (pair is null) {
            pair = new modoff.Model.PairData {
                PairId = pairId,
                UserId = (Guid)(user?.Id),
                VikingId = viking?.Id,
                DragonId = dragon?.Id,
                Pairs = new List<modoff.Model.Pair>()
            };
            exists = false;
        }
        if (schemaData is null || schemaData.Pairs is null)
            return true;
        // Update or create the key-value pairs
        foreach (var p in schemaData.Pairs) {
            if (string.IsNullOrEmpty(p.PairValue))
                return false; // NOTE: The real server doesn't update anything if one value is empty
            var pairItem = pair.Pairs.FirstOrDefault(e => e.Key == p.PairKey);
            if (pairItem != null)
                pairItem.Value = p.PairValue;
            else {
                pairItem = new modoff.Model.Pair {
                    Key = p.PairKey,
                    Value = p.PairValue
                };
                pair.Pairs.Add(pairItem);
            }
        }

        if (exists)
            ctx.PairData.Update(pair);
        else
            ctx.PairData.Add(pair);
        ctx.SaveChanges();

        return true;
    }

    public PairData? ModelToSchema(modoff.Model.PairData? model) {
        if (model is null) return null;

        // Convert to schema class
        List<Pair> pairs = new List<Pair>();
        foreach (var p in model.Pairs) {
            pairs.Add(new Pair {
                PairKey = p.Key,
                PairValue = p.Value
            });
        }

        return new PairData { Pairs = pairs.ToArray() };
    } 

    private modoff.Model.PairData? CheckOwnerAndGetPairData(ref User? user, ref Viking? viking, ref Dragon dragon, string? userId, int pairId) {
        if (userId != null) {
            Guid userIdGuid = Guid.Parse(userId);
            // find dragon
            dragon = viking?.Dragons.FirstOrDefault(e => e.EntityId == userIdGuid);

            // if not dragon then try viking -> check ID
            if (dragon != null || viking?.Uid != userIdGuid) {
                // if not viking and user not set, then try set user from viking
                if (user is null)
                    user = viking?.User;
                viking = null;
            }

            // if not dragon nor viking then try user -> check ID
            if (viking != null || user?.Id != userIdGuid) user = null;
        }

        // NOTE: only one of (dragon, viking, user) can be not null here

        modoff.Model.PairData? pair = null;
        if (viking != null)
            pair = viking.PairData?.FirstOrDefault(e => e.PairId == pairId);
        else if (user != null)
            pair = user.PairData.FirstOrDefault(e => e.PairId == pairId);
        else if (dragon != null)
            pair = dragon.PairData.FirstOrDefault(e => e.PairId == pairId);

        return pair;
    }
}
