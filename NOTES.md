# Susu Manager — Design Notes

## 1. Assumptions and Business Decisions

### Contributions
- Each group has a fixed weekly contribution amount denominated in Ghanaian cedis (GHS).
- Contributions can be made in partial payments.
- A member's payments for a week are added together until the full contribution has been paid.
- Payments must be positive and have no more than two decimal places.
- A payment cannot exceed the member's outstanding balance.

### Membership
- Each member is identified by their name.
- Member names are trimmed of surrounding whitespace and compared without case sensitivity.
- Members must be added before contributions begin.
- Members cannot be added after contributions have started because changing group membership during a savings cycle could affect payout calculations and fairness.
- At least one member must exist before a week can be closed.

### Weekly Closure and Payouts
- A week can be closed only when every member has paid the full weekly contribution.
- If any member still owes money, the week remains open and no payout is made.
- The total pot is calculated by multiplying the weekly contribution by the number of group members.
- Members receive the pot once each, following their joining order.
- The group is complete after every member has received one payout.
- The final week number remains visible after the cycle completes.

### Invalid Operations
- Invalid payments, unknown members, duplicate members, and invalid group configurations are rejected.
- Invalid operations should not change the group's financial state.

## 2. Technical Decisions

### Money Representation
The application uses C#'s `decimal` type for monetary values instead of `float` or `double`. This helps avoid the binary floating-point precision issues that can arise when representing decimal monetary amounts.

### Separation of Responsibilities
The implementation separates member and payout models from the `SusuGroup` business logic. The console interface is responsible for interacting with the user, while the group service enforces the business rules.

### Automated Testing
The project uses xUnit to test important behaviours, including partial payments, outstanding balances, payout rotation, invalid payments, and attempts to close incomplete weeks.

### In-Memory Storage
Group members, contributions, and payout history are stored in memory. This keeps the initial implementation simple and focused on business rules, but the information is lost when the application exits.

## 3. Known Limitations

- Data is not persisted between application runs.
- Only one group can be managed during a running session.
- Members are identified by names rather than separate unique IDs.
- The application does not record timestamps or a detailed audit trail for individual payments.
- There is no authentication, authorization, or multi-user access.
- The console interface is intended for a small demonstration rather than production deployment.

## 4. Potential Improvements

1. Add database persistence using Entity Framework Core and SQL Server or PostgreSQL.
2. Introduce unique member IDs to avoid relying solely on names.
3. Record individual payment transactions with dates and times.
4. Add integration tests if a database is introduced.
5. Add a more structured reporting and export feature.
6. Support multiple savings groups.
7. Add safeguards for concurrent payments if multiple users are supported.

## 5. Scope

This project prioritizes a clear, testable implementation of the core rotating savings rules. It is a learning and demonstration application and is not intended to be a production-ready financial system.
