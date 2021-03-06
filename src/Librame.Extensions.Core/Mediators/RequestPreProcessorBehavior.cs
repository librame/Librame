﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core.Mediators
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class RequestPreProcessorBehavior<TRequest, TResponse> : IRequestPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequestIndication
    {
        private readonly IEnumerable<IRequestPreProcessor<TRequest>> _preProcessors;


        public RequestPreProcessorBehavior(IEnumerable<IRequestPreProcessor<TRequest>> preProcessors)
        {
            _preProcessors = preProcessors.NotEmpty(nameof(preProcessors));
        }


        public async Task<TResponse> HandleAsync(TRequest request, RequestHandler<TResponse> nextHandler,
            CancellationToken cancellationToken = default)
        {
            nextHandler.NotNull(nameof(nextHandler));

            foreach (var pre in _preProcessors)
                await pre.ProcessAsync(request, cancellationToken).ConfigureAwait();

            return await nextHandler.Invoke().ConfigureAwait();
        }

    }
}
