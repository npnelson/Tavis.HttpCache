// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Tavis.Test
{
    using Microsoft.Rest;
    using Microsoft.Rest.Azure;
    using Models;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Extension methods for TestServiceClient.
    /// </summary>
    public static partial class TestServiceClientExtensions
    {
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IList<string>> GetTestStringValuesAsync(this ITestServiceClient operations, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetTestStringValuesWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='value'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task ApiValuesPostAsync(this ITestServiceClient operations, string value = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.ApiValuesPostWithHttpMessagesAsync(value, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IList<TestOU>> GetTestOUsAsync(this ITestServiceClient operations, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetTestOUsWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<string> ApiValuesByIdGetAsync(this ITestServiceClient operations, int id, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.ApiValuesByIdGetWithHttpMessagesAsync(id, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='value'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task ApiValuesByIdPutAsync(this ITestServiceClient operations, int id, string value = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.ApiValuesByIdPutWithHttpMessagesAsync(id, value, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task ApiValuesByIdDeleteAsync(this ITestServiceClient operations, int id, CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.ApiValuesByIdDeleteWithHttpMessagesAsync(id, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

    }
}
