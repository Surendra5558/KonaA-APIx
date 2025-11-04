namespace KonaAI.Master.Repository.Common.Constants
{
    /// <summary>
    /// Generic utility class for mapping between enum values and GUIDs.
    /// </summary>
    /// <typeparam name="T">The enum type to map.</typeparam>
    public static class EnumGuidMapper<T> where T : struct, Enum
    {
        private static readonly Dictionary<Guid, T> _enumsByGuid = new();
        private static readonly Dictionary<T, Guid> _guidsByEnum = new();
        private static bool _isInitialized = false;
        private static readonly object _lock = new object();

        /// <summary>
        /// Initializes the mapper with the provided GUID-to-enum mappings.
        /// This method should be called once during application startup.
        /// </summary>
        /// <param name="mappings">Dictionary of GUID to enum value mappings.</param>
        public static void Initialize(Dictionary<Guid, T> mappings)
        {
            lock (_lock)
            {
                if (_isInitialized)
                {
                    throw new InvalidOperationException($"EnumGuidMapper for {typeof(T).Name} has already been initialized.");
                }

                foreach (var mapping in mappings)
                {
                    _enumsByGuid[mapping.Key] = mapping.Value;
                    _guidsByEnum[mapping.Value] = mapping.Key;
                }

                _isInitialized = true;
            }
        }

        /// <summary>
        /// Gets the enum value associated with the GUID.
        /// </summary>
        /// <param name="guid">The GUID to look up.</param>
        /// <returns>The associated enum value.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the GUID doesn't have an associated enum value.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the mapper hasn't been initialized.</exception>
        public static T GetEnumByGuid(Guid guid)
        {
            EnsureInitialized();
            return _enumsByGuid[guid];
        }

        /// <summary>
        /// Tries to get the enum value associated with the GUID.
        /// </summary>
        /// <param name="guid">The GUID to look up.</param>
        /// <param name="enumValue">The associated enum value if found.</param>
        /// <returns>True if the enum value was found; otherwise, false.</returns>
        public static bool TryGetEnumByGuid(Guid guid, out T enumValue)
        {
            EnsureInitialized();
            return _enumsByGuid.TryGetValue(guid, out enumValue);
        }

        /// <summary>
        /// Gets the GUID associated with the enum value.
        /// </summary>
        /// <param name="enumValue">The enum value.</param>
        /// <returns>The associated GUID.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the enum value doesn't have an associated GUID.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the mapper hasn't been initialized.</exception>
        public static Guid GetGuid(T enumValue)
        {
            EnsureInitialized();
            if (_guidsByEnum.TryGetValue(enumValue, out var guid))
            {
                return guid;
            }
            throw new KeyNotFoundException($"No GUID mapping found for {typeof(T).Name}.{enumValue}");
        }

        /// <summary>
        /// Tries to get the GUID associated with the enum value.
        /// </summary>
        /// <param name="enumValue">The enum value.</param>
        /// <param name="guid">The associated GUID if found.</param>
        /// <returns>True if the GUID was found; otherwise, false.</returns>
        public static bool TryGetGuid(T enumValue, out Guid guid)
        {
            EnsureInitialized();
            return _guidsByEnum.TryGetValue(enumValue, out guid);
        }

        private static void EnsureInitialized()
        {
            if (!_isInitialized)
            {
                throw new InvalidOperationException($"EnumGuidMapper for {typeof(T).Name} has not been initialized. Call Initialize() first.");
            }
        }
    }
}