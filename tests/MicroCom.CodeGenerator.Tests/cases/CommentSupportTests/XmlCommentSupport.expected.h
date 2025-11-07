/**
 * Test struct with XML comments.
 */
struct XmlCommentStruct;
/**
 * Test interface for XML comment support.
 */
struct IXmlCommentTest;
/**
 * Test enum with XML comments.
 */
enum XmlCommentEnum
{
    /**
     * First value.
     */
    First = 0,
    /**
     * Second value.
     */
    Second = 1,
    /**
     * Third value.
     */
    Third = 2,
};
/**
 * Test struct with XML comments.
 */
struct XmlCommentStruct
{
    /**
     * The integer value.
     */
    int Value;
    /**
     * The string name.
     */
    string Name;
};
/**
 * Test interface for XML comment support.
 */
COMINTERFACE(IXmlCommentTest, 4b236ea0, ca9d, 4588, a8, a3, ba, 7d, 94, f9, 41, a7) : IUnknown
{
    /**
     * Does something important.
     */
    virtual void DoSomething () = 0;
    /**
     * Gets a value.
     * @return The value.
     */
    virtual int GetValue () = 0;
    /**
     * Processes a value.
     * @param value The value to process.
     */
    virtual void ProcessValue (
        int value
    ) = 0;
};
