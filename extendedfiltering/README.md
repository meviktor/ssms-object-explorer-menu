# Extended filtering for `MenuItem`s

## Filter string syntax

The syntax resembles to the URNs used in SQL Server. However, it uses a very small subset of the URN syntax and also have the `*` wildcard as its unique extension.

Possible object types:
- `Server`
- `Database`
- `Table`
- `Column`

Possible properties:
- `Name`: for all object types
- `Schema`: only for `Table`

If you define a segment, all properties must be defined (as the `*` wildcard as a "minimum"):

- Invalid: `Server[]`, use `Server[@Name='*']` or `Server[@Name='MyComputer']`, etc. instead.
- Also invalid: `Table[@Name='Sales']`. If you don't care the exact schema, use the form: `Table[@Name='Sales' and @Schema='*']` instead.

As the value provided for the `Name` and `Schema` properies, they have to comply to the below rules:

