namespace Boom
{
    using Boom.Utility;
    using Boom.Values;
    using Candid;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public static class ConfigUtil
    {
        //TRY GET CONFGIS
        public static bool TryGetConfig(string worldId, string configId, out MainDataTypes.AllConfigs.Config outValue)
        {
            outValue = default;

            var result = UserUtil.GetMainData<MainDataTypes.AllConfigs>();


            if (result.IsErr)
            {
                result.AsErr().Error();
                return false;
            }

            var allConfigs = result.AsOk();

            if (allConfigs.configs.TryGetValue(worldId, out var worldConfigs) == false)
            {
                $"Could not find configs from world of id: {worldId}".Warning();
                return false;
            }

            if (worldConfigs.TryGetValue(configId, out outValue) == false)
            {
                $"Could not find config of id: {configId} in world of id: {worldId}".Warning();

                return false;
            }

            return result.IsOk;
        }
        public static bool TryGetConfig(string worldId, Predicate<MainDataTypes.AllConfigs.Config> predicate, out MainDataTypes.AllConfigs.Config outValue)
        {
            outValue = default;

            var result = UserUtil.GetMainData<MainDataTypes.AllConfigs>();


            if (result.IsErr)
            {
                Debug.LogError(result.AsErr());
                return false;
            }

            var allConfigs = result.AsOk();

            if (allConfigs.configs.TryGetValue(worldId, out var worldConfigs) == false)
            {
                Debug.LogError($"Could not find configs from world of id: {worldId}");
                return false;
            }


            foreach (var item in worldConfigs)
            {
                if (predicate(item.Value))
                {
                    outValue = item.Value;
                    return true;
                }
            }

            return false;
        }

        public static bool QueryConfigs(string worldId, Predicate<MainDataTypes.AllConfigs.Config> predicate, out LinkedList<MainDataTypes.AllConfigs.Config> outValue)
        {
            outValue = default;

            var configsResult = UserUtil.GetMainData<MainDataTypes.AllConfigs>();


            if (configsResult.IsErr)
            {
                Debug.LogError(configsResult.AsErr());
                return false;
            }

            var allConfigs = configsResult.AsOk();

            if (allConfigs.configs.TryGetValue(worldId, out var worldConfigs) == false)
            {
                Debug.LogError($"Could not find configs from world of id: {worldId}");
                return false;
            }

            outValue = new();

            foreach (var item in worldConfigs)
            {
                if (predicate(item.Value)) outValue.AddLast(item.Value);
            }

            return true;
        }
        public static bool QueryConfigsByTag(string worldId, string tag, out LinkedList<MainDataTypes.AllConfigs.Config> outValue)
        {
            return QueryConfigs(worldId, e =>
            {
                if (!e.fields.TryGetValue("tag", out var value)) return false;

                return value.Contains(tag);
            }, out outValue);
        }

        //

        public static bool TryGetConfig(this EntityOutcome entity, string worldId, out MainDataTypes.AllConfigs.Config entityConfig)
        {
            return TryGetConfig(worldId, entity.eid, out entityConfig);
        }
        public static bool TryGetConfig(this DataTypes.Entity entity, string worldId, out MainDataTypes.AllConfigs.Config entityConfig)
        {
            return TryGetConfig(worldId, entity.eid, out entityConfig);
        }
        public static bool TryGetConfig(this EntityConstrainTypes.Base entity, string worldId, out MainDataTypes.AllConfigs.Config entityConfig)
        {
            return TryGetConfig(worldId, entity.Eid, out entityConfig);
        }

        public static bool TryGetConfigFieldAs<T>(string worldId, string configId, string fieldName, out T outValue, T defaultValue = default)
        {
            outValue = defaultValue;

            if (!TryGetConfig(worldId, configId, out var config))
            {
                return false;
            }

            if (!config.fields.TryGetValue(fieldName, out var value))
            {
                $"Failure to find in config of id: {configId} a field of name {fieldName}".Warning();

                return false;
            }

            if (value.TryParseValue<T>(out outValue) == false)
            {
                $"Failure to parse config field of id: {configId} to {typeof(T).Name}".Warning();

                return false;
            }

            return true;
        }
        public static bool TryGetConfigFieldAs<T>(this DataTypes.Entity entity, string fieldName, out T outValue, T defaultValue = default)
        {
            return TryGetConfigFieldAs(entity.wid, entity.eid, fieldName, out outValue, defaultValue);
        }
        public static bool TryGetConfigFieldAs<T>(this EntityOutcome newEntityValues, string worldId, string fieldName, out T outValue, T defaultValue = default)
        {
            return TryGetConfigFieldAs(worldId, newEntityValues.eid, fieldName, out outValue, defaultValue);
        }
        public static bool TryGetConfigFieldAs<T>(this MainDataTypes.AllConfigs.Config config, string fieldName, out T outValue, T defaultValue = default)
        {
            outValue = defaultValue;
            if (!config.fields.TryGetValue(fieldName, out var value)) return false;

            if (value.TryParseValue<T>(out outValue))
            {
                return true;
            }
            return false;
        }

        //ACTIONS
        public static bool TryGetAction(string worldId, string actionId, out MainDataTypes.AllAction.Action outValue)
        {
            outValue = default;

            var result = UserUtil.GetMainData<MainDataTypes.AllAction>();


            if (result.IsErr)
            {
                result.AsErr().Error();
                return false;
            }

            var allConfigs = result.AsOk();

            if (allConfigs.actions.TryGetValue(worldId, out var worldActions) == false)
            {
                $"Could not find configs from world of id: {worldId}".Warning();
                return false;
            }

            if (worldActions.TryGetValue(actionId, out outValue) == false)
            {
                $"Could not find config of id: {actionId} in world of id: {worldId}".Warning();
                return false;
            }

            return true;
        }
        public static bool TryGetActionPart<T>(this string actionId, Func<MainDataTypes.AllAction.Action, T> func, out T outValue)
        {
            outValue = default;

            if (!ConfigUtil.TryGetAction(BoomManager.Instance.WORLD_CANISTER_ID, actionId, out var action))
            {
                Debug.LogError("Could not find action of id: " + actionId);

                return false;
            }


            try
            {
                outValue = func(action);
            }
            catch
            {
                return false;
            }


            return true;
        }

        //TOKENS
        public static UResult<LinkedList<MainDataTypes.AllTokenConfigs.TokenConfig>, string> GetAllTokenConfigs()
        {
            var result = UserUtil.GetMainData<MainDataTypes.AllTokenConfigs>();

            if (result.IsErr)
            {
                return new(result.AsErr());
            }

            var allConfigs = result.AsOk();

            LinkedList<MainDataTypes.AllTokenConfigs.TokenConfig> configs = new();

            foreach (var tokenConfig in allConfigs.configs)
            {
                configs.AddLast(tokenConfig.Value);
            }

            return new(configs);
        }
        public static bool TryGetTokenConfig(string canisterId, out MainDataTypes.AllTokenConfigs.TokenConfig outValue)
        {
            outValue = default;

            var result = UserUtil.GetMainData<MainDataTypes.AllTokenConfigs>();

            if (result.IsErr)
            {
                Debug.LogError(result.AsErr());
                return false;
            }

            var allConfigs = result.AsOk();

            if (allConfigs.configs.TryGetValue(canisterId, out outValue) == false)
            {
                Debug.LogError($"Could not find token configs for canister id: {canisterId}");
                return false;
            }

            return true;
        }
        public static bool TryGetTokenConfig(this DataTypes.Token token, out MainDataTypes.AllTokenConfigs.TokenConfig outValue)
        {
            return TryGetTokenConfig(token.canisterId, out outValue);
        }
        //NFTS
        public static UResult<LinkedList<MainDataTypes.AllNftCollectionConfig.NftConfig>, string> GetAllNftConfigs()
        {
            var result = UserUtil.GetMainData<MainDataTypes.AllNftCollectionConfig>();

            if (result.IsErr)
            {
                return new(result.AsErr());
            }

            var allConfigs = result.AsOk();

            LinkedList<MainDataTypes.AllNftCollectionConfig.NftConfig> configs = new();

            foreach (var tokenConfig in allConfigs.configs)
            {
                configs.AddLast(tokenConfig.Value);
            }

            return new(configs);
        }

        public static bool TryGetNftCollectionConfig(string canisterId, out MainDataTypes.AllNftCollectionConfig.NftConfig outValue)
        {
            outValue = default;

            var result = UserUtil.GetMainData<MainDataTypes.AllNftCollectionConfig>();

            if (result.IsErr)
            {
                Debug.LogError(result.AsErr());
                return false;
            }

            var allConfigs = result.AsOk();

            if (allConfigs.configs.TryGetValue(canisterId, out outValue) == false)
            {
                Debug.LogError($"Could not find token configs for canister id: {canisterId}");
                return false;
            }

            return true;
        }

        public static bool TryGetNftCollectionConfig(this DataTypes.NftCollection collection, out MainDataTypes.AllNftCollectionConfig.NftConfig outValue)
        {
            return TryGetNftCollectionConfig(collection.canisterId, out outValue);
        }

    }
}