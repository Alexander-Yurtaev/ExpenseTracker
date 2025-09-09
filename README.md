# ExpenseTracker
********************************
Уровень: Beginner
Тип: CLI
Технологии: C#, .NET9
Источник задания: (ExpenseTracker)[https://roadmap.sh/projects/expense-tracker]
Статус: завершён

Простое приложение для отслеживания расходов для управления вашими финансами. 
Приложение позволяет пользователям добавлять, удалять и просматривать свои расходы. 
Приложение также предоставляет сводную информацию о расходах.

- Пользователи могут добавлять расходы с описанием и суммой.
- Пользователи могут обновлять расходы.
- Пользователи могут удалять расходы.
- Пользователи могут просматривать все расходы.
- Пользователи могут просматривать сводную информацию обо всех расходах.
- Пользователи могут просматривать сводную информацию о расходах за определенный месяц (текущего года).

## Example

```shell
expense-tracker add --description "Lunch" --amount 20
```

### Expense added successfully (ID: 1)
```shell
expense-tracker add --description "Dinner" --amount 10
```

### Expense added successfully (ID: 2)
```shell
expense-tracker list
```
```
# ID  Date       Description  Amount
# 1   2024-08-06  Lunch        $20
# 2   2024-08-06  Dinner       $10
```

```shell
expense-tracker summary
```
``` Total expenses: $30 ```

```shell
expense-tracker delete --id 2
```
``` Expense deleted successfully ```

```shell
$ expense-tracker summary
```
```# Total expenses: $20```


```shell
expense-tracker summary --month 8
```

``` Total expenses for August: $20```

********************************
Level: Beginner
Type: CLI
Technology: C#, .NET9
Source: (ExpenseTracker)[https://roadmap.sh/projects/expense-tracker]
Stage: Done

A simple expense tracker application to manage your finances. 
The application allow users to add, delete, and view their expenses. 
The application also provide a summary of the expenses.

- Users can add an expense with a description and amount.
- Users can update an expense.
- Users can delete an expense.
- Users can view all expenses.
- Users can view a summary of all expenses.
- Users can view a summary of expenses for a specific month (of current year).

## Example

```shell
expense-tracker add --description "Lunch" --amount 20
```

### Expense added successfully (ID: 1)
```shell
expense-tracker add --description "Dinner" --amount 10
```

### Expense added successfully (ID: 2)
```shell
expense-tracker list
```
```
# ID  Date       Description  Amount
# 1   2024-08-06  Lunch        $20
# 2   2024-08-06  Dinner       $10
```

```shell
expense-tracker summary
```
``` Total expenses: $30 ```

```shell
expense-tracker delete --id 2
```
``` Expense deleted successfully ```

```shell
$ expense-tracker summary
```
```# Total expenses: $20```


```shell
expense-tracker summary --month 8
```

``` Total expenses for August: $20```