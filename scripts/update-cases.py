#!/usr/bin/python3
import os
import sys
import shutil

def update_cases(case_path):
    print(f'Traversing cases in: {case_path}')
    for root, dirs, files in os.walk(case_path):
        print(f'Entering directory: {root}')
        for file in files:
            if file.endswith('.out.cs'):
                basename = file[:-7]  # Remove '.out.cs'
                src = os.path.join(root, file)
                dst = os.path.join(root, f'{basename}.expected.cs')
                print(f'Copying {src} -> {dst}')
                shutil.copyfile(src, dst)
            elif file.endswith('.out.h'):
                basename = file[:-6]  # Remove '.out.h'
                src = os.path.join(root, file)
                dst = os.path.join(root, f'{basename}.expected.h')
                print(f'Copying {src} -> {dst}')
                shutil.copyfile(src, dst)

def update_case(case_path):
    shutil.copyfile(case_path + '.out.cs', case_path + '.expected.cs')
    shutil.copyfile(case_path + '.out.h', case_path + '.expected.h')

def main():
    # Default cases directory
    default_cases_dir = os.path.join(os.path.dirname(__file__), '..', 'tests', 'MicroCom.CodeGenerator.Tests', 'cases')
    if len(sys.argv) > 1:
        # Treat argument as relative path to a test case (without .idl)
        case_path = os.path.join(default_cases_dir, sys.argv[1])
        update_case(case_path)
    else:
        update_cases(default_cases_dir)

if __name__ == '__main__':
    main()
