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

using System.Collections.Generic;
using Newtonsoft.Json;
using StatesLanguage.Internal;

namespace StatesLanguage.States
{
    /// <summary>
    ///     A single branch of parallel execution in a state machine. See <see cref="ParallelState" />.
    /// </summary>
    /// <remarks>
    /// See the <a href="https://states-language.net/spec.html#parallel-state">Parallel State</a> section in the specification.
    /// </remarks>
    public class SubStateMachine
    {
        private SubStateMachine()
        {
        }

        /// <summary>
        /// REQUIRED. The name of the state where the branch's execution begins.
        /// </summary>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#branch-fields">Branch Fields</a> section in the specification.
        /// </remarks>
        public string StartAt { get; private set; }

        /// <summary>
        /// OPTIONAL. A human-readable description of the branch.
        /// </summary>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#branch-fields">Branch Fields</a> section in the specification.
        /// </remarks>
        public string Comment { get; private set; }

        /// <summary>
        /// REQUIRED. A dictionary where keys are state names (strings) and values are <see cref="State"/> objects defining each state.
        /// </summary>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#branch-fields">Branch Fields</a> section in the specification.
        /// </remarks>
        public Dictionary<string, State> States { get; private set; }

        /// <returns>Builder instance to construct a <see cref="SubStateMachine" />.</returns>
        public static Builder GetBuilder()
        {
            return new Builder();
        }

        public sealed class Builder : IBuildable<SubStateMachine>
        {
            [JsonProperty(PropertyNames.COMMENT)] private string _comment;

            [JsonProperty(PropertyNames.START_AT)] private string _startAt;

            [JsonProperty(PropertyNames.STATES)] private Dictionary<string, State.IBuilder<State>> _stateBuilders =
                new Dictionary<string, State.IBuilder<State>>();

            internal Builder()
            {
            }

            /// <summary>
            /// Builds an immutable <see cref="SubStateMachine"/> object.
            /// </summary>
            /// <returns>An immutable <see cref="SubStateMachine"/> instance.</returns>
            /// <remarks>
            /// See the <a href="https://states-language.net/spec.html#branch-fields">Branch Fields</a> section in the specification.
            /// </remarks>
            public SubStateMachine Build()
            {
                return new SubStateMachine
                {
                    StartAt = _startAt,
                    Comment = _comment,
                    States = BuildableUtils.Build(_stateBuilders)
                };
            }

            /// <summary>
            /// REQUIRED. Sets the name of the state where the branch execution begins.
            /// This name must correspond to one of the states added via the <see cref="State{T}"/> method.
            /// </summary>
            /// <param name="startAt">The name of the starting state.</param>
            /// <returns>This builder instance for method chaining.</returns>
            /// <remarks>
            /// See the <a href="https://states-language.net/spec.html#branch-fields">Branch Fields</a> section in the specification.
            /// </remarks>
            public Builder StartAt(string startAt)
            {
                _startAt = startAt;
                return this;
            }

            /// <summary>
            /// OPTIONAL. Sets a human-readable description for the branch.
            /// </summary>
            /// <param name="comment">The description text.</param>
            /// <returns>This builder instance for method chaining.</returns>
            /// <remarks>
            /// See the <a href="https://states-language.net/spec.html#branch-fields">Branch Fields</a> section in the specification.
            /// </remarks>
            public Builder Comment(string comment)
            {
                _comment = comment;
                return this;
            }

            /// <summary>
            /// REQUIRED. Adds a state definition to the branch.
            /// A branch must contain at least one state.
            /// </summary>
            /// <typeparam name="T">The specific type of the state being added.</typeparam>
            /// <param name="stateName">The name that uniquely identifies the state within the branch.</param>
            /// <param name="stateBuilder">The builder for the state to be added.</param>
            /// <returns>This builder instance for method chaining.</returns>
            /// <remarks>
            /// See the <a href="https://states-language.net/spec.html#branch-fields">Branch Fields</a> section in the specification.
            /// </remarks>
            public Builder State<T>(string stateName, State.IBuilder<T> stateBuilder) where T : State
            {
                _stateBuilders.Add(stateName, stateBuilder);
                return this;
            }
        }
    }
}