using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using MediaCenter.SharedComponents;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace MediaCenter.LyricsFinder.Tests.SharedComponents
{

    /// <summary>
    /// Unit tests of the task extensions in <see cref="AsyncUtility"/>.
    /// </summary>
    /// <remarks>
    /// <para>These tests are hermetic: no network, no data file, no user configuration.</para>
    /// <para>They cover section 5.6 of the architecture report: the predicate of <c>WhenAny</c> used to be
    /// called on faulted tasks too, where reading <c>Result</c> throws an <see cref="AggregateException"/>
    /// instead of the exception the caller is catching.</para>
    /// </remarks>
    [TestClass]
    public class AsyncUtilityTests
    {

        private const int _foundValue = 42;


        /// <summary>
        /// Creates a task that has already completed with the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A completed task.</returns>
        private static Task<int> CompletedTask(int value)
        {
            return Task.FromResult(value);
        }


        /// <summary>
        /// Creates a task that has already failed with the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>A faulted task.</returns>
        private static Task<int> FaultedTask(Exception exception)
        {
            return Task.FromException<int>(exception);
        }


        /// <summary>
        /// Creates a task that has already been canceled.
        /// </summary>
        /// <returns>A canceled task.</returns>
        private static Task<int> CanceledTask()
        {
            var taskCompletionSource = new TaskCompletionSource<int>();

            taskCompletionSource.SetCanceled();

            return taskCompletionSource.Task;
        }


        /// <summary>
        /// Creates a task that never completes.
        /// </summary>
        /// <returns>A running task.</returns>
        /// <remarks>
        /// Never pass this to <c>WhenAny</c>, which would wait for it forever.
        /// </remarks>
        private static Task<int> RunningTask()
        {
            return new TaskCompletionSource<int>().Task;
        }


        [TestMethod]
        public async Task WhenAny_WithAMatchingTask_ReturnsItsResult()
        {
            var tasks = new[] { CompletedTask(1), CompletedTask(_foundValue) };

            var result = await tasks.WhenAny(t => t.Result == _foundValue);

            Assert.AreEqual(_foundValue, result);
        }


        [TestMethod]
        public async Task WhenAny_WithNoMatchingTask_ReturnsTheDefaultValue()
        {
            var tasks = new[] { CompletedTask(1), CompletedTask(2) };

            var result = await tasks.WhenAny(t => t.Result == _foundValue);

            Assert.AreEqual(0, result);
        }


        [TestMethod]
        public async Task WhenAny_WithAFaultedTask_SkipsItAndReturnsTheMatchingResult()
        {
            // This is the section 5.6 regression: the predicate must not read Result on a faulted task.
            var tasks = new[] { FaultedTask(new InvalidOperationException("Boom")), CompletedTask(_foundValue) };

            var result = await tasks.WhenAny(t => t.Result == _foundValue);

            Assert.AreEqual(_foundValue, result);
        }


        [TestMethod]
        public async Task WhenAny_WithOnlyFaultedTasks_ReturnsTheDefaultValueWithoutThrowing()
        {
            var tasks = new[] { FaultedTask(new InvalidOperationException("Boom")), FaultedTask(new NotSupportedException()) };

            var result = await tasks.WhenAny(t => t.Result == _foundValue);

            Assert.AreEqual(0, result);
        }


        [TestMethod]
        public async Task WhenAny_WithACanceledTask_SkipsItAndReturnsTheMatchingResult()
        {
            var tasks = new[] { CanceledTask(), CompletedTask(_foundValue) };

            var result = await tasks.WhenAny(t => t.Result == _foundValue);

            Assert.AreEqual(_foundValue, result);
        }


        [TestMethod]
        public async Task WhenAny_WithNullArguments_Throws()
        {
            var tasks = new[] { CompletedTask(_foundValue) };

            await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () => { _ = await ((IEnumerable<Task<int>>)null).WhenAny(t => true); });
            await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () => { _ = await tasks.WhenAny(null); });
        }


        [TestMethod]
        public void CollectExceptions_WithAFaultedTask_AddsTheInnerExceptionItself()
        {
            var boom = new InvalidOperationException("Boom");
            var exceptions = new List<Exception>();

            var count = new[] { FaultedTask(boom) }.CollectExceptions(exceptions);

            Assert.AreEqual(1, count);
            Assert.AreEqual(1, exceptions.Count);

            // The original exception, not the AggregateException wrapping it.
            Assert.AreSame(boom, exceptions[0]);
        }


        [TestMethod]
        public void CollectExceptions_WithSeveralFaultedTasks_AddsAllOfThem()
        {
            var boom1 = new InvalidOperationException("Boom 1");
            var boom2 = new NotSupportedException("Boom 2");
            var exceptions = new List<Exception>();

            var count = new[] { FaultedTask(boom1), CompletedTask(_foundValue), FaultedTask(boom2) }.CollectExceptions(exceptions);

            Assert.AreEqual(2, count);
            Assert.AreEqual(2, exceptions.Count);
            Assert.AreSame(boom1, exceptions[0]);
            Assert.AreSame(boom2, exceptions[1]);
        }


        [TestMethod]
        public void CollectExceptions_WithCompletedRunningAndCanceledTasks_AddsNothing()
        {
            var exceptions = new List<Exception>();

            var count = new[] { CompletedTask(_foundValue), RunningTask(), CanceledTask() }.CollectExceptions(exceptions);

            Assert.AreEqual(0, count);
            Assert.AreEqual(0, exceptions.Count);
        }


        [TestMethod]
        public void CollectExceptions_CalledTwice_DoesNotAddTheSameExceptionTwice()
        {
            var tasks = new[] { FaultedTask(new InvalidOperationException("Boom")) };
            var exceptions = new List<Exception>();

            var count1 = tasks.CollectExceptions(exceptions);
            var count2 = tasks.CollectExceptions(exceptions);

            Assert.AreEqual(1, count1);
            Assert.AreEqual(0, count2);
            Assert.AreEqual(1, exceptions.Count);
        }


        [TestMethod]
        public void CollectExceptions_WithNullArguments_Throws()
        {
            var tasks = new[] { CompletedTask(_foundValue) };

            Assert.ThrowsExactly<ArgumentNullException>(() => { _ = ((IEnumerable<Task<int>>)null).CollectExceptions(new List<Exception>()); });
            Assert.ThrowsExactly<ArgumentNullException>(() => { _ = tasks.CollectExceptions(null); });
        }

    }

}
