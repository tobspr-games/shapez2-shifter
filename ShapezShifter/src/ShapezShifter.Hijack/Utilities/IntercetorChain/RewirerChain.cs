namespace ShapezShifter.Hijack
{
    public static class RewirerChain
    {
        public static RewirerChainLink BeginRewiringWith(IChainableRewirer rewirer)
        {
            RewirerHandle handle = GameRewirers.AddRewirer(rewirer);
            rewirer.AfterHijack.Register(RemoveInterceptor);
            return new RewirerChainLink(rewirer.AfterHijack);

            void RemoveInterceptor()
            {
                GameRewirers.RemoveRewirer(handle);
            }
        }
        
        /// <summary>
        /// Creates a <c>RewirerChainLink</c> that registers and removes a <c>IChainableRewirer</c>
        /// like <c>BeginRewiringWith(IChainableRewirer)</c>. Additionally, this overload accepts a constant <c>RewirerHandle</c>
        /// that the rewirer should be registered with. Thus, this overload should be used when the same rewirer
        /// chain is invoked multiple times, because it ensures all rewirers are applied in a deterministic order based on the handle.
        /// </summary>
        public static RewirerChainLink BeginRewiringWith(RewirerHandle reservedHandle, IChainableRewirer rewirer)
        {
            GameRewirers.AddRewirer(reservedHandle, rewirer);
            rewirer.AfterHijack.Register(RemoveInterceptor);
            return new RewirerChainLink(rewirer.AfterHijack);

            void RemoveInterceptor()
            {
                GameRewirers.RemoveRewirer(reservedHandle);
            }
        }

        public static RewirerChainLink<TPropagatedData> BeginRewiringWith<TPropagatedData>(
            IChainableRewirer<TPropagatedData> rewirer)
        {
            RewirerHandle handle = GameRewirers.AddRewirer(rewirer);
            rewirer.AfterHijack.Register(RemoveInterceptor);

            return new RewirerChainLink<TPropagatedData>(rewirer.AfterHijack);

            void RemoveInterceptor(TPropagatedData obj)
            {
                GameRewirers.RemoveRewirer(handle);
            }
        }
        
        /// <summary>
        /// <inheritdoc cref="BeginRewiringWith(RewirerHandle,IChainableRewirer)"/>
        /// </summary>
        public static RewirerChainLink<TPropagatedData> BeginRewiringWith<TPropagatedData>(
            RewirerHandle reservedHandle,
            IChainableRewirer<TPropagatedData> rewirer)
        {
            GameRewirers.AddRewirer(reservedHandle, rewirer);
            rewirer.AfterHijack.Register(RemoveInterceptor);

            return new RewirerChainLink<TPropagatedData>(rewirer.AfterHijack);

            void RemoveInterceptor(TPropagatedData obj)
            {
                GameRewirers.RemoveRewirer(reservedHandle);
            }
        }
    }
}
