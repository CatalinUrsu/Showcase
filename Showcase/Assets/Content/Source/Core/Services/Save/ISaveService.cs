namespace Source
{
public interface ISaveService
{
    /// <summary>
    /// Writes save file now if something changed since last write.
    /// </summary>
    void Flush();
}
}
