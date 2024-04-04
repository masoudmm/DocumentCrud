# Document CRUD Application OverView
This project is designed to manage accounting documents, including Invoices, Independent Credit Notes, and Dependent Credit Notes. The application is structured using Onion Architecture and includes a simple front-end implementation using Angular and Bootstrap.


# Prerequisites

## Database setup
I personally used SQL Server 2019 for the project and the tests, you can navigate to DocumentCrud.WebAPI project and change the connection string value in AppSettings.json file in DocumentCrud.WebAPI project.
Currently it is set like this:
```bash
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=documentCrud;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
},
```


## Clone the repository and navigate to directory
```bash
git clone https://github.com/masoudmm/DocumentCrud.git
```


## Navigate to the backend project directory
```bash
cd DocumentCrud/src/DocumentCrud.WebAPI
```


## Restore .NET Core packages & Run
```bash
dotnet restore
```
```bash
dotnet run
```


# Frontend setup
## Navigate to the frontend project directory from the root of the repository
```bash
cd src/DocumentCrud.UI
```

## Install NPM packages & Run Angular application
```bash
npm install
```
```bash
ng serve -o
```

## Running the tests
```bash
cd tests/DocumentCrud.Tests
```
```bash
dotnet test
```
