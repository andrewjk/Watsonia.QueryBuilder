# Watsonia.QueryBuilder #

Build SQL Queries in .NET, using string names or strongly-typed expressions.

Supports Select, Insert, Update and Delete statements.

Create a statement and run its Build function to create an object with CommandText and Parameters properties.

Pass Insert, Update and Delete commands directly to EntityFramework like so:

```C#
context.Database.ExecuteSqlCommand(command.CommandText, command.Parameters);
```

Or use with any other ORM that accepts SQL, such as Dapper.

## Select ##

Using string names:

```C#
var command =
	Select.From("Customer")
	.Columns("Name", "Code", "LicenseCount")
	.Where("Code", SqlOperator.Equals, "HI123")
	.And("BusinessNumber", SqlOperator.Equals, "123 456 789")
	.OrderBy("Name")
	.Build();
```

Results in:

```SQL
SELECT [Name], [Code], [LicenseCount]
FROM [Customer]
WHERE [Code] = @p0
AND [BusinessNumber] = @p1
ORDER BY [Name]
```

Using strongly-typed expressions:

```C#
var command =
	Select.From<Customer>()
	.Columns(c => c.Name)
	.Columns(c => c.Code)
	.Columns(c => c.LicenseCount)
	.Where(c => c.Code == "HI123")
	.And(c => c.BusinessNumber == "123 456 789")
	.OrderBy(c => c.Name)
	.Build();
```

Results in:

```SQL
SELECT [Customer].[Name], [Customer].[Code], [Customer].[LicenseCount]
FROM [Customer]
WHERE ([Customer].[Code] = @p0
AND [Customer].[BusinessNumber] = @p1)
ORDER BY [Customer].[Name]
```

## Insert##

Using string names:

```C#
var command =
	Insert.Into("Customer")
	.Value("Code", "HI123")
	.Value("Description", "Hi I'm a test value")
	.Value("BusinessNumber", "123 456 789")
	.Value("LicenseCount", 5)
	.Build();
```

Results in:

```SQL
INSERT INTO [Customer] ([Code], [Description], [BusinessNumber], [LicenseCount])
VALUES (@p0, @p1, @p2, @p3)",
```

Using strongly-typed expressions:

```C#
var command =
	Insert.Into<Customer>()
	.Value(c => c.Code, "HI123")
	.Value(c => c.Description, "Hi I'm a test value")
	.Value(c => c.BusinessNumber, "123 456 789")
	.Value(c => c.LicenseCount, 5)
	.Build();
```

Results in:

```SQL
INSERT INTO [Customer] ([Code], [Description], [BusinessNumber], [LicenseCount])
VALUES (@p0, @p1, @p2, @p3)
```

## Update##

Using string names:

```C#
var command =
	Update.Table("Customer")
	.Set("Code", "HI456")
	.Set("Description", "Hi I'm a test value")
	.Set("LicenseCount", 10)
	.Where("Code", SqlOperator.Equals, "HI123")
	.And("BusinessNumber", SqlOperator.Equals, "123 456 789")
	.Build();
```

Results in:

```SQL
UPDATE [Customer]
SET [Code] = @p0, [Description] = @p1, [LicenseCount] = @p2
WHERE [Code] = @p3
AND [BusinessNumber] = @p4
```

Using strongly-typed expressions:

```C#
var command =
	Update.Table<Customer>()
	.Set(c => c.Code, "HI456")
	.Set(c => c.Description, "Hi I'm a test value")
	.Set(c => c.LicenseCount, 10)
	.Where(c => c.Code == "HI123")
	.And(c => c.BusinessNumber == "123 456 789")
	.Build();
```

Results in:

```SQL
UPDATE [Customer]
SET [Customer].[Code] = @p0, [Customer].[Description] = @p1, [Customer].[LicenseCount] = @p2
WHERE ([Customer].[Code] = @p3
AND [Customer].[BusinessNumber] = @p4)
```

## Delete ##

Using string names:

```C#
var command =
	Delete.From("Customer")
	.Where("Code", SqlOperator.Equals, "HI123")
	.And("LicenseCount", SqlOperator.Equals, 10)
	.Build();
```

Results in:

```SQL
DELETE FROM [Customer]
WHERE [Code] = @p0
AND [LicenseCount] = @p1
```

Using strongly-typed expressions:

```C#
var command =
	Delete.From<Customer>()
	.Where(c => c.Code == "HI123")
	.And(c => c.LicenseCount == 10)
	.Build();
```

Results in:

```SQL
DELETE FROM [Customer]
WHERE ([Customer].[Code] = @p0
AND [Customer].[LicenseCount] = @p1)
```

## License ##

Watsonia.QueryBuilder is released under the terms of the [MIT License](http://opensource.org/licenses/MIT).
