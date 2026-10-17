# Susu Manager

A console-based rotating savings group management application built with C# and .NET 9.

## Overview

Susu Manager helps manage a rotating savings group in which members contribute a fixed amount each week. When every member has paid their full weekly contribution, the application distributes the total pot to the next eligible member in joining order.

The application is designed to demonstrate object-oriented programming, business rule validation, exact monetary calculations, automated testing, and Git version control.

## Features

- Create a group with a fixed weekly contribution in Ghanaian cedis (GHS).
- Add members with unique names.
- Record full or partial member contributions.
- Track outstanding balances and members who have paid in full.
- Prevent overpayments and invalid contribution amounts.
- Close a week only when all members have paid in full.
- Distribute the pot in joining order.
- Display payout history and the group's current status.
- Run automated unit tests.

## Technologies

- C#
- .NET 9
- xUnit
- Git

## Prerequisites

Install the .NET 9 SDK before running the application.

Verify your installation:

```bash
dotnet --version
```

## Getting Started

Clone the repository and navigate to the project directory:

```bash
git clone https://github.com/jacobOpintan/SusuManager.git 
cd SusuManager
```

Build the solution:

```bash
dotnet build
```

Run the application:

```bash
dotnet run --project SusuManager/SusuManager.csproj
```

Run the automated tests:

```bash
dotnet test
```

## How It Works

1. Enter the group's fixed weekly contribution.
2. Add the members in the order they should receive the pot.
3. Record each member's full or partial contribution.
4. Review payment statuses and outstanding balances.
5. Close the current week when all members have paid in full.
6. Review the payout history and continue to the next week until the cycle is complete.

## Business Rules

- The weekly contribution must be positive and have no more than two decimal places.
- Member names must be unique, ignoring capitalization and surrounding whitespace.
- Members cannot be added after contributions begin.
- Payments must be positive and have no more than two decimal places.
- Payments cannot exceed the member's outstanding balance.
- A week cannot close while any member still owes money.
- Payouts follow the order in which members joined.
- The cycle completes after every member has received one payout.

## Testing

The solution includes automated unit tests for partial payments, outstanding balances, payout rotation, week closure, duplicate members, invalid payments, and other important business rules.

Run all tests using:

```bash
dotnet test
```

## Limitations

- Group data is held in memory and is lost when the application exits.
- The application supports one group per running session.
- The application has a console interface; no web or graphical interface is provided.

## Future Improvements

- Persist group, contribution, and payout data in a database.
- Support multiple groups.
- Add member IDs and more detailed transaction records.
- Introduce contribution and payout reports that can be exported.
- Add integration tests if persistent storage is introduced.

## License

No license has been specified for this project yet.