# HouseBroker Unit Tests

This project contains comprehensive unit tests for the HouseBroker application, covering commission logic, listing service logic, and controller tests with mocked dependencies.

## Test Structure

### 📁 Services Tests
- **CommissionServiceTests.cs** - Tests for commission calculation logic
  - Commission rate calculation with different price ranges
  - Database vs default rate fallback logic
  - Active/inactive rate filtering
  - Commission calculation accuracy

### 📁 Repositories Tests  
- **PropertyRepositoryTests.cs** - Tests for property listing and search logic
  - CRUD operations (Create, Read, Update, Delete)
  - Search functionality with various filters
  - Soft delete implementation
  - Featured properties filtering
  - Broker-specific property queries

### 📁 Controllers Tests
- **PropertiesControllerTests.cs** - Tests for Properties API endpoints
  - Cache hit/miss scenarios
  - CRUD operations with proper HTTP status codes
  - Authentication and authorization
  - Input validation
  - Search functionality

- **AuthControllerTests.cs** - Tests for Authentication API endpoints
  - User registration with role validation
  - Login with JWT token generation
  - Error handling for invalid credentials
  - User status validation

### 📁 Helpers
- **TestDataHelper.cs** - Common test utilities and sample data
  - Sample property creation
  - Sample commission rate creation
  - Database seeding utilities
  - In-memory database setup

## Test Coverage

### Commission Logic Tests
✅ **Default Commission Rates**
- Properties under 50 lakhs: 2%
- Properties 50 lakhs to 1 crore: 1.75%
- Properties over 1 crore: 1.5%

✅ **Database Commission Rates**
- Active rate lookup by price range
- Inactive rate filtering
- Fallback to default rates

✅ **Commission Calculation**
- Accurate percentage calculations
- Edge cases (zero price, very high prices)

### Listing Service Logic Tests
✅ **Property Repository Operations**
- Get by ID (including soft delete filtering)
- Get all properties (excluding deleted)
- Add new properties
- Update existing properties
- Soft delete properties

✅ **Search Functionality**
- City-based filtering
- Price range filtering
- Multiple filter combinations
- Sorting and pagination

✅ **Special Queries**
- Featured properties
- Broker-specific properties
- Status-based filtering

### Controller Tests
✅ **Properties Controller**
- Cache behavior (hit/miss scenarios)
- HTTP status codes for all operations
- Authentication requirements
- Input validation
- Error handling

✅ **Auth Controller**
- User registration flow
- Login authentication
- JWT token generation
- Role-based access control
- Error scenarios

## Running Tests

### Prerequisites
- .NET 8.0 SDK
- Visual Studio 2022 or VS Code

### Run All Tests
```bash
dotnet test
```

### Run Specific Test Categories
```bash
# Run only service tests
dotnet test --filter "Category=Services"

# Run only controller tests  
dotnet test --filter "Category=Controllers"

# Run only repository tests
dotnet test --filter "Category=Repositories"
```

### Run Tests with Coverage
```bash
dotnet test --collect:"XPlat Code Coverage"
```

### Run Tests in Parallel
```bash
dotnet test --maxcpucount:4
```

## Test Dependencies

- **xUnit** - Testing framework
- **Moq** - Mocking framework for dependencies
- **FluentAssertions** - Readable assertions
- **Microsoft.EntityFrameworkCore.InMemory** - In-memory database for testing
- **Microsoft.AspNetCore.Mvc.Testing** - Integration testing support

## Best Practices Implemented

### 🎯 **Arrange-Act-Assert Pattern**
All tests follow the AAA pattern for clear structure and readability.

### 🔄 **Isolated Tests**
Each test is independent and doesn't rely on other tests' state.

### 🗄️ **In-Memory Database**
Uses Entity Framework's in-memory provider for fast, isolated database tests.

### 🎭 **Mocked Dependencies**
External dependencies are properly mocked to ensure unit test isolation.

### 📊 **Comprehensive Coverage**
Tests cover happy paths, edge cases, and error scenarios.

### 🧹 **Test Data Helpers**
Reusable test data creation utilities to reduce code duplication.

## Adding New Tests

When adding new tests:

1. **Follow Naming Convention**: `MethodName_Scenario_ExpectedResult`
2. **Use TestDataHelper**: For creating sample data
3. **Mock Dependencies**: Use Moq for external dependencies
4. **Test Edge Cases**: Include boundary conditions and error scenarios
5. **Use FluentAssertions**: For readable assertions

## Example Test Structure

```csharp
[Fact]
public async Task MethodName_WithValidInput_ReturnsExpectedResult()
{
    // Arrange
    var input = TestDataHelper.CreateSampleData();
    _mockService.Setup(x => x.Method(input)).ReturnsAsync(expectedResult);

    // Act
    var result = await _service.Method(input);

    // Assert
    result.Should().NotBeNull();
    result.Property.Should().Be(expectedValue);
    _mockService.Verify(x => x.Method(input), Times.Once);
}
```

## Continuous Integration

These tests are designed to run in CI/CD pipelines and provide:
- Fast execution (under 30 seconds for full suite)
- Reliable results (no external dependencies)
- Clear failure messages
- Coverage reporting 