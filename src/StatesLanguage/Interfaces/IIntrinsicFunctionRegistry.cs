using StatesLanguage.IntrinsicFunctions;

namespace StatesLanguage.Interfaces
{
    /// <summary>
    /// Defines a registry for managing Amazon States Language Intrinsic Functions.
    /// Allows registering and unregistering custom functions.
    /// </summary>
    /// <remarks>
    /// See the <a href="https://states-language.net/spec.html#appendix-b">Intrinsic Functions</a> appendix in the specification.
    /// </remarks>
    public interface IIntrinsicFunctionRegistry
    {
        /// <summary>
        /// Registers a new intrinsic function or replaces an existing one.
        /// </summary>
        /// <param name="name">The name of the intrinsic function (e.g., "States.Format").</param>
        /// <param name="func">The delegate implementing the function's logic.</param>
        void Register(string name, IntrinsicFunctionFunc func);

        /// <summary>
        /// Unregisters an intrinsic function.
        /// </summary>
        /// <param name="name">The name of the intrinsic function to remove.</param>
        void Unregister(string name);
    }
}