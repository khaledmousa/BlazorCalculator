# BlazorCalculator

A simple Blazor calculator utilizing a reference to [RuntimeExpressions](https://github.com/khaledmousa/RuntimeExpressions) to parse simple expressions.

See [Demo](https://khaledmousa.github.io/)

The idea is to be able to write dependent expressions and have the evaluation of one variable propagate through all dependent variables (e.g. `x = y + 5` followed by `y = 5` will update `x` to 10).
