﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace DotNetty.Transport.Channels
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using DotNetty.Common.Utilities;

    /**
     * A skeletal server-side {@link Channel} implementation.  A server-side
     * {@link Channel} does not allow the following operations:
     * 
     * {@link #connect(EndPoint, ChannelPromise)}
     * {@link #disconnect(ChannelPromise)}
     * {@link #write(Object, ChannelPromise)}
     * {@link #flush()}
     * and the shortcut methods which calls the methods mentioned above
     */
    public abstract class AbstractServerChannel<TChannel, TUnsafe> : AbstractChannel<TChannel, TUnsafe>, IServerChannel
        where TChannel : AbstractServerChannel<TChannel, TUnsafe>
        where TUnsafe : AbstractServerChannel<TChannel, TUnsafe>.DefaultServerUnsafe, new()
    {
        static readonly ChannelMetadata METADATA = new ChannelMetadata(false, 16);

        /**
         * Creates a new instance.
         */
        protected AbstractServerChannel()
            : base(null)
        {
        }

        public override ChannelMetadata Metadata => METADATA;

        protected override EndPoint RemoteAddressInternal => null;

        protected override void DoDisconnect() => throw new NotSupportedException();

        //protected override IChannelUnsafe NewUnsafe() => new DefaultServerUnsafe(this);

        protected override void DoWrite(ChannelOutboundBuffer buf) => throw new NotSupportedException();

        protected override object FilterOutboundMessage(object msg) => throw new NotSupportedException();

        public class DefaultServerUnsafe : AbstractUnsafe
        {
            Task err;

            public override void Initialize(TChannel channel)
            {
                base.Initialize(channel);
                this.err = TaskUtil.FromException(new NotSupportedException());
            }

            public override Task ConnectAsync(EndPoint remoteAddress, EndPoint localAddress) => this.err;
        }
    }
}