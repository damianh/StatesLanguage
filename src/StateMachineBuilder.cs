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
using StatesLanguage.Conditions;
using StatesLanguage.Internal;
using StatesLanguage.States;

namespace StatesLanguage
{
    /// <summary>
    /// Fluent API for creating a <see cref="StateMachine"/> object and its components (States, Conditions, Transitions, etc.).
    /// Provides static factory methods for builders.
    /// </summary>
    public static class StateMachineBuilder
    {
        /// <summary>
        /// Creates a builder for a <see cref="StateMachine"/>.
        /// A state machine represents a workflow defined using the Amazon States Language.
        /// </summary>
        /// <returns>A new <see cref="StateMachine.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#toplevelfields">State Machine Structure</a> section in the specification.
        /// </remarks>
        public static StateMachine.Builder StateMachine()
        {
            return StatesLanguage.StateMachine.GetBuilder();
        }

        /// <summary>
        /// Creates a builder for a <see cref="ParallelState"/>.
        /// A Parallel state executes multiple branches of execution concurrently.
        /// </summary>
        /// <returns>A new <see cref="ParallelState.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#parallel-state">Parallel State</a> section in the specification.
        /// </remarks>
        public static ParallelState.Builder ParallelState()
        {
            return States.ParallelState.GetBuilder();
        }

        /// <summary>
        /// Creates a builder for a <see cref="SubStateMachine"/> which represents a branch in a Parallel state.
        /// </summary>
        /// <returns>A new <see cref="SubStateMachine.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#parallel-state">Parallel State</a> section in the specification.
        /// </remarks>
        public static SubStateMachine.Builder SubStateMachine()
        {
            return States.SubStateMachine.GetBuilder();
        }

        /// <summary>
        /// Creates a builder for a <see cref="PassState"/>.
        /// A Pass state simply passes its input to its output, optionally transforming it or injecting fixed data.
        /// </summary>
        /// <returns>A new <see cref="PassState.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#pass-state">Pass State</a> section in the specification.
        /// </remarks>
        public static PassState.Builder PassState()
        {
            return States.PassState.GetBuilder();
        }

        /// <summary>
        /// Creates a builder for a <see cref="SucceedState"/>.
        /// A Succeed state terminates the state machine execution successfully.
        /// </summary>
        /// <returns>A new <see cref="SucceedState.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#succeed-state">Succeed State</a> section in the specification.
        /// </remarks>
        public static SucceedState.Builder SucceedState()
        {
            return States.SucceedState.GetBuilder();
        }

        /// <summary>
        /// Creates a builder for a <see cref="FailState"/>.
        /// A Fail state terminates the state machine execution and marks it as failed.
        /// </summary>
        /// <returns>A new <see cref="FailState.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#fail-state">Fail State</a> section in the specification.
        /// </remarks>
        public static FailState.Builder FailState()
        {
            return States.FailState.GetBuilder();
        }

        /// <summary>
        /// Creates a builder for a <see cref="WaitState"/>.
        /// A Wait state delays the state machine execution for a specified duration or until a specific time.
        /// </summary>
        /// <returns>A new <see cref="WaitState.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#wait-state">Wait State</a> section in the specification.
        /// </remarks>
        public static WaitState.Builder WaitState()
        {
            return States.WaitState.GetBuilder();
        }

        /// <summary>
        /// Creates a builder for a <see cref="TaskState"/>.
        /// A Task state represents a single unit of work performed by the state machine, identified by a resource ARN.
        /// </summary>
        /// <returns>A new <see cref="TaskState.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#task-state">Task State</a> section in the specification.
        /// </remarks>
        public static TaskState.Builder TaskState()
        {
            return States.TaskState.GetBuilder();
        }

        /// <summary>
        /// Creates a builder for a <see cref="ChoiceState"/>.
        /// A Choice state adds branching logic based on conditions evaluated against the input.
        /// </summary>
        /// <returns>A new <see cref="ChoiceState.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State</a> section in the specification.
        /// </remarks>
        public static ChoiceState.Builder ChoiceState()
        {
            return States.ChoiceState.GetBuilder();
        }

        /// <summary>
        /// Creates a builder for a <see cref="MapState"/>.
        /// A Map state processes elements of an input array concurrently.
        /// </summary>
        /// <returns>A new <see cref="MapState.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#map-state">Map State</a> section in the specification.
        /// </remarks>
        public static MapState.Builder MapState()
        {
            return States.MapState.GetBuilder();
        }

        /// <summary>
        /// Creates a builder for a <see cref="Choice"/> rule used within a <see cref="ChoiceState"/>.
        /// Each choice defines a condition and the next state to transition to if the condition is met.
        /// </summary>
        /// <returns>A new <see cref="Choice.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State</a> section in the specification.
        /// </remarks>
        public static Choice.Builder Choice()
        {
            return States.Choice.GetBuilder();
        }

        /// <summary>
        /// Creates a builder for a <see cref="Retrier"/> policy used within <see cref="TaskState"/> or <see cref="ParallelState"/>.
        /// Defines retry behavior for specified errors.
        /// </summary>
        /// <returns>A new <see cref="Retrier.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#errors">Error Handling</a> section in the specification.
        /// </remarks>
        public static Retrier.Builder Retrier()
        {
            return States.Retrier.GetBuilder();
        }

        /// <summary>
        /// Creates a builder for a <see cref="Catcher"/> policy used within <see cref="TaskState"/> or <see cref="ParallelState"/>.
        /// Defines how to handle specified errors by transitioning to a different state.
        /// </summary>
        /// <returns>A new <see cref="Catcher.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#errors">Error Handling</a> section in the specification.
        /// </remarks>
        public static Catcher.Builder Catcher()
        {
            return States.Catcher.GetBuilder();
        }


        /// <summary>
        /// Creates a builder for a <see cref="StringMatchesCondition"/>.
        /// Checks if a string value matches a pattern containing wildcard characters (*).
        /// </summary>
        /// <param name="variable">JSONPath expression pointing to the input field to compare.</param>
        /// <param name="expectedValue">The string pattern to match against. May contain '*' wildcards.</param>
        /// <returns>A new <see cref="StringMatchesCondition.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State Comparison Operators</a> section.
        /// </remarks>
        public static StringMatchesCondition.Builder Match(string variable, string expectedValue)
        {
            return StringMatchesCondition.GetBuilder().Variable(variable).ExpectedValue(expectedValue);
        }

        /// <summary>
        /// Creates a builder for a <see cref="StringEqualsCondition"/>.
        /// Checks if a string value is equal to the expected value.
        /// </summary>
        /// <param name="variable">JSONPath expression pointing to the input field to compare.</param>
        /// <param name="expectedValue">The expected string value.</param>
        /// <returns>A new <see cref="StringEqualsCondition.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State Comparison Operators</a> section.
        /// </remarks>
        public static StringEqualsCondition.Builder StringEquals(string variable, string expectedValue)
        {
            return StringEqualsCondition.GetBuilder().Variable(variable).ExpectedValue(expectedValue);
        }

        /// <summary>
        /// Creates a builder for a <see cref="StringEqualsPathCondition"/>.
        /// Checks if a string value is equal to the value at another path in the input.
        /// </summary>
        /// <param name="variable">JSONPath expression pointing to the input field to compare.</param>
        /// <param name="expectedValuePath">JSONPath expression pointing to the expected value in the input.</param>
        /// <returns>A new <see cref="StringEqualsPathCondition.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State Comparison Operators</a> section.
        /// </remarks>
        public static StringEqualsPathCondition.Builder StringEqualsPath(string variable, string expectedValuePath)
        {
            return StringEqualsPathCondition.GetBuilder().Variable(variable).ExpectedValuePath(expectedValuePath);
        }

        /// <summary>
        /// Creates a builder for a <see cref="NumericEqualsCondition{T}"/> for integer comparison.
        /// Checks if a numeric value is equal to the expected integer value.
        /// </summary>
        /// <param name="variable">JSONPath expression pointing to the input field to compare.</param>
        /// <param name="expectedValue">The expected integer value.</param>
        /// <returns>A new <see cref="NumericEqualsCondition{T}.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State Comparison Operators</a> section.
        /// </remarks>
        public static NumericEqualsCondition<long>.Builder NumericEquals(string variable, long expectedValue)
        {
            return NumericEqualsCondition<long>.GetBuilder().Variable(variable).ExpectedValue(expectedValue);
        }

        /// <summary>
        /// Creates a builder for a <see cref="NumericEqualsPathCondition"/>.
        /// Checks if a numeric value is equal to the value at another path in the input.
        /// </summary>
        /// <param name="variable">JSONPath expression pointing to the input field to compare.</param>
        /// <param name="expectedValuePath">JSONPath expression pointing to the expected value in the input.</param>
        /// <returns>A new <see cref="NumericEqualsPathCondition.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State Comparison Operators</a> section.
        /// </remarks>
        public static NumericEqualsPathCondition.Builder NumericEqualsPath(string variable, string expectedValuePath)
        {
            return NumericEqualsPathCondition.GetBuilder().Variable(variable).ExpectedValuePath(expectedValuePath);
        }

        /// <summary>
        /// Creates a builder for a <see cref="NumericEqualsCondition{T}"/> for floating-point comparison.
        /// Checks if a numeric value is equal to the expected floating-point value.
        /// </summary>
        /// <param name="variable">JSONPath expression pointing to the input field to compare.</param>
        /// <param name="expectedValue">The expected floating-point value.</param>
        /// <returns>A new <see cref="NumericEqualsCondition{T}.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State Comparison Operators</a> section.
        /// </remarks>
        public static NumericEqualsCondition<double>.Builder NumericEquals(string variable, double expectedValue)
        {
            return NumericEqualsCondition<double>.GetBuilder()
                .Variable(variable)
                .ExpectedValue(expectedValue);
        }

        /// <summary>
        /// Creates a builder for a <see cref="BooleanEqualsCondition"/>.
        /// Checks if a boolean value is equal to the expected boolean value.
        /// </summary>
        /// <param name="variable">JSONPath expression pointing to the input field to compare.</param>
        /// <param name="expectedValue">The expected boolean value.</param>
        /// <returns>A new <see cref="BooleanEqualsCondition.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State Comparison Operators</a> section.
        /// </remarks>
        public static BooleanEqualsCondition.Builder BooleanEquals(string variable, bool expectedValue)
        {
            return BooleanEqualsCondition.GetBuilder().Variable(variable).ExpectedValue(expectedValue);
        }

        /// <summary>
        /// Creates a builder for a <see cref="BooleanEqualsPathCondition"/>.
        /// Checks if a boolean value is equal to the value at another path in the input.
        /// </summary>
        /// <param name="variable">JSONPath expression pointing to the input field to compare.</param>
        /// <param name="expectedValuePath">JSONPath expression pointing to the expected value in the input.</param>
        /// <returns>A new <see cref="BooleanEqualsPathCondition.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State Comparison Operators</a> section.
        /// </remarks>
        public static BooleanEqualsPathCondition.Builder BooleanEqualsPath(string variable, string expectedValuePath)
        {
            return BooleanEqualsPathCondition.GetBuilder().Variable(variable).ExpectedValuePath(expectedValuePath);
        }

        /// <summary>
        /// Creates a builder for a <see cref="TimestampEqualCondition"/>.
        /// Checks if a timestamp value (ISO 8601 string) is equal to the expected timestamp.
        /// </summary>
        /// <param name="variable">JSONPath expression pointing to the input field to compare.</param>
        /// <param name="expectedValue">The expected timestamp value.</param>
        /// <returns>A new <see cref="TimestampEqualCondition.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State Comparison Operators</a> section.
        /// </remarks>
        public static TimestampEqualCondition.Builder TimestampEquals(string variable, DateTime expectedValue)
        {
            return TimestampEqualCondition.GetBuilder().Variable(variable).ExpectedValue(expectedValue);
        }

        /// <summary>
        /// Creates a builder for a <see cref="TimestampEqualPathCondition"/>.
        /// Checks if a timestamp value is equal to the timestamp value at another path in the input.
        /// </summary>
        /// <param name="variable">JSONPath expression pointing to the input field to compare.</param>
        /// <param name="expectedValuePath">JSONPath expression pointing to the expected timestamp value in the input.</param>
        /// <returns>A new <see cref="TimestampEqualPathCondition.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State Comparison Operators</a> section.
        /// </remarks>
        public static TimestampEqualPathCondition.Builder TimestampEqualsPath(string variable, string expectedValuePath)
        {
            return TimestampEqualPathCondition.GetBuilder().Variable(variable).ExpectedValuePath(expectedValuePath);
        }

        /// <summary>
        /// Creates a builder for a <see cref="StringGreaterThanCondition"/>.
        /// Checks if a string value is lexicographically greater than the expected value.
        /// </summary>
        /// <param name="variable">JSONPath expression pointing to the input field to compare.</param>
        /// <param name="expectedValue">The expected string value to compare against.</param>
        /// <returns>A new <see cref="StringGreaterThanCondition.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State Comparison Operators</a> section.
        /// </remarks>
        public static StringGreaterThanCondition.Builder StringGreaterThan(string variable, string expectedValue)
        {
            return StringGreaterThanCondition.GetBuilder().Variable(variable).ExpectedValue(expectedValue);
        }

        /// <summary>
        /// Creates a builder for a <see cref="StringGreaterThanPathCondition"/>.
        /// Checks if a string value is lexicographically greater than the value at another path in the input.
        /// </summary>
        /// <param name="variable">JSONPath expression pointing to the input field to compare.</param>
        /// <param name="expectedValuePath">JSONPath expression pointing to the expected value in the input.</param>
        /// <returns>A new <see cref="StringGreaterThanPathCondition.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State Comparison Operators</a> section.
        /// </remarks>
        public static StringGreaterThanPathCondition.Builder StringGreaterThanPath(string variable,
            string expectedValuePath)
        {
            return StringGreaterThanPathCondition.GetBuilder().Variable(variable).ExpectedValuePath(expectedValuePath);
        }

        /// <summary>
        /// Creates a builder for a <see cref="NumericGreaterThanCondition{T}"/> for integer comparison.
        /// Checks if a numeric value is greater than the expected integer value.
        /// </summary>
        /// <param name="variable">JSONPath expression pointing to the input field to compare.</param>
        /// <param name="expectedValue">The expected integer value.</param>
        /// <returns>A new <see cref="NumericGreaterThanCondition{T}.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State Comparison Operators</a> section.
        /// </remarks>
        public static NumericGreaterThanCondition<long>.Builder NumericGreaterThan(string variable, long expectedValue)
        {
            return NumericGreaterThanCondition<long>.GetBuilder().Variable(variable).ExpectedValue(expectedValue);
        }

        /// <summary>
        /// Creates a builder for a <see cref="NumericGreaterThanPathCondition"/>.
        /// Checks if a numeric value is greater than the value at another path in the input.
        /// </summary>
        /// <param name="variable">JSONPath expression pointing to the input field to compare.</param>
        /// <param name="expectedValuePath">JSONPath expression pointing to the expected value in the input.</param>
        /// <returns>A new <see cref="NumericGreaterThanPathCondition.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State Comparison Operators</a> section.
        /// </remarks>
        public static NumericGreaterThanPathCondition.Builder NumericGreaterThanPath(string variable,
            string expectedValuePath)
        {
            return NumericGreaterThanPathCondition.GetBuilder().Variable(variable).ExpectedValuePath(expectedValuePath);
        }

        /// <summary>
        /// Creates a builder for a <see cref="NumericGreaterThanCondition{T}"/> for floating-point comparison.
        /// Checks if a numeric value is greater than the expected floating-point value.
        /// </summary>
        /// <param name="variable">JSONPath expression pointing to the input field to compare.</param>
        /// <param name="expectedValue">The expected floating-point value.</param>
        /// <returns>A new <see cref="NumericGreaterThanCondition{T}.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State Comparison Operators</a> section.
        /// </remarks>
        public static NumericGreaterThanCondition<double>.Builder NumericGreaterThan(string variable,
            double expectedValue)
        {
            return NumericGreaterThanCondition<double>.GetBuilder().Variable(variable)
                .ExpectedValue(expectedValue);
        }

        /// <summary>
        /// Creates a builder for a <see cref="TimestampGreaterThanCondition"/>.
        /// Checks if a timestamp value (ISO 8601 string) is later than the expected timestamp.
        /// </summary>
        /// <param name="variable">JSONPath expression pointing to the input field to compare.</param>
        /// <param name="expectedValue">The expected timestamp value.</param>
        /// <returns>A new <see cref="TimestampGreaterThanCondition.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State Comparison Operators</a> section.
        /// </remarks>
        public static TimestampGreaterThanCondition.Builder TimestampGreaterThan(string variable,
            DateTime expectedValue)
        {
            return TimestampGreaterThanCondition.GetBuilder().Variable(variable).ExpectedValue(expectedValue);
        }

        /// <summary>
        /// Creates a builder for a <see cref="TimestampGreaterThanPathCondition"/>.
        /// Checks if a timestamp value is later than the timestamp value at another path in the input.
        /// </summary>
        /// <param name="variable">JSONPath expression pointing to the input field to compare.</param>
        /// <param name="expectedValuePath">JSONPath expression pointing to the expected timestamp value in the input.</param>
        /// <returns>A new <see cref="TimestampGreaterThanPathCondition.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State Comparison Operators</a> section.
        /// </remarks>
        public static TimestampGreaterThanPathCondition.Builder TimestampGreaterThanPath(string variable,
            string expectedValuePath)
        {
            return TimestampGreaterThanPathCondition.GetBuilder().Variable(variable)
                .ExpectedValuePath(expectedValuePath);
        }

        /// <summary>
        /// Creates a builder for a <see cref="StringGreaterThanOrEqualCondition"/>.
        /// Checks if a string value is lexicographically greater than or equal to the expected value.
        /// </summary>
        /// <param name="variable">JSONPath expression pointing to the input field to compare.</param>
        /// <param name="expectedValue">The expected string value to compare against.</param>
        /// <returns>A new <see cref="StringGreaterThanOrEqualCondition.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State Comparison Operators</a> section.
        /// </remarks>
        public static StringGreaterThanOrEqualCondition.Builder StringGreaterThanEquals(string variable,
            string expectedValue)
        {
            return StringGreaterThanOrEqualCondition.GetBuilder().Variable(variable).ExpectedValue(expectedValue);
        }

        /// <summary>
        /// Creates a builder for a <see cref="StringGreaterThanOrEqualPathCondition"/>.
        /// Checks if a string value is lexicographically greater than or equal to the value at another path in the input.
        /// </summary>
        /// <param name="variable">JSONPath expression pointing to the input field to compare.</param>
        /// <param name="expectedValuePath">JSONPath expression pointing to the expected value in the input.</param>
        /// <returns>A new <see cref="StringGreaterThanOrEqualPathCondition.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State Comparison Operators</a> section.
        /// </remarks>
        public static StringGreaterThanOrEqualPathCondition.Builder StringGreaterThanEqualsPath(string variable,
            string expectedValuePath)
        {
            return StringGreaterThanOrEqualPathCondition.GetBuilder().Variable(variable)
                .ExpectedValuePath(expectedValuePath);
        }

        /// <summary>
        /// Creates a builder for a <see cref="NumericGreaterThanOrEqualPathCondition"/>.
        /// Checks if a numeric value is greater than or equal to the value at another path in the input.
        /// </summary>
        /// <param name="variable">JSONPath expression pointing to the input field to compare.</param>
        /// <param name="expectedValuePath">JSONPath expression pointing to the expected value in the input.</param>
        /// <returns>A new <see cref="NumericGreaterThanOrEqualPathCondition.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State Comparison Operators</a> section.
        /// </remarks>
        public static NumericGreaterThanOrEqualPathCondition.Builder NumericGreaterThanEqualsPath(string variable,
            string expectedValuePath)
        {
            return NumericGreaterThanOrEqualPathCondition.GetBuilder().Variable(variable)
                .ExpectedValuePath(expectedValuePath);
        }

        /// <summary>
        /// Creates a builder for a <see cref="NumericGreaterThanOrEqualCondition{T}"/> for integer comparison.
        /// Checks if a numeric value is greater than or equal to the expected integer value.
        /// </summary>
        /// <param name="variable">JSONPath expression pointing to the input field to compare.</param>
        /// <param name="expectedValue">The expected integer value.</param>
        /// <returns>A new <see cref="NumericGreaterThanOrEqualCondition{T}.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State Comparison Operators</a> section.
        /// </remarks>
        public static NumericGreaterThanOrEqualCondition<long>.Builder NumericGreaterThanEquals(string variable,
            long expectedValue)
        {
            return NumericGreaterThanOrEqualCondition<long>.GetBuilder()
                .Variable(variable).ExpectedValue(expectedValue);
        }

        /// <summary>
        /// Creates a builder for a <see cref="NumericGreaterThanOrEqualCondition{T}"/> for floating-point comparison.
        /// Checks if a numeric value is greater than or equal to the expected floating-point value.
        /// </summary>
        /// <param name="variable">JSONPath expression pointing to the input field to compare.</param>
        /// <param name="expectedValue">The expected floating-point value.</param>
        /// <returns>A new <see cref="NumericGreaterThanOrEqualCondition{T}.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State Comparison Operators</a> section.
        /// </remarks>
        public static NumericGreaterThanOrEqualCondition<double>.Builder NumericGreaterThanEquals(string variable,
            double expectedValue)
        {
            return NumericGreaterThanOrEqualCondition<double>.GetBuilder()
                .Variable(variable).ExpectedValue(expectedValue);
        }

        /// <summary>
        /// Creates a builder for a <see cref="TimestampGreaterThanOrEqualCondition"/>.
        /// Checks if a timestamp value (ISO 8601 string) is later than or equal to the expected timestamp.
        /// </summary>
        /// <param name="variable">JSONPath expression pointing to the input field to compare.</param>
        /// <param name="expectedValue">The expected timestamp value.</param>
        /// <returns>A new <see cref="TimestampGreaterThanOrEqualCondition.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State Comparison Operators</a> section.
        /// </remarks>
        public static TimestampGreaterThanOrEqualCondition.Builder TimestampGreaterThanEquals(string variable,
            DateTime expectedValue)
        {
            return TimestampGreaterThanOrEqualCondition.GetBuilder().Variable(variable).ExpectedValue(expectedValue);
        }

        /// <summary>
        /// Creates a builder for a <see cref="TimestampGreaterThanOrEqualPathCondition"/>.
        /// Checks if a timestamp value is later than or equal to the timestamp value at another path in the input.
        /// </summary>
        /// <param name="variable">JSONPath expression pointing to the input field to compare.</param>
        /// <param name="expectedValuePath">JSONPath expression pointing to the expected timestamp value in the input.</param>
        /// <returns>A new <see cref="TimestampGreaterThanOrEqualPathCondition.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State Comparison Operators</a> section.
        /// </remarks>
        public static TimestampGreaterThanOrEqualPathCondition.Builder TimestampGreaterThanEqualsPath(string variable,
            string expectedValuePath)
        {
            return TimestampGreaterThanOrEqualPathCondition.GetBuilder().Variable(variable)
                .ExpectedValuePath(expectedValuePath);
        }

        /// <summary>
        /// Creates a builder for a <see cref="StringLessThanCondition"/>.
        /// Checks if a string value is lexicographically less than the expected value.
        /// </summary>
        /// <param name="variable">JSONPath expression pointing to the input field to compare.</param>
        /// <param name="expectedValue">The expected string value to compare against.</param>
        /// <returns>A new <see cref="StringLessThanCondition.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State Comparison Operators</a> section.
        /// </remarks>
        public static StringLessThanCondition.Builder StringLessThan(string variable, string expectedValue)
        {
            return StringLessThanCondition.GetBuilder().Variable(variable).ExpectedValue(expectedValue);
        }

        /// <summary>
        /// Creates a builder for a <see cref="StringLessThanPathCondition"/>.
        /// Checks if a string value is lexicographically less than the value at another path in the input.
        /// </summary>
        /// <param name="variable">JSONPath expression pointing to the input field to compare.</param>
        /// <param name="expectedValuePath">JSONPath expression pointing to the expected value in the input.</param>
        /// <returns>A new <see cref="StringLessThanPathCondition.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State Comparison Operators</a> section.
        /// </remarks>
        public static StringLessThanPathCondition.Builder StringLessThanPath(string variable, string expectedValuePath)
        {
            return StringLessThanPathCondition.GetBuilder().Variable(variable).ExpectedValuePath(expectedValuePath);
        }

        /// <summary>
        /// Creates a builder for a <see cref="NumericLessThanPathCondition"/>.
        /// Checks if a numeric value is less than the value at another path in the input.
        /// </summary>
        /// <param name="variable">JSONPath expression pointing to the input field to compare.</param>
        /// <param name="expectedValuePath">JSONPath expression pointing to the expected value in the input.</param>
        /// <returns>A new <see cref="NumericLessThanPathCondition.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State Comparison Operators</a> section.
        /// </remarks>
        public static NumericLessThanPathCondition.Builder NumericLessThanPath(string variable,
            string expectedValuePath)
        {
            return NumericLessThanPathCondition.GetBuilder().Variable(variable).ExpectedValuePath(expectedValuePath);
        }

        /// <summary>
        /// Creates a builder for a <see cref="NumericLessThanCondition{T}"/> for integer comparison.
        /// Checks if a numeric value is less than the expected integer value.
        /// </summary>
        /// <param name="variable">JSONPath expression pointing to the input field to compare.</param>
        /// <param name="expectedValue">The expected integer value.</param>
        /// <returns>A new <see cref="NumericLessThanCondition{T}.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State Comparison Operators</a> section.
        /// </remarks>
        public static NumericLessThanCondition<long>.Builder NumericLessThan(string variable, long expectedValue)
        {
            return NumericLessThanCondition<long>.GetBuilder().Variable(variable)
                .ExpectedValue(expectedValue);
        }

        /// <summary>
        /// Creates a builder for a <see cref="NumericLessThanCondition{T}"/> for floating-point comparison.
        /// Checks if a numeric value is less than the expected floating-point value.
        /// </summary>
        /// <param name="variable">JSONPath expression pointing to the input field to compare.</param>
        /// <param name="expectedValue">The expected floating-point value.</param>
        /// <returns>A new <see cref="NumericLessThanCondition{T}.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State Comparison Operators</a> section.
        /// </remarks>
        public static NumericLessThanCondition<double>.Builder NumericLessThan(string variable, double expectedValue)
        {
            return NumericLessThanCondition<double>.GetBuilder().Variable(variable)
                .ExpectedValue(expectedValue);
        }

        /// <summary>
        /// Creates a builder for a <see cref="TimestampLessThanCondition"/>.
        /// Checks if a timestamp value (ISO 8601 string) is earlier than the expected timestamp.
        /// </summary>
        /// <param name="variable">JSONPath expression pointing to the input field to compare.</param>
        /// <param name="expectedValue">The expected timestamp value.</param>
        /// <returns>A new <see cref="TimestampLessThanCondition.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State Comparison Operators</a> section.
        /// </remarks>
        public static TimestampLessThanCondition.Builder TimestampLessThan(string variable, DateTime expectedValue)
        {
            return TimestampLessThanCondition.GetBuilder().Variable(variable).ExpectedValue(expectedValue);
        }

        /// <summary>
        /// Creates a builder for a <see cref="TimestampLessThanPathCondition"/>.
        /// Checks if a timestamp value is earlier than the timestamp value at another path in the input.
        /// </summary>
        /// <param name="variable">JSONPath expression pointing to the input field to compare.</param>
        /// <param name="expectedValuePath">JSONPath expression pointing to the expected timestamp value in the input.</param>
        /// <returns>A new <see cref="TimestampLessThanPathCondition.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State Comparison Operators</a> section.
        /// </remarks>
        public static TimestampLessThanPathCondition.Builder TimestampLessThanPath(string variable,
            string expectedValuePath)
        {
            return TimestampLessThanPathCondition.GetBuilder().Variable(variable).ExpectedValuePath(expectedValuePath);
        }

        /// <summary>
        /// Creates a builder for a <see cref="StringLessThanOrEqualCondition"/>.
        /// Checks if a string value is lexicographically less than or equal to the expected value.
        /// </summary>
        /// <param name="variable">JSONPath expression pointing to the input field to compare.</param>
        /// <param name="expectedValue">The expected string value to compare against.</param>
        /// <returns>A new <see cref="StringLessThanOrEqualCondition.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State Comparison Operators</a> section.
        /// </remarks>
        public static StringLessThanOrEqualCondition.Builder StringLessThanEquals(string variable, string expectedValue)
        {
            return StringLessThanOrEqualCondition.GetBuilder().Variable(variable).ExpectedValue(expectedValue);
        }

        /// <summary>
        /// Creates a builder for a <see cref="StringLessThanOrEqualPathCondition"/>.
        /// Checks if a string value is lexicographically less than or equal to the value at another path in the input.
        /// </summary>
        /// <param name="variable">JSONPath expression pointing to the input field to compare.</param>
        /// <param name="expectedValuePath">JSONPath expression pointing to the expected value in the input.</param>
        /// <returns>A new <see cref="StringLessThanOrEqualPathCondition.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State Comparison Operators</a> section.
        /// </remarks>
        public static StringLessThanOrEqualPathCondition.Builder StringLessThanEqualsPath(string variable,
            string expectedValuePath)
        {
            return StringLessThanOrEqualPathCondition.GetBuilder().Variable(variable)
                .ExpectedValuePath(expectedValuePath);
        }

        /// <summary>
        /// Creates a builder for a <see cref="NumericLessThanOrEqualPathCondition"/>.
        /// Checks if a numeric value is less than or equal to the value at another path in the input.
        /// </summary>
        /// <param name="variable">JSONPath expression pointing to the input field to compare.</param>
        /// <param name="expectedValuePath">JSONPath expression pointing to the expected value in the input.</param>
        /// <returns>A new <see cref="NumericLessThanOrEqualPathCondition.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State Comparison Operators</a> section.
        /// </remarks>
        public static NumericLessThanOrEqualPathCondition.Builder NumericLessThanEqualsPath(string variable,
            string expectedValuePath)
        {
            return NumericLessThanOrEqualPathCondition.GetBuilder().Variable(variable)
                .ExpectedValuePath(expectedValuePath);
        }

        /// <summary>
        /// Creates a builder for a <see cref="NumericLessThanOrEqualCondition{T}"/> for integer comparison.
        /// Checks if a numeric value is less than or equal to the expected integer value.
        /// </summary>
        /// <param name="variable">JSONPath expression pointing to the input field to compare.</param>
        /// <param name="expectedValue">The expected integer value.</param>
        /// <returns>A new <see cref="NumericLessThanOrEqualCondition{T}.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State Comparison Operators</a> section.
        /// </remarks>
        public static NumericLessThanOrEqualCondition<long>.Builder NumericLessThanEquals(string variable,
            long expectedValue)
        {
            return NumericLessThanOrEqualCondition<long>.GetBuilder().Variable(variable).ExpectedValue(expectedValue);
        }

        /// <summary>
        /// Creates a builder for a <see cref="NumericLessThanOrEqualCondition{T}"/> for floating-point comparison.
        /// Checks if a numeric value is less than or equal to the expected floating-point value.
        /// </summary>
        /// <param name="variable">JSONPath expression pointing to the input field to compare.</param>
        /// <param name="expectedValue">The expected floating-point value.</param>
        /// <returns>A new <see cref="NumericLessThanOrEqualCondition{T}.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State Comparison Operators</a> section.
        /// </remarks>
        public static NumericLessThanOrEqualCondition<double>.Builder NumericLessThanEquals(string variable,
            double expectedValue)
        {
            return NumericLessThanOrEqualCondition<double>.GetBuilder().Variable(variable).ExpectedValue(expectedValue);
        }

        /// <summary>
        /// Creates a builder for a <see cref="TimestampLessThanOrEqualCondition"/>.
        /// Checks if a timestamp value (ISO 8601 string) is earlier than or equal to the expected timestamp.
        /// </summary>
        /// <param name="variable">JSONPath expression pointing to the input field to compare.</param>
        /// <param name="expectedValue">The expected timestamp value.</param>
        /// <returns>A new <see cref="TimestampLessThanOrEqualCondition.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State Comparison Operators</a> section.
        /// </remarks>
        public static TimestampLessThanOrEqualCondition.Builder TimestampLessThanEquals(string variable,
            DateTime expectedValue)
        {
            return TimestampLessThanOrEqualCondition.GetBuilder().Variable(variable).ExpectedValue(expectedValue);
        }

        /// <summary>
        /// Creates a builder for a <see cref="TimestampLessThanOrEqualPathCondition"/>.
        /// Checks if a timestamp value is earlier than or equal to the timestamp value at another path in the input.
        /// </summary>
        /// <param name="variable">JSONPath expression pointing to the input field to compare.</param>
        /// <param name="expectedValuePath">JSONPath expression pointing to the expected timestamp value in the input.</param>
        /// <returns>A new <see cref="TimestampLessThanOrEqualPathCondition.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State Comparison Operators</a> section.
        /// </remarks>
        public static TimestampLessThanOrEqualPathCondition.Builder TimestampLessThanEqualsPath(string variable,
            string expectedValuePath)
        {
            return TimestampLessThanOrEqualPathCondition.GetBuilder().Variable(variable)
                .ExpectedValuePath(expectedValuePath);
        }

        /// <summary>
        /// Creates a builder for an <see cref="IsNullCondition"/>.
        /// Checks if the value at the specified path is null.
        /// </summary>
        /// <param name="variable">JSONPath expression pointing to the input field to check.</param>
        /// <param name="expectedValue">Set to true to check for null, false to check for not null.</param>
        /// <returns>A new <see cref="IsNullCondition.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State Comparison Operators</a> section.
        /// </remarks>
        public static IsNullCondition.Builder IsNull(string variable, bool expectedValue)
        {
            return IsNullCondition.GetBuilder().Variable(variable).ExpectedValue(expectedValue);
        }

        /// <summary>
        /// Creates a builder for an <see cref="IsPresentCondition"/>.
        /// Checks if the specified path exists in the input.
        /// </summary>
        /// <param name="variable">JSONPath expression pointing to the input field to check.</param>
        /// <param name="expectedValue">Set to true to check for presence, false to check for absence.</param>
        /// <returns>A new <see cref="IsPresentCondition.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State Comparison Operators</a> section.
        /// </remarks>
        public static IsPresentCondition.Builder IsPresent(string variable, bool expectedValue)
        {
            return IsPresentCondition.GetBuilder().Variable(variable).ExpectedValue(expectedValue);
        }

        /// <summary>
        /// Creates a builder for an <see cref="IsNumericCondition"/>.
        /// Checks if the value at the specified path is a number (integer or floating-point).
        /// </summary>
        /// <param name="variable">JSONPath expression pointing to the input field to check.</param>
        /// <param name="expectedValue">Set to true to check if numeric, false otherwise.</param>
        /// <returns>A new <see cref="IsNumericCondition.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State Comparison Operators</a> section.
        /// </remarks>
        public static IsNumericCondition.Builder IsNumeric(string variable, bool expectedValue)
        {
            return IsNumericCondition.GetBuilder().Variable(variable).ExpectedValue(expectedValue);
        }

        /// <summary>
        /// Creates a builder for an <see cref="IsStringCondition"/>.
        /// Checks if the value at the specified path is a string.
        /// </summary>
        /// <param name="variable">JSONPath expression pointing to the input field to check.</param>
        /// <param name="expectedValue">Set to true to check if string, false otherwise.</param>
        /// <returns>A new <see cref="IsStringCondition.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State Comparison Operators</a> section.
        /// </remarks>
        public static IsStringCondition.Builder IsString(string variable, bool expectedValue)
        {
            return IsStringCondition.GetBuilder().Variable(variable).ExpectedValue(expectedValue);
        }

        /// <summary>
        /// Creates a builder for an <see cref="IsBooleanCondition"/>.
        /// Checks if the value at the specified path is a boolean.
        /// </summary>
        /// <param name="variable">JSONPath expression pointing to the input field to check.</param>
        /// <param name="expectedValue">Set to true to check if boolean, false otherwise.</param>
        /// <returns>A new <see cref="IsBooleanCondition.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State Comparison Operators</a> section.
        /// </remarks>
        public static IsBooleanCondition.Builder IsBoolean(string variable, bool expectedValue)
        {
            return IsBooleanCondition.GetBuilder().Variable(variable).ExpectedValue(expectedValue);
        }

        /// <summary>
        /// Creates a builder for an <see cref="IsTimestampCondition"/>.
        /// Checks if the value at the specified path is a timestamp (ISO 8601 string).
        /// </summary>
        /// <param name="variable">JSONPath expression pointing to the input field to check.</param>
        /// <param name="expectedValue">Set to true to check if timestamp, false otherwise.</param>
        /// <returns>A new <see cref="IsTimestampCondition.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State Comparison Operators</a> section.
        /// </remarks>
        public static IsTimestampCondition.Builder IsTimestamp(string variable, bool expectedValue)
        {
            return IsTimestampCondition.GetBuilder().Variable(variable).ExpectedValue(expectedValue);
        }

        /// <summary>
        /// Creates a builder for a <see cref="NotCondition"/>.
        /// Represents the logical negation of another condition.
        /// </summary>
        /// <typeparam name="T">The type of the condition being negated.</typeparam>
        /// <param name="conditionBuilder">The builder for the condition to negate.</param>
        /// <returns>A new <see cref="NotCondition.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State Logical Operators</a> section.
        /// </remarks>
        public static NotCondition.Builder Not<T>(IConditionBuilder<T> conditionBuilder) where T : ICondition
        {
            return NotCondition.GetBuilder().Condition(conditionBuilder);
        }

        /// <summary>
        /// Creates a builder for an <see cref="AndCondition"/>.
        /// Represents the logical AND of multiple conditions. All conditions must be true for the AND condition to be true.
        /// </summary>
        /// <param name="conditionBuilders">An array of builders for the conditions to combine.</param>
        /// <returns>A new <see cref="AndCondition.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State Logical Operators</a> section.
        /// </remarks>
        public static AndCondition.Builder And(params IBuildable<ICondition>[] conditionBuilders)
        {
            return AndCondition.GetBuilder().Conditions(conditionBuilders);
        }

        /// <summary>
        /// Creates a builder for an <see cref="OrCondition"/>.
        /// Represents the logical OR of multiple conditions. At least one condition must be true for the OR condition to be true.
        /// </summary>
        /// <param name="conditionBuilders">An array of builders for the conditions to combine.</param>
        /// <returns>A new <see cref="OrCondition.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#choice-state">Choice State Logical Operators</a> section.
        /// </remarks>
        public static OrCondition.Builder Or(params IBuildable<ICondition>[] conditionBuilders)
        {
            return OrCondition.GetBuilder().Conditions(conditionBuilders);
        }

        /// <summary>
        /// Creates a builder for a <see cref="NextStateTransition"/>.
        /// Represents a transition to a specific state by name.
        /// </summary>
        /// <param name="nextStateName">The name of the state to transition to.</param>
        /// <returns>A new <see cref="NextStateTransition.Builder"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#transition">Transition</a> section in the specification.
        /// </remarks>
        public static NextStateTransition.Builder Next(string nextStateName)
        {
            return NextStateTransition.GetBuilder().NextStateName(nextStateName);
        }

        /// <summary>
        /// Creates a builder for an <see cref="EndTransition"/>.
        /// Represents the termination of the current execution path (branch or state machine).
        /// </summary>
        /// <returns>A new <see cref="ITransitionBuilder{EndTransition}"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#transition">Transition</a> section in the specification.
        /// </remarks>
        public static ITransitionBuilder<EndTransition> End()
        {
            return EndTransition.GetBuilder();
        }

        /// <summary>
        /// Creates a builder for <see cref="WaitForSeconds"/> used in a <see cref="WaitState"/>.
        /// Specifies a wait duration in seconds.
        /// </summary>
        /// <param name="seconds">The number of seconds to wait. Must be positive.</param>
        /// <returns>A new <see cref="IWaitForBuilder{WaitForSeconds}"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#wait-state">Wait State</a> section in the specification.
        /// </remarks>
        public static IWaitForBuilder<WaitForSeconds> Seconds(int seconds)
        {
            return WaitForSeconds.GetBuilder().Seconds(seconds);
        }

        /// <summary>
        /// Creates a builder for <see cref="WaitForSecondsPath"/> used in a <see cref="WaitState"/>.
        /// Specifies a wait duration by referencing a path in the input data that contains the number of seconds.
        /// </summary>
        /// <param name="secondsPath">JSONPath expression pointing to the number of seconds in the input.</param>
        /// <returns>A new <see cref="IWaitForBuilder{WaitForSecondsPath}"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#wait-state">Wait State</a> section in the specification.
        /// </remarks>
        public static IWaitForBuilder<WaitForSecondsPath> SecondsPath(string secondsPath)
        {
            return WaitForSecondsPath.GetBuilder().SecondsPath(secondsPath);
        }

        /// <summary>
        /// Creates a builder for <see cref="WaitForTimestamp"/> used in a <see cref="WaitState"/>.
        /// Specifies an absolute time to wait until.
        /// </summary>
        /// <param name="timestamp">The absolute time (UTC) to wait until.</param>
        /// <returns>A new <see cref="IWaitForBuilder{WaitForTimestamp}"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#wait-state">Wait State</a> section in the specification.
        /// </remarks>
        public static IWaitForBuilder<WaitForTimestamp> Timestamp(DateTime timestamp)
        {
            return WaitForTimestamp.GetBuilder().Timestamp(timestamp);
        }

        /// <summary>
        /// Creates a builder for <see cref="WaitForTimestampPath"/> used in a <see cref="WaitState"/>.
        /// Specifies an absolute time to wait until by referencing a path in the input data that contains the timestamp string.
        /// </summary>
        /// <param name="timestampPath">JSONPath expression pointing to the timestamp string (ISO 8601 format) in the input.</param>
        /// <returns>A new <see cref="IWaitForBuilder{WaitForTimestampPath}"/> instance.</returns>
        /// <remarks>
        /// See the <a href="https://states-language.net/spec.html#wait-state">Wait State</a> section in the specification.
        /// </remarks>
        public static IWaitForBuilder<WaitForTimestampPath> TimestampPath(string timestampPath)
        {
            return WaitForTimestampPath.GetBuilder().TimestampPath(timestampPath);
        }
    }
}