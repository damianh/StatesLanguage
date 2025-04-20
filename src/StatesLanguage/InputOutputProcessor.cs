using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using StatesLanguage.Interfaces;
using StatesLanguage.Internal;
using StatesLanguage.IntrinsicFunctions;
using StatesLanguage.ReferencePaths;
using StatesLanguage.States;

namespace StatesLanguage
{
    /// <summary>
    /// Implements the <see cref="Interfaces.IInputOutputProcessor"/> interface, handling the data flow logic
    /// (InputPath, Parameters, ResultSelector, ResultPath, OutputPath) for states as defined in the
    /// Amazon States Language specification.
    /// </summary>
    /// <remarks>
    /// This class uses JSONPath for path evaluations and supports Intrinsic Functions via the provided registry.
    /// See the <a href="https://states-language.net/spec.html#data">Input and Output Processing</a> section in the specification.
    /// </remarks>
    public class InputOutputProcessor : IInputOutputProcessor
    {
        private readonly IntrinsicFunctionRegistry _registry;
        private const string ROOT_MEMBER_OBJECT = "$";

        /// <summary>
        /// Initializes a new instance of the <see cref="InputOutputProcessor"/> class.
        /// </summary>
        /// <param name="registry">The registry used to resolve and execute Intrinsic Functions.</param>
        public InputOutputProcessor(IntrinsicFunctionRegistry registry)
        {
            _registry = registry;
        }
        
        /// <summary>
        /// Calculates the effective input for a state by applying the InputPath, Parameters, and context object.
        /// </summary>
        /// <param name="input">The raw input to the state.</param>
        /// <param name="inputPath">The InputPath filter to apply to the raw input. Defaults to '$' if not set.</param>
        /// <param name="payload">The Parameters field (payload template) to apply. Can be null.</param>
        /// <param name="context">The context object ($$). Can be null.</param>
        /// <returns>The effective input JToken for the state.</returns>
        /// <exception cref="PathMatchFailureException">Thrown if the InputPath does not match.</exception>
        /// <exception cref="ParameterPathFailureException">Thrown if a path within the Parameters field does not match the input or context.</exception>
        /// <remarks>
        /// See <a href="https://states-language.net/spec.html#filters">InputPath</a> and
        /// <a href="https://states-language.net/spec.html#payload-template">Parameters</a> sections.
        /// </remarks>
        public JToken GetEffectiveInput(JToken input, OptionalString inputPath, JObject payload, JObject context)
        {
            if (!inputPath.IsSet)
                inputPath.Value = ROOT_MEMBER_OBJECT;
            
            return TransformPayloadTemplate(ExtractTokenFromJsonPath(input, inputPath.Value), payload, context);
        }

        /// <summary>
        /// Calculates the effective result by applying the ResultSelector and context object to the state's raw result.
        /// </summary>
        /// <param name="output">The raw result (output) from the state's execution.</param>
        /// <param name="payload">The ResultSelector field (payload template) to apply. Can be null.</param>
        /// <param name="context">The context object ($$). Can be null.</param>
        /// <returns>The effective result JToken.</returns>
        /// <exception cref="ParameterPathFailureException">Thrown if a path within the ResultSelector field does not match the output or context.</exception>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#result-selector">ResultSelector</a> section.
        /// </remarks>
        public JToken GetEffectiveResult(JToken output, JObject payload, JObject context)
        {
            return TransformPayloadTemplate(output, payload, context);
        }

        /// <summary>
        /// Calculates the effective output of a state by combining the effective input and the effective result using ResultPath and OutputPath.
        /// </summary>
        /// <param name="input">The effective input to the state (after InputPath and Parameters).</param>
        /// <param name="result">The effective result from the state (after ResultSelector).</param>
        /// <param name="outputPath">The OutputPath filter to apply to the final result. Defaults to '$' if not set. If null, returns {}.</param>
        /// <param name="resultPath">The path where the effective result should be inserted into the effective input. Defaults to '$' if not set.</param>
        /// <returns>The final effective output JToken of the state.</returns>
        /// <exception cref="ResultPathMatchFailureException">Thrown if the ResultPath cannot be applied.</exception>
        /// <exception cref="PathMatchFailureException">Thrown if the OutputPath does not match.</exception>
        /// <remarks>
        /// See <a href="https://states-language.net/spec.html#result-path">ResultPath</a> and
        /// <a href="https://states-language.net/spec.html#filters">OutputPath</a> sections.
        /// </remarks>
        public JToken GetEffectiveOutput(JToken input, JToken result, OptionalString outputPath, OptionalString resultPath)
        {
            if (!outputPath.IsSet)
            {
                outputPath.Value = ROOT_MEMBER_OBJECT;
            }
            else if (!outputPath.HasValue)
            {
                //If the value of OutputPath is null, that means the input and result are discarded,
                //and the effective output from the state is an empty JSON object, {}
                return new JObject();
            }
            
            if (!resultPath.IsSet)
            {
                resultPath.Value = ROOT_MEMBER_OBJECT;
            }
            return ExtractTokenFromJsonPath(HandleResultPath(input, result, resultPath.Value),outputPath.Value);
        }

        /// <summary>
        /// Resolves the value for a Fail state's "ErrorPath" or "CausePath" field.
        /// The path can be a Reference Path or an Intrinsic Function.
        /// The resolved value MUST be a string.
        /// </summary>
        /// <param name="input">The input data available to the state.</param>
        /// <param name="failPath">The Reference Path or Intrinsic Function string from the "ErrorPath" or "CausePath" field.</param>
        /// <param name="payload">The Parameters field (payload template), not typically used for Fail paths but included for consistency.</param>
        /// <param name="context">The context object ($$).</param>
        /// <returns>The resolved string value for the path.</returns>
        /// <exception cref="PathMatchFailureException">Thrown if the path does not resolve to a string value or cannot be found.</exception>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#fail-state">Fail State</a> section, specifically ErrorPath and CausePath.
        /// </remarks>
        public JToken GetFailPathValue(JToken input, OptionalString failPath, JObject payload, JObject context)
        {
            // A JSONPath Fail state MAY have "ErrorPath" and "CausePath" fields whose values
            // MUST be Reference Paths or Intrinsic Functions which,
            // when resolved, MUST be string values

            if (!failPath.IsSet)
                failPath.Value = ROOT_MEMBER_OBJECT;

            JToken result;

            if (IntrinsicFunction.TryParse(failPath, out var intrinsicFunction))
            {
                result =  _registry.CallFunction(intrinsicFunction, input, context);
            }
            else
            {
                result = ExtractTokenFromJsonPath(input, failPath);
            }

            if (result.Type != JTokenType.String)
            {
                throw new PathMatchFailureException($"Failed to extract value from Fail Path: '{failPath}' value must resolve as a string");
            }

            return result;
        }

        private static JToken ExtractTokenFromJsonPath(JToken input, string path)
        {
            if (input == null)
            {
                throw new PathMatchFailureException($"Input is null, unable to extract Path '{path}'");
            }
            
            if (path == null)
            {
                return new JObject();
            }
            
            if(!path.StartsWith(ROOT_MEMBER_OBJECT))
                throw new PathMatchFailureException($"Invalid JsonPath '{path}', must start with '{ROOT_MEMBER_OBJECT}'");
            
            if (path.Equals(ROOT_MEMBER_OBJECT))
            {
                return input;
            }
            
            var tokens = input.SelectTokens(path).ToArray();

            switch (tokens.Length)
            {
                case 0:
                    throw new PathMatchFailureException($"Input Path '{path}' does not exists in Input received: '{input}'");
                case 1:
                    return tokens[0];
                default:
                {
                    var arr = new JArray();
                    foreach(var t in tokens)
                        arr.Add(t);
                    return arr;
                }
            }
        }

        private JToken TransformPayloadTemplate(JToken input, JToken payload, JObject context)
        {
            if (payload == null)
            {
                return input;
            }

            return payload.Type switch
            {
                JTokenType.Array => TransformPayloadArray(input, (JArray) payload, context),
                JTokenType.Object => TransformPayloadObject(input, (JObject) payload, context),
                _ => payload
            };
        }
        
        private JToken TransformPayloadArray(JToken input, JArray parameters, JObject context)
        {
            foreach (var element in parameters)
            {
                TransformPayloadTemplate(input, element, context);
            }

            return parameters;
        }
        
        private JToken TransformPayloadObject(JToken input, JObject parameters, JObject context)
        {
            var changes = new Dictionary<string, JToken>();
            foreach (var element in parameters)
            {
                if (element.Value is JContainer container)
                {
                    TransformPayloadTemplate(input, container, context);
                }
                else if (element.Value.Type == JTokenType.String && element.Key.EndsWith(".$"))
                {
                    var elementValue = element.Value.Value<string>();
                    var newPropertyName = element.Key.Substring(0, element.Key.Length - 2);

                    if (elementValue.StartsWith("$$."))
                    {
                        Ensure.IsNotNull(context, new ParameterPathFailureException($"Context is null, unable to extract Input Path '{elementValue}'"));
                        var contextToken = context.SelectToken(elementValue.Substring(1, elementValue.Length - 1));
                        Ensure.IsNotNull(contextToken, new ParameterPathFailureException($"Input Path '{elementValue}' does not exists in Context received: '{context}'"));
                        changes.Add(element.Key, new JProperty(newPropertyName, contextToken));
                    }
                    else if (elementValue.StartsWith('$'))
                    {
                        var token = input.SelectToken(elementValue);
                        Ensure.IsNotNull(token, new ParameterPathFailureException($"Input Path '{elementValue}' does not exists in Input received: '{input}'"));
                        changes.Add(element.Key, new JProperty(newPropertyName, token));
                    }
                    else // Intrinsic functions
                    {
                        var f = IntrinsicFunction.Parse(elementValue);
                        changes.Add(element.Key,
                            new JProperty(
                                newPropertyName,
                                _registry.CallFunction(f, input, context)));
                    }
                }
            }

            foreach (var keyVal in changes)
            {
                parameters.Remove(keyVal.Key);
                parameters.Add(keyVal.Value);
            }

            return parameters; }

        private static JToken HandleResultPath(JToken input, JToken result, string resultPath)
        {
            switch (resultPath)
            {
                case null:
                    return input;
                case ROOT_MEMBER_OBJECT:
                    return result;
                default:
                    // Check if token already exists
                    var token = input.SelectToken(resultPath);
                    if (token != null)
                    {
                        token.Replace(result);
                        return input;
                    }

                    var refPath = ReferencePath.Parse(resultPath);
                    token = CreateJTokenFromResult(result, refPath.Parts);

                    if (token.Type == input.Type && input is JContainer container)
                    {
                        container.Merge(token,new JsonMergeSettings
                        {
                            MergeArrayHandling = MergeArrayHandling.Merge,
                            MergeNullValueHandling = MergeNullValueHandling.Ignore
                        });
                        return container;
                    }

                    throw new ResultPathMatchFailureException($"Unable to apply result path '{resultPath}' to input '{input}'");
            }
        }
        
        private static JToken CreateJTokenFromResult(JToken result, List<PathToken> filters)
        {
            if (filters.Count == 0)
                return result;
            
            var token = filters[0];
            filters.RemoveAt(0);
            var r = CreateJTokenFromResult(result, filters);

            if (token is FieldToken fieldFilter)
            {
                var tmp = new JObject {{fieldFilter.Name, r}};
                result = tmp;
            }
            else if (token is ArrayIndexToken arrayIndexFilter)
            {
                var tmp = new JArray();
                for (int i = 0; i < arrayIndexFilter.Index; i++)
                {
                    tmp.Add(null);
                }
                tmp.Add(r);
                result = tmp;
            }
            
            return result;
        }
    }
}