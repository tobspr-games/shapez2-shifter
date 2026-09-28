using System;
using System.Runtime.CompilerServices;

namespace ShapezShifter.Hijack
{
    public readonly struct RewirerHandle : IEquatable<RewirerHandle>, IComparable<RewirerHandle>
    {
        internal static readonly RewirerHandle Invalid = default(RewirerHandle);
        private readonly int Id;

        private RewirerHandle(int id)
        {
            Id = id;
        }

        /// <inheritdoc />
        public bool Equals(RewirerHandle other)
        {
            return Id == other.Id;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is RewirerHandle other && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Id;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Id.ToString();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(RewirerHandle lhs, RewirerHandle rhs)
        {
            return lhs.Id == rhs.Id;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(RewirerHandle lhs, RewirerHandle rhs)
        {
            return lhs.Id != rhs.Id;
        }
        
        public int CompareTo(RewirerHandle other)
        {
            return Id.CompareTo(other.Id);
        }

        internal RewirerHandle Next()
        {
            return new RewirerHandle(Id + 1);
        }
    }
}
