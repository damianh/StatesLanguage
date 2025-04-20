using Newtonsoft.Json.Linq;
using StatesLanguage.States;

namespace StatesLanguage.Interfaces
{
    /// <summary>
    /// Defines the contract for processing input and output data for states according to the
    /// Amazon States Language specification.
    /// </summary>
    /// <remarks>
    /// Implementations of this interface handle the application of InputPath, Parameters,
    /// ResultSelector, ResultPath, and OutputPath fields.
    /// See the <a href="https://states-language.net/spec.html#data">Input and Output Processing</a> section in the specification.
    /// </remarks>
    public interface IInputOutputProcessor
    {
        /// <summary>
        /// Calculates the effective input for a state.
        /// </summary>
        /// <param name="input">The raw input to the state.</param>
        /// <param name="inputPath">The JSONPath string specifying the portion of the input to pass to the state (InputPath field). Defaults to '$' if not provided.</param>
        /// <param name="payload">The payload template (Parameters field) to transform the input. Can be null.</param>
        /// <param name="context">The context object ($$). Can be null.</param>
        /// <returns>The effective input as a <see cref="JToken"/>.</returns>
        /// <remarks>
        /// See <a href="https://states-language.net/spec.html#filters">InputPath</a> and
        /// <a href="https://states-language.net/spec.html#payload-template">Parameters</a> sections.
        /// </remarks>
        JToken GetEffectiveInput(JToken input, OptionalString inputPath, JObject payload, JObject context);

        /// <summary>
        /// Calculates the effective result of a state's execution.
        /// </summary>
        /// <param name="output">The raw result produced by the state's execution.</param>
        /// <param name="payload">The result selector (ResultSelector field) to transform the raw result. Can be null.</param>
        /// <param name="context">The context object ($$). Can be null.</param>
        /// <returns>The effective result as a <see cref="JToken"/>.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#result-selector">ResultSelector</a> section.
        /// </remarks>
        JToken GetEffectiveResult(JToken output, JObject payload, JObject context);

        /// <summary>
        /// Calculates the final effective output of a state.
        /// </summary>
        /// <param name="input">The effective input to the state (result of <see cref="GetEffectiveInput"/>).</param>
        /// <param name="result">The effective result of the state (result of <see cref="GetEffectiveResult"/>).</param>
        /// <param name="outputPath">The JSONPath string specifying the portion of the final result to pass as the state's output (OutputPath field). Defaults to '$' if not provided. If null, the output is an empty object {}.</param>
        /// <param name="resultPath">The JSONPath string specifying where to insert the effective result into the effective input (ResultPath field). Defaults to '$' if not provided.</param>
        /// <returns>The final effective output as a <see cref="JToken"/>.</returns>
        /// <remarks>
        /// See <a href="https://states-language.net/spec.html#result-path">ResultPath</a> and
        /// <a href="https://states-language.net/spec.html#filters">OutputPath</a> sections.
        /// </remarks>
        JToken GetEffectiveOutput(JToken input, JToken result, OptionalString outputPath, OptionalString resultPath);

        /// <summary>
        /// Resolves the value for a Fail state's "ErrorPath" or "CausePath" field.
        /// </summary>
        /// <param name="input">The input data available to the state.</param>
        /// <param name="failPath">The Reference Path or Intrinsic Function string from the "ErrorPath" or "CausePath" field.</param>
        /// <param name="payload">The Parameters field (payload template), not typically used for Fail paths but included for consistency.</param>
        /// <param name="context">The context object ($$).</param>
        /// <returns>The resolved string value for the path as a <see cref="JToken"/>.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#fail-state">Fail State</a> section, specifically ErrorPath and CausePath.
        /// The resolved value MUST be a string.
        /// </remarks>
        JToken GetFailPathValue(JToken input, OptionalString failPath, JObject payload, JObject context);
    }
}