namespace Disfarm.Data.Enums
{
    /// <summary>
    /// Represents what category ingredient comes from.
    /// Used for understating what what table ingredient id is.
    /// </summary>
    public enum IngredientCategory : byte
    {
        /// <summary>
        /// Represents nothing.
        /// </summary>
        Undefined,
        Product,
        Crop,
        Food
    }
}