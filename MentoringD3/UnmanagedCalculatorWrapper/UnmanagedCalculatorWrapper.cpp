#include "stdafx.h"
#include "UnmanagedCalculatorWrapper.h"
#include "Calculator.h"

namespace CalculatorWrapperNsp {
    public ref class CalculatorWrapper {
        Calculator* nativeCalculator;

    public:
        CalculatorWrapper::CalculatorWrapper() {
            nativeCalculator = new Calculator();
        }
        CalculatorWrapper::~CalculatorWrapper() {
            delete nativeCalculator;
        }

        int  CalculatorWrapper::Add(int a, int b)
        {
            return nativeCalculator->Add(a, b);
        }
    };
}