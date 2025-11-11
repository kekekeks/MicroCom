template <typename T>
struct Foo;
struct NormalStruct;
template <typename T>
struct Foo
{
    T* bar;
};
struct NormalStruct
{
    int a;
    Foo<Foo<float*>**> b;
};
