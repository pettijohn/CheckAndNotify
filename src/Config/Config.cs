public static class Config<T>
{
    public static T? Read (string path = "config.jsonc")
    {
        // Read the file
        // Deserialize into classes in this namespace 
        // Evaluate inheritence (necessary? or magically happens via type runtime?)
        // Return appropriate
        return default(T);
    }
}