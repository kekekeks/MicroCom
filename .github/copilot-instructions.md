
This project is about generating C++ header and C# interop for COM interfaces.
Interfaces are defined in MIDL-like custom dialect. Before doing anything about IDL files inspect existing files in tests/MicroCom.CodeGenerator.Tests/cases.

Tests involving idl files are split into files in cases dir and actual xunit tests. Files are discovered by naming convention. For example, for a test named "SomeTest" in "SomeClass" there should be a file "SomeTest.idl" in cases/SomeClass dir. When adding a new .idl file don't forget to add the actual test.

When writing a new test, you can run test first, make it generate .out.* files, and inspect those. If those look good, you can run ./scripts/update-cases.py SomeClass/SomeTest to update the expected output files.

When adding new features, make sure to add tests for them. When fixing bugs, make sure to add regression tests.