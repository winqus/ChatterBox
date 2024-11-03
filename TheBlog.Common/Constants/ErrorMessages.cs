namespace TheBlog.Common.Constants
{
    public static class ErrorMessages
    {
        public static readonly string FileLengthZero = "File length can't be 0.";

        public static readonly string FileEmpty = "File is empty.";
        
        public static readonly string FileTypeProhibited = "Prohibited file type.";

        public static string FileLengthExceedsLimit(string maxSize) => $"File length can't be over {maxSize}.";
    }
}