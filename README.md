# Task 13

## MyFinance (BlazorUI)

MyFinance is a .Net Core Blazor server-side application for managing self finance.

It has such features:
* CRUD for 'Categories' list of types for income and expenses (e.g. salary, utilities, travelling, etc.).
* CRUD for 'Transactions' list of financial operations.
* A report for the selected date (input: a date; the result: total income for the date, total expenses for the date, a list of operations for the date).
* A report for the selected period (input: a start date, an end date; the result: total income for the period, total expenses for the period, a list of operations for the period).

Note: you should set up 'Categories' first, because the CategoryId field in 'Transactions' is required.