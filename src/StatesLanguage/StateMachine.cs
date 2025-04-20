/*
 * Copyright 2010-2017 Amazon.com, Inc. or its affiliates. All Rights Reserved.
 * Copyright 2018- Vincent DARON All Rights Reserved.
 *
 * Licensed under the Apache License, Version 2.0 (the "License").
 * You may not use this file except in compliance with the License.
 * A copy of the License is located at
 *
 *  http://aws.amazon.com/apache2.0
 *
 * or in the "license" file accompanying this file. This file is distributed
 * on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
 * express or implied. See the License for the specific language governing
 * permissions and limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StatesLanguage.Internal;
using StatesLanguage.Internal.Validation;
using StatesLanguage.Serialization;
using StatesLanguage.States;

namespace StatesLanguage
{
    /// <summary>
    /// Represents an Amazon States Language state machine definition.
    /// A state machine is a collection of states that defines a workflow.
    /// </summary>
    /// <remarks>
    /// This class represents the top-level structure of a state machine.
    /// See the <a href="https://states-language.net/spec.html#toplevelfields">State Machine Structure</a> section in the Amazon States Language specification.
    /// </remarks>
    public class StateMachine
    {
        private StateMachine()
        {
        }

        /// <summary>
        /// OPTIONAL. A human-readable description of the state machine.
        /// </summary>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#fields">Top-level fields</a> section in the specification.
        /// </remarks>
        [JsonProperty(PropertyNames.COMMENT)]
        public string Comment { get; private set; }

        /// <summary>
        /// REQUIRED. The name of the state where the state machine execution begins.
        /// This name must exactly match one of the keys in the <see cref="States"/> dictionary.
        /// </summary>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#fields">Top-level fields</a> section in the specification.
        /// </remarks>
        [JsonProperty(PropertyNames.START_AT)]
        public string StartAt { get; private set; }

        /// <summary>
        /// OPTIONAL. The maximum number of seconds an execution of the state machine is allowed to run.
        /// If the execution exceeds this duration, it fails with a `States.Timeout` error.
        /// </summary>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#fields">Top-level fields</a> section in the specification.
        /// </remarks>
        [JsonProperty(PropertyNames.TIMEOUT_SECONDS)]
        public int? TimeoutSeconds { get; private set; }

        /// <summary>
        /// REQUIRED. A dictionary where keys are state names (strings) and values are <see cref="State"/> objects defining each state.
        /// A state machine must contain at least one state.
        /// </summary>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#fields">Top-level fields</a> and <a href="https://states-language.net/spec.html#state-types">State Types</a> sections in the specification.
        /// </remarks>
        [JsonProperty(PropertyNames.STATES)]
        public Dictionary<string, State> States { get; private set; }

        /// <summary>
        /// Deserializes a JSON string representation of a state machine into a mutable <see cref="Builder"/> instance.
        /// </summary>
        /// <param name="json">The JSON string defining the state machine.</param>
        /// <returns>A <see cref="Builder"/> instance populated from the JSON.</returns>
        /// <exception cref="StatesLanguageException">If the JSON is invalid or cannot be deserialized.</exception>
        public static Builder FromJson(string json)
        {
            try
            {
                using (var stringReader = new StringReader(json))
                using (var jsonTextReader = new JsonTextReader(stringReader))
                {
                    return GetJsonSerializer().Deserialize<Builder>(jsonTextReader);
                }
            }
            catch (Exception e)
            {
                throw new StatesLanguageException($"Could not deserialize state machine.\n{json}", e);
            }
        }

        /// <summary>
        /// Deserializes a <see cref="JObject"/> representation of a state machine into a mutable <see cref="Builder"/> instance.
        /// </summary>
        /// <param name="json">The <see cref="JObject"/> defining the state machine.</param>
        /// <returns>A <see cref="Builder"/> instance populated from the <see cref="JObject"/>.</returns>
        /// <exception cref="StatesLanguageException">If the <see cref="JObject"/> is invalid or cannot be deserialized.</exception>
        public static Builder FromJObject(JObject json)
        {
            try
            {
                return json.ToObject<Builder>(GetJsonSerializer());
            }
            catch (Exception e)
            {
                throw new StatesLanguageException($"Could not deserialize state machine.\n{json}", e);
            }
        }

        /// <summary>
        /// Serializes the current <see cref="StateMachine"/> instance into a JSON string.
        /// </summary>
        /// <param name="formatting">Specifies the formatting options for the output JSON string (e.g., indented or none).</param>
        /// <returns>A JSON string representation of the state machine.</returns>
        /// <exception cref="StatesLanguageException">If serialization fails.</exception>
        public string ToJson(Formatting formatting = Formatting.None)
        {
            try
            {
                var result = new StringBuilder();
                using (var stringWriter = new StringWriter(result))
                using (var jsonWriter = new JsonTextWriter(stringWriter))
                {
                    GetJsonSerializer(formatting).Serialize(jsonWriter, this);
                }

                return result.ToString();
            }
            catch (Exception e)
            {
                throw new StatesLanguageException("Could not serialize state machine.", e);
            }
        }

        /// <summary>
        /// Serializes the current <see cref="StateMachine"/> instance into a <see cref="JObject"/>.
        /// </summary>
        /// <returns>A <see cref="JObject"/> representation of the state machine.</returns>
        /// <exception cref="StatesLanguageException">If serialization fails.</exception>
        public JObject ToJObject()
        {
            try
            {
                return JObject.FromObject(this, GetJsonSerializer());
            }
            catch (Exception e)
            {
                throw new StatesLanguageException("Could not serialize state machine.", e);
            }
        }

        /// <summary>
        /// Creates a new, empty <see cref="Builder"/> instance for constructing a <see cref="StateMachine"/>.
        /// </summary>
        /// <returns>A new <see cref="Builder"/> instance.</returns>
        public static Builder GetBuilder()
        {
            return new Builder();
        }

        internal static JsonSerializer GetJsonSerializer(Formatting formatting = Formatting.None)
        {
            var jsonSerializer = new JsonSerializer();

            jsonSerializer.Converters.Add(new StateConverter());
            jsonSerializer.Converters.Add(new ChoiceDeserializer());
            jsonSerializer.Converters.Add(new WaitStateDeserializer());
            jsonSerializer.Converters.Add(new TransitionStateDeserializer());
            jsonSerializer.Converters.Add(new ParallelStateSerializer());
            jsonSerializer.Converters.Add(new MapStateSerializer());

            jsonSerializer.ContractResolver = StatesContractResolver.Instance;
            jsonSerializer.Formatting = formatting;
            return jsonSerializer;
        }

        /// <summary>
        /// Builder pattern implementation for creating immutable <see cref="StateMachine"/> objects.
        /// Provides methods to set the properties of a state machine before building it.
        /// </summary>
        [JsonObject(MemberSerialization.Fields)]
        public sealed class Builder
        {
            [JsonProperty(PropertyNames.COMMENT)] private string _comment;

            [JsonProperty(PropertyNames.START_AT)] private string _startAt;

            [JsonProperty(PropertyNames.STATES)]
            private Dictionary<string, State.IBuilder<State>> _states = new Dictionary<string, State.IBuilder<State>>();

            [JsonProperty(PropertyNames.TIMEOUT_SECONDS)]
            private int? _timeoutSeconds;

            /// <summary>
            /// OPTIONAL. Sets a human-readable description for the state machine.
            /// </summary>
            /// <param name="comment">The description text.</param>
            /// <returns>This builder instance for method chaining.</returns>
            /// <remarks>
            /// See the <a href="https://states-language.net/spec.html#fields">Top-level fields</a> section in the specification.
            /// </remarks>
            public Builder Comment(string comment)
            {
                _comment = comment;
                return this;
            }

            /// <summary>
            /// REQUIRED. Sets the name of the state where the state machine execution should begin.
            /// This name must correspond to one of the states added via the <see cref="State{T}"/> method.
            /// </summary>
            /// <param name="startAt">The name of the starting state.</param>
            /// <returns>This builder instance for method chaining.</returns>
            /// <remarks>
            /// See the <a href="https://states-language.net/spec.html#fields">Top-level fields</a> section in the specification.
            /// </remarks>
            public Builder StartAt(string startAt)
            {
                _startAt = startAt;
                return this;
            }

            /// <summary>
            /// OPTIONAL. Sets the maximum time, in seconds, that a state machine execution is allowed to run.
            /// If the execution exceeds this duration, it fails with a `States.Timeout` error.
            /// </summary>
            /// <param name="timeoutSeconds">The timeout duration in seconds. Must be a positive integer if provided.</param>
            /// <returns>This builder instance for method chaining.</returns>
            /// <remarks>
            /// See the <a href="https://states-language.net/spec.html#fields">Top-level fields</a> section in the specification.
            /// </remarks>
            public Builder TimeoutSeconds(int timeoutSeconds)
            {
                _timeoutSeconds = timeoutSeconds;
                return this;
            }

            /// <summary>
            /// REQUIRED. Adds a state definition to the state machine.
            /// A state machine MUST contain at least one state.
            /// </summary>
            /// <typeparam name="T">The specific type of the state being added (e.g., <see cref="PassState"/>, <see cref="TaskState"/>).</typeparam>
            /// <param name="stateName">The name of the state. This name must be unique within the state machine and is used for transitions and the <see cref="StartAt"/> field.</param>
            /// <param name="stateBuilder">A builder instance for the state to be added (e.g., obtained from <see cref="StateMachineBuilder.PassState()"/>).</param>
            /// <returns>This builder instance for method chaining.</returns>
            /// <remarks>
            /// See the <a href="https://states-language.net/spec.html#fields">Top-level fields</a> and <a href="https://states-language.net/spec.html#state-types">State Types</a> sections in the specification.
            /// </remarks>
            public Builder State<T>(string stateName, State.IBuilder<T> stateBuilder) where T : State
            {
                _states.Add(stateName, stateBuilder);
                return this;
            }

            /// <summary>
            /// Constructs an immutable <see cref="StateMachine"/> instance from the current configuration of the builder.
            /// This method also performs validation checks to ensure the state machine definition conforms to the Amazon States Language specification.
            /// </summary>
            /// <returns>An immutable, validated <see cref="StateMachine"/> instance.</returns>
            /// <exception cref="StatesLanguageException">If the state machine definition fails validation (e.g., missing StartAt state, invalid state transitions).</exception>
            public StateMachine Build()
            {
                return new StateMachineValidator(new StateMachine
                {
                    Comment = _comment,
                    StartAt = _startAt,
                    TimeoutSeconds = _timeoutSeconds,
                    States = BuildableUtils.Build(_states)
                }).Validate();
            }
        }
    }
}