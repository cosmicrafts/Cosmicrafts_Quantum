namespace Boom
{
    using Boom.Utility;
    using Boom.Values;
    using Candid;
    using Candid.World.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Unity.VisualScripting;
    using UnityEngine;
    using static Boom.MainDataTypes.AllAction;

    // Ignore Spelling: Util

    public static class EntityFilterPredicate
    {
        public class Base { }

        public class Text : Base
        {
            public Predicate<string> Predicate { get; private set; }

            public Text(Predicate<string> predicate)
            {
                Predicate = predicate;
            }
        }
        public class Double : Base
        {
            public Predicate<double> Predicate { get; private set; }

            public Double(Predicate<double> predicate)
            {
                Predicate = predicate;
            }
        }
        public class ULong : Base
        {
            public Predicate<ulong> Predicate { get; private set; }

            public ULong(Predicate<ulong> predicate)
            {
                Predicate = predicate;
            }
        }
        public class Bool : Base
        {
            public Predicate<bool> Predicate { get; private set; }

            public Bool(Predicate<bool> predicate)
            {
                Predicate = predicate;
            }
        }
    }
    public static class EntityFilter
    {
        public abstract class Base
        {

            public KeyValue<string, EntityFilterPredicate.Base>[] fieldsConditions;

            protected Base(KeyValue<string, EntityFilterPredicate.Base>[] fieldsConditions)
            {
                this.fieldsConditions = fieldsConditions;
            }
        }

        public class FromUser : Base
        {
            public string PrincipalId { get; private set; }

            public FromUser(string principal, params KeyValue<string, EntityFilterPredicate.Base>[] fieldCondition) : base(fieldCondition)
            {
                this.PrincipalId = principal;
            }
        }

        public class FromWorld : Base
        {
            public string PrincipalId { get { return BoomManager.Instance.WORLD_CANISTER_ID; } }

            public FromWorld(params KeyValue<string, EntityFilterPredicate.Base>[] fieldCondition) : base(fieldCondition)
            {
            }
        }

        public class FromSelf : Base
        {
            public FromSelf(params KeyValue<string, EntityFilterPredicate.Base>[] fieldCondition) : base(fieldCondition)
            {
            }
        }
    }

    internal static class EntityUtil
    {

        public static class Queries
        {
            public static EntityFilter.FromWorld rooms = new EntityFilter.FromWorld(
                            new KeyValue<string, EntityFilterPredicate.Base>("tag", new EntityFilterPredicate.Text(e => e == "room")),
                            new KeyValue<string, EntityFilterPredicate.Base>("userCount", new EntityFilterPredicate.Double(e => e != 0.0)));

            public static EntityFilter.FromSelf ownItems = new EntityFilter.FromSelf(new KeyValue<string, EntityFilterPredicate.Base>("tag", new EntityFilterPredicate.Text(e => e == "item")));
        }

        //Multiple output
        public static bool TryGetEntities(out LinkedList<DataTypes.Entity> entities, string userPrincipalId = "self", string sourceWorldId = default)
        {
            if (string.IsNullOrEmpty(sourceWorldId)) sourceWorldId = BoomManager.Instance.WORLD_CANISTER_ID;

            entities = default;
            var elementsResult = UserUtil.GetElementsOfType<DataTypes.Entity>(userPrincipalId);

            if (elementsResult.IsErr) return false;

            entities = new();

            var elements = elementsResult.AsOk();
            foreach (var e in elements)
            {
                if (sourceWorldId == e.wid)
                    entities.AddLast(e);
            }

            return true;
        }

        public static bool TryQueryEntities(EntityFilter.Base entityFilter, out LinkedList<DataTypes.Entity> entities, string sourceWorldId = default)
        {
            if (string.IsNullOrEmpty(sourceWorldId)) sourceWorldId = BoomManager.Instance.WORLD_CANISTER_ID;

            string principalId = "";
            KeyValue<string, EntityFilterPredicate.Base>[] conditions = null;

            switch (entityFilter)
            {
                case EntityFilter.FromUser e:
                    principalId = e.PrincipalId;
                    conditions = e.fieldsConditions;
                    break;
                case EntityFilter.FromWorld e:
                    principalId = e.PrincipalId;
                    conditions = e.fieldsConditions;
                    break;
                case EntityFilter.FromSelf e:
                    principalId = "self";
                    conditions = e.fieldsConditions;
                    break;
            }

            entities = default;

            var elementsResult = UserUtil.GetElementsOfType<DataTypes.Entity>(principalId);

            if (elementsResult.IsErr) return false;

            entities = new();

            if (conditions == null)
            {
                var elements = elementsResult.AsOk();

                foreach (var e in elements)
                {
                    if (sourceWorldId == e.wid)
                        entities.AddLast(e);
                }
            }
            else
            {
                var entitiesToCheckOn = elementsResult.AsOk();

                foreach (var entity in entitiesToCheckOn)
                {
                    if (sourceWorldId != entity.wid) continue;

                    foreach (var condition in conditions)
                    {
                        if (entity.fields.TryGetValue(condition.key, out var value) == false)
                        {
                            goto entityLoopEnd;
                        }
                        else
                        {
                            if (condition.value != null)
                            {
                                switch (condition.value)
                                {
                                    case EntityFilterPredicate.Text predicate:

                                        if (predicate.Predicate.Invoke(value) == false) goto entityLoopEnd;

                                        break;
                                    case EntityFilterPredicate.Double predicate:

                                        if (value.TryParseValue<double>(out var decimalValue) == false)
                                        {
                                            $"Issue parsing field of id: {condition.key}, from string to double".Error();
                                            return false;
                                        }

                                        if (predicate.Predicate.Invoke(decimalValue) == false) goto entityLoopEnd;

                                        break;
                                    case EntityFilterPredicate.ULong predicate:

                                        if (value.TryParseValue<ulong>(out var ulongValue) == false)
                                        {
                                            $"Issue parsing field of id: {condition.key}, from string to ulong".Error();
                                            return false;
                                        }

                                        if (predicate.Predicate.Invoke(ulongValue) == false) goto entityLoopEnd;

                                        break;
                                    case EntityFilterPredicate.Bool predicate:

                                        if (value.TryParseValue<bool>(out var boolValue) == false)
                                        {
                                            $"Issue parsing field of id: {condition.key}, from string to bool".Error();
                                            return false;
                                        }

                                        if (predicate.Predicate.Invoke(boolValue) == false) goto entityLoopEnd;

                                        break;
                                }
                            }
                        }
                    }

                    entities.AddLast(entity);

                entityLoopEnd: { }
                }
            }

            return true;
        }

        public static bool TryQueryAllEntitiesFeild<T>(EntityFilter.Base entityFilter, out LinkedList<T> allEntityField, Func<DataTypes.Entity, T> getter, string sourceWorldId = default)
        {
            allEntityField = default;

            if (TryQueryEntities(entityFilter, out var entities, sourceWorldId) == false) return false;

            allEntityField = new();

            foreach (var entity in entities)
            {
                allEntityField.AddLast(getter(entity));
            }
            return true;
        }

        //Single output
        public static bool TryGetEntity(string uid, string entityId, out DataTypes.Entity entity, string sourceWorldId = default)
        {
            if (string.IsNullOrEmpty(sourceWorldId)) sourceWorldId = BoomManager.Instance.WORLD_CANISTER_ID;

            entity = null;

            var result = UserUtil.GetElementOfType<DataTypes.Entity>(uid, $"{sourceWorldId}{entityId}");

            if (result.IsErr)
            {
                $"{result.AsErr()}".Warning(typeof(EntityUtil).Name);

                return false;
            }

            entity = result.AsOk();

            return true;
        }
        private static bool TryGetFieldAs<T>(string uid, string entityId, string fieldName, out T outValue, T defaultValue = default, string sourceWorldId = default)
        {
            if (string.IsNullOrEmpty(sourceWorldId)) sourceWorldId = BoomManager.Instance.WORLD_CANISTER_ID;

            outValue = defaultValue;

            if (TryGetEntity(uid, entityId, out var entity, sourceWorldId) == false)
            {
                return false;
            }

            if (!entity.fields.TryGetValue(fieldName, out var value)) return false;

            if (value.TryParseValue<T>(out outValue))
            {
                return true;
            }
            $"Error on \"value\" type, current type: {value.GetType()}, desired type is: {typeof(T)}".Error();
            return false;
        }
        public static bool TryGetFieldAsText(string uid, string entityId, string fieldName, out string outValue, string defaultValue = default, string sourceWorldId = default)
        {
            return TryGetFieldAs<string>(uid, entityId, fieldName, out outValue, defaultValue, sourceWorldId);
        }
        public static bool TryGetFieldAsDouble(string uid, string entityId, string fieldName, out double outValue, double defaultValue = default, string sourceWorldId = default)
        {
            return TryGetFieldAs<double>(uid, entityId, fieldName, out outValue, defaultValue, sourceWorldId);
        }
        public static bool TryGetFieldAsTimeStamp(string uid, string entityId, string fieldName, out ulong outValue, ulong defaultValue = default, string sourceWorldId = default)
        {
            return TryGetFieldAs<ulong>(uid, entityId, fieldName, out outValue, defaultValue, sourceWorldId);
        }
        public static bool TryGetFieldAsBool(string uid, string entityId, string fieldName, out bool outValue, bool defaultValue = default, string sourceWorldId = default)
        {
            return TryGetFieldAs<bool>(uid, entityId, fieldName, out outValue, defaultValue, sourceWorldId);
        }

        //Single output (Datatypes.Entity extension functions)
        private static bool TryGetFieldAs<T>(this DataTypes.Entity entity, string fieldName, out T outValue, T defaultValue = default)
        {
            outValue = defaultValue;

            if (!entity.fields.TryGetValue(fieldName, out var value)) return false;

            if (value.TryParseValue<T>(out outValue))
            {
                return true;
            }
            $"Error on \"value\" type, current type: {value.GetType()}, desired type is: {typeof(T)}".Error();
            return false;
        }
        public static bool TryGetFieldAsText(this DataTypes.Entity entity, string fieldName, out string outValue, string defaultValue = default)
        {
            return TryGetFieldAs<string>(entity, fieldName, out outValue, defaultValue);
        }
        public static bool TryGetFieldAsDouble(this DataTypes.Entity entity, string fieldName, out double outValue, double defaultValue = default)
        {
            return TryGetFieldAs<double>(entity, fieldName, out outValue, defaultValue);
        }
        public static bool TryGetFieldAsTimeStamp(this DataTypes.Entity entity, string fieldName, out ulong outValue, ulong defaultValue = default)
        {
            return TryGetFieldAs<ulong>(entity, fieldName, out outValue, defaultValue);
        }
        public static bool TryGetFieldAsBool(this DataTypes.Entity entity, string fieldName, out bool outValue, bool defaultValue = default)
        {
            return TryGetFieldAs<bool>(entity, fieldName, out outValue, defaultValue);
        }

        //Single output (EntityOutcome extension functions)
        public static bool TryGetOutcomeFieldAsText(this EntityOutcome newEntityValues, string fieldName, out string outValue, string defaultValue = default)
        {
            outValue = defaultValue;

            if (!newEntityValues.fields.TryGetValue(fieldName, out var edit)) return false;

            switch (edit)
            {
                case EntityFieldEdit.SetText e:
                    outValue = e.Value;
                    return true;
                default:
                    $"Error on \"value\" type, current type: {edit.GetType()}, desired type is: {typeof(EntityFieldEdit.SetText)} for field: {fieldName}".Error();
                    return false;
            }
        }
        public static bool TryGetOutcomeFieldAsDouble(this EntityOutcome newEntityValues, string fieldName, out EntityFieldEdit.Numeric outValue, EntityFieldEdit.Numeric defaultValue = default)
        {
            outValue = defaultValue;

            if (!newEntityValues.fields.TryGetValue(fieldName, out var edit)) return false;

            switch (edit)
            {
                case EntityFieldEdit.Numeric e:

                    outValue = e;
                    return true;
                default:
                    $"Error on \"value\" type, current type: {edit.GetType()}, desired type is: {typeof(EntityFieldEdit.Numeric)} for field: {fieldName}".Error();
                    return false;
            }
        }
        public static bool TryGetOutcomeFieldAsTimeStamp(this EntityOutcome newEntityValues, string fieldName, out ulong outValue, ulong defaultValue = default)
        {
            outValue = defaultValue;

            if (!newEntityValues.fields.TryGetValue(fieldName, out var edit)) return false;

            switch (edit)
            {
                case EntityFieldEdit.Numeric e:

                    outValue = (ulong)e.Value;
                    return true;
                default:
                    $"Error on \"value\" type, current type: {edit.GetType()}, desired type is: {typeof(EntityFieldEdit.Numeric)} for field: {fieldName}".Error();
                    return false;
            }
        }
        public static bool TryGetOutcomeFieldAsBool(this EntityOutcome newEntityValues, string fieldName, out bool outValue, bool defaultValue = default)
        {
            outValue = defaultValue;

            if (!newEntityValues.fields.TryGetValue(fieldName, out var edit)) return false;

            switch (edit)
            {
                case EntityFieldEdit.SetText e:

                    var valueToCheck = e.Value.ToLower();
                    if (valueToCheck == "true") outValue = true;
                    else if (valueToCheck == "false") outValue = false;
                    else
                    {
                        $"Error trying to read \"{e.Value}\" as a bool value type".Error();
                        return false;
                    }

                    return true;
                default:
                    $"Error on \"value\" type, current type: {edit.GetType()}, desired type is: {typeof(EntityFieldEdit.SetText)} for field: {fieldName}".Error();
                    return false;
            }
        }

        //Formulas
        private static readonly FormulaEvaluation formulaEvaluation = new();

        public static (IEnumerable<DataTypes.Entity> callerEntities, IEnumerable<DataTypes.Entity> targetEntities, IEnumerable<DataTypes.Entity> worldEntities, IEnumerable<MainDataTypes.AllConfigs.Config> configs) GetFormulaDependencies(ActionReturn actionReturn)
        {
            (IEnumerable<DataTypes.Entity> callerEntities, IEnumerable<DataTypes.Entity> targetEntities, IEnumerable<DataTypes.Entity> worldEntities, IEnumerable<MainDataTypes.AllConfigs.Config> configs) formulaEvaluationDependencies = new();
            //CONFIG
            var configDataTypeResult = UserUtil.GetMainData<MainDataTypes.AllConfigs>();

            if (configDataTypeResult.IsOk)
            {
                if (configDataTypeResult.AsOk().configs.TryGetValue(BoomManager.Instance.WORLD_CANISTER_ID, out var worldConfig) == false)
                {
                    formulaEvaluationDependencies.configs = worldConfig.Map(e => e.Value);
                }
            }

            //WORLD
            var worldEntityDataTypeResult = UserUtil.GetData<DataTypes.Entity>(actionReturn.WorldPrincipalId);

            if (worldEntityDataTypeResult.IsOk)
            {
                formulaEvaluationDependencies.worldEntities = worldEntityDataTypeResult.AsOk().elements.Map(e => e.Value);
            }
            else
            {
                formulaEvaluationDependencies.worldEntities = new DataTypes.Entity[0];
                Debug.LogWarning(worldEntityDataTypeResult.AsErr());
            }

            //CALLER
            var callerEntityDataTypeResult = UserUtil.GetDataSelf<DataTypes.Entity>();

            if (callerEntityDataTypeResult.IsOk)
            {
                formulaEvaluationDependencies.callerEntities = callerEntityDataTypeResult.AsOk().elements.Map(e => e.Value);
            }
            else
            {
                formulaEvaluationDependencies.callerEntities = new DataTypes.Entity[0];
                Debug.LogWarning(callerEntityDataTypeResult.AsErr());
            }

            //TARGET
            if (string.IsNullOrEmpty(actionReturn.TargetPrincipalId) == false)
            {
                var targetEntityDataTypeResult = UserUtil.GetData<DataTypes.Entity>(actionReturn.TargetPrincipalId);

                if (targetEntityDataTypeResult.IsOk)
                {
                    formulaEvaluationDependencies.targetEntities = targetEntityDataTypeResult.AsOk().elements.Map(e => e.Value);
                }
                else
                {
                    formulaEvaluationDependencies.targetEntities = new DataTypes.Entity[0];
                    Debug.LogWarning(targetEntityDataTypeResult.AsErr());
                }
            }
            else formulaEvaluationDependencies.targetEntities = new DataTypes.Entity[0];


            return formulaEvaluationDependencies;
        }

        private static string ReplaceVariables(string formula, IEnumerable<DataTypes.Entity> worldEntities, IEnumerable<DataTypes.Entity> callerEntities, IEnumerable<DataTypes.Entity> targetEntities, IEnumerable<MainDataTypes.AllConfigs.Config> configs, IEnumerable<Field> args)
        {
            StringBuilder subExpr = new();
            bool isOpen = false;
            var index = 0;
            var outValue = formula;
            while (index < formula.Length)
            {
                string token = $"{formula[index]}";


                if (isOpen)
                {
                    subExpr.Append(token);
                }

                if (token == "{")
                {
                    isOpen = true;
                }
                else if (index < formula.Length - 1)
                {
                    string nextToken = $"{formula[index + 1]}";

                    if (nextToken == "}")
                    {
                        isOpen = false;
                    }
                }

                if (!isOpen && subExpr.Length > 0)
                {
                    string variable = subExpr.ToString();
                    subExpr.Length = 0;

                    var variableFieldNameElements = variable.Split('.');

                    if (variableFieldNameElements.Length == 3)
                    {
                        var source = variableFieldNameElements[0];
                        var key = variableFieldNameElements[1];
                        var fieldName = variableFieldNameElements[2];
                        var feildValue = "";
                        IEnumerable<DataTypes.Entity> entities = null;
                        if (source == "$caller") entities = callerEntities;
                        else if (source == "$target") entities = targetEntities;
                        else if (source == "$world") entities = worldEntities;
                        else if (source == "$config")
                        {
                            if (configs.TryLocate(e => e.cid == key, out var config))
                            {
                                if (!config.fields.TryGetValue(fieldName, out feildValue))
                                {
                                    $"Formula error, variable's value of id: {variable} could not be found".Error();
                                    feildValue = "Nan";
                                }
                                else
                                {
                                    if (fieldName.Contains('@'))
                                    {
                                        feildValue = EvaluateFormula(feildValue, worldEntities, callerEntities, targetEntities, configs, args).ToString();
                                    }
                                }
                            }
                            outValue = outValue.Replace("{" + $"{variable}" + "}", $"{feildValue}");
                        }

                        if (entities.TryLocate(e => e.eid == key, out var entity))
                        {
                            if (!entity.fields.TryGetValue(fieldName, out feildValue))
                            {
                                $"Formula error, variable's value of id: {variable} could not be found".Error();
                                feildValue = "Nan";
                            }
                        }
                        outValue = outValue.Replace("{" + $"{variable}" + "}", $"{feildValue}");
                    }
                    else if (variableFieldNameElements.Length == 2)
                    {
                        if (variableFieldNameElements[0] == "$args")
                        {
                            var actionArgFieldName = variableFieldNameElements[1];

                            if (args.TryLocate(e => e.FieldName == actionArgFieldName, out var argValue) == false)
                            {
                                return "Could not find arg value of field name: " + actionArgFieldName;
                            }

                            outValue = outValue.Replace("{" + $"{variable}" + "}", $"{argValue.FieldValue}");
                        }
                    }
                }


                index += 1;
            }

            return outValue;
        }
        public static double EvaluateFormula(string formula, IEnumerable<DataTypes.Entity> worldEntities, IEnumerable<DataTypes.Entity> callerEntities, IEnumerable<DataTypes.Entity> targetEntities, IEnumerable<MainDataTypes.AllConfigs.Config> configs, IEnumerable<Field> args)
        {
            var formulaWithVariableReplaced = ReplaceVariables(formula, worldEntities, callerEntities, targetEntities, configs, args);

            return formulaEvaluation.Evaluate(formulaWithVariableReplaced);
        }

        internal static void ApplyEntityEdits(ProcessedActionResponse.Outcomes outcomes)
        {
            var uid = outcomes.uid;
            Debug.Log($"Apply outcomes to {uid}, outcomes: {outcomes.entityOutcomes}");
            var entityOutcomes = outcomes.entityOutcomes;

            Dictionary<string, DataTypes.Entity> editedEntities = new();


            foreach (var entity in entityOutcomes)
            {
                var eid = entity.Value.eid;
                var wid = entity.Value.wid;
                var fieldsEdits = entity.Value.fields;

                Dictionary<string, string> currentEntityFields = null;

                var currentEntityDataTypeResult = UserUtil.GetElementOfType<DataTypes.Entity>(uid, $"{wid}{eid}");

                if (currentEntityDataTypeResult.IsOk) currentEntityFields = currentEntityDataTypeResult.AsOk().fields;
                else currentEntityFields = new();


                if(fieldsEdits != null)
                {
                    foreach (var edit in fieldsEdits)
                    {
                        var fieldId = edit.Key;

                        switch (edit.Value)
                        {
                            case EntityFieldEdit.SetText e:
                                currentEntityFields[fieldId] = e.Value;
                                break;

                            case EntityFieldEdit.DeleteField e:
                                if (currentEntityFields.ContainsKey(fieldId)) currentEntityFields.Remove(fieldId);
                                break;
                            case EntityFieldEdit.AddToList e:
                                if (currentEntityFields.TryAdd(fieldId, e.Value) == false)
                                {
                                    currentEntityFields[fieldId] += $",{e.Value}";
                                }
                                break;
                            case EntityFieldEdit.RemoveFromList e:
                                if (currentEntityFields.ContainsKey(e.Value))
                                {
                                    var currentFieldValue = currentEntityFields[fieldId];

                                    if (currentFieldValue.Contains(e.Value))
                                    {
                                        var split = currentFieldValue.Split(',').ToList();

                                        split.Remove(e.Value);

                                        var newFieldValue = split.Reduce(e => $"{e}", ",");

                                        currentEntityFields[fieldId] = newFieldValue;
                                    }
                                }

                                break;

                            case EntityFieldEdit.Numeric e:

                                if (e.NumericType_ == EntityFieldEdit.Numeric.NumericType.Set)
                                {
                                    currentEntityFields[fieldId] = e.Value.ToString();
                                }
                                else if (e.NumericType_ == EntityFieldEdit.Numeric.NumericType.Increment)
                                {
                                    if (currentEntityFields.TryGetValue(fieldId, out var numberAsText) == false) numberAsText = "0";
                                    numberAsText.TryParseValue(out double currentNumericValue);

                                    currentEntityFields[fieldId] = (currentNumericValue + e.Value).ToString();
                                }
                                else
                                {
                                    if (currentEntityFields.TryGetValue(fieldId, out var numberAsText) == false) numberAsText = "0";
                                    numberAsText.TryParseValue(out double currentNumericValue);

                                    currentEntityFields[fieldId] = (currentNumericValue - e.Value).ToString();
                                }

                                break;

                            case EntityFieldEdit.RenewTimestamp e:

                                currentEntityFields[fieldId] = (MainUtil.Now().MilliToNano() + e.Value).ToString();

                                break;
                        }
                    }
                }

                var newEditedEntity = new DataTypes.Entity(wid, eid, currentEntityFields);
                if (entity.Value.dispose) newEditedEntity.ScheduleDisposal();

                editedEntities[eid] = newEditedEntity;
            }

            if (editedEntities.Count > 0) UserUtil.UpdateData<DataTypes.Entity>(uid, editedEntities.ToArray().Map(e => e.Value).ToArray());
        }
    }

    public class FormulaEvaluation
    {
        string specialTokens = "()^*/%+-<>";
        private string[] operators = { "-", "+", "%", "/", "*", "^", "<", ">" };
        private Func<double, double, double>[] _operations = {
        (a1, a2) => a1 - a2,
        (a1, a2) => a1 + a2,
        (a1, a2) => a1 % a2,
        (a1, a2) => a1 / a2,
        (a1, a2) => a1 * a2,
        (a1, a2) => Math.Pow(a1, a2),
        (a1, a2) => Math.Min(a1, a2),
        (a1, a2) => Math.Max(a1, a2)
    };

        public double Evaluate(string expression)
        {
            List<string> tokens = GetTokens(expression);
            Stack<double> operandStack = new Stack<double>();
            Stack<string> operatorStack = new Stack<string>();
            int tokenIndex = 0;

            while (tokenIndex < tokens.Count)
            {
                //
                string token = tokens[tokenIndex];
                if (token == "(")
                {
                    string subExpr = GetSubExpression(tokens, ref tokenIndex);
                    operandStack.Push(Evaluate(subExpr));
                    continue;
                }
                if (token == ")")
                {
                    throw new ArgumentException("Mis-matched parentheses in expression");
                }
                //
                if (Array.IndexOf(operators, token) >= 0)
                {
                    if (operatorStack.Count > 0 && Array.IndexOf(operators, token) < Array.IndexOf(operators, operatorStack.Peek()))
                    {
                        while (operatorStack.Count > 0)
                        {
                            string op = operatorStack.Pop();
                            double arg2 = operandStack.Pop();
                            double arg1 = operandStack.Pop();
                            operandStack.Push(_operations[Array.IndexOf(operators, op)](arg1, arg2));
                        }
                    }

                    operatorStack.Push(token);
                }
                else
                {
                    operandStack.Push(double.Parse(token));
                }
                //
                tokenIndex += 1;
            }
            //
            while (operatorStack.Count > 0)
            {
                string op = operatorStack.Pop();
                double arg2 = operandStack.Pop();
                double arg1 = operandStack.Pop();
                operandStack.Push(_operations[Array.IndexOf(operators, op)](arg1, arg2));
            }
            //
            return operandStack.Pop();
        }

        private string GetSubExpression(List<string> tokens, ref int index)
        {
            StringBuilder subExpr = new StringBuilder();
            int parenLevels = 1;
            index += 1;
            while (index < tokens.Count && parenLevels > 0)
            {
                string token = tokens[index];
                if (token == "(")
                {
                    parenLevels += 1;
                }

                if (token == ")")
                {
                    parenLevels -= 1;
                }

                if (parenLevels > 0)
                {
                    subExpr.Append(token);
                }

                index += 1;
            }

            if (parenLevels > 0)
            {
                throw new ArgumentException("Mis-matched parentheses in expression");
            }
            return subExpr.ToString();
        }

        private List<string> GetTokens(string expression)
        {

            List<string> tokens = new List<string>();
            StringBuilder sb = new StringBuilder();

            foreach (char c in expression.Replace(" ", string.Empty))
            {
                if (specialTokens.IndexOf(c) >= 0)
                {
                    if ((sb.Length > 0))
                    {
                        tokens.Add(sb.ToString());
                        sb.Length = 0;
                    }
                    tokens.Add($"{c}");
                }
                else
                {
                    sb.Append(c);
                }
            }

            if ((sb.Length > 0))
            {
                tokens.Add(sb.ToString());
            }
            return tokens;
        }
    }
}