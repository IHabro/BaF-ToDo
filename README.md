# ToDo REST API application for interview purposes

Assignment for company: Be a Future in Ostrava.

The solution is divided into 2 projects: BaF-ToDo which contains the REST API, and BaF-ToDo-Unit-Testing which includes all unit tests for responsible controller.

## BaF-ToDo

The project is based on .NET 8.0 as this version contains Swagger API documentation by default, I wanted to use .NET 10.0, but decided that I am a little lazy with NuGet installation. The core principle and design idea behind it should not change with the framework version, though.

The project is subdivided into these 4 parts: Controller, Models, Repositories folders and Program.cs class that is present within the root folder of the API Project. I contemplated adding another folder called "Helpers" which would contain all of 1 interface and custom ValidationAttribute: **NotEmptyOrWhitespaceAttribute**. I, however, decided againtst it as the project is already a bit overengineered.

### Logic and Architecture decitions

As we discussed during the interview, I used Repository for unified access to the Data Layer, which in this case is an in-memory DbContext containing a single DbSet<TaskEntity>. The repository does not care about any validation or error handling, which in retrospect, probably should have its own EF error handling and recovery, outside the generic one that is present and configured in Program.cs.

The models folder contains separate DTO classes for each required CRUD operation, eg. **Create** and **Update** as operations for **Read** and **Delete** usually use just and **id** instead of a whole object. AS such, there is a **CreateTaskEntityDTO** and **UpdateTaskEntityDTO**. Outside these, there is an **TaskEntityDTO** which is supplied to the User when he requests any data. Lastly, there are definitions of **TaskEntityDbContext** and **TaskEntity** classes. The latter also contains 2 methods: **MarkCompleted** and **ToDTO**. Both of those method names are quite self-explanatory, as is the rest of the code, so I will not go into too much detail.

Lastly, it is a **TaskEntityController** which contain these endpoints: GetTasks, GetTaskById, CreateTask, UpdateTask, DeleteTask and CompleteTask. Also most of them are self-explanatory.

## BaF-ToDo-Unit-Testing

This project includes a single class with 12 testts. These tests try to simulate basic code coverage (all branching returning types) as well as a few basic edge cases, such as **null** values. The program has potential for other unit testing, but taking into account the scope and purpose of this assignment, I decided not to add them.

Needless to say that I have limited prior knowledge of Enterprise SW testing, so I mostly followed best-practices and tutorials that I found in the dotnet community. Same can be said about the general erro handling for **Unhandled exception occurred**, as my first instinct was to add erro handling into each method separately, which would have caused unproductive delay and code bloating.

## Declaration of AI use

Visual Studio has its own Intelisense and code completion algortihms as well as scaffolding, which I used to create the first version of **TaskEntityController** otherwise, I mainly used Claude Sonnet 4.6 to consult and discuss problems I had, most of which revolved around non-function tests and error handling. Basically, using it as a glorified StackOverFlow and Goodle Search Engine.
